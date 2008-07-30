using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using DSLFactory.Candle.SystemModel.Utilities;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using System.ComponentModel;
using DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover;
using DSLFactory.Candle.SystemModel.Strategies.NHibernate;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Candle strategy for generating a DAL using nhibernate
    /// </summary>
    [Strategy(NHibernateStrategy.ModelsNHibernateStrategyID)]
    public class NHibernateStrategy : StrategyBase, IStrategyCodeGenerator, IStrategyAddElementInterceptor, IStrategyValidator
    {
        private const string ModelsNHibernateStrategyID = "0811B1D6-7055-43a3-AA51-E95732F4E8C5";

        private string _daoTemplate = "NHibernateDALStandAlone";
        private string _template = "NHibernateEntityEx";
        private string _enumTemplate = "EnumEntity";
        private bool _generateHbmFile = true;
        private bool _generateConfigurationSettings = true;
        private bool _generateHttpModuleConfiguration = true;
        private VSLangProj.prjBuildAction _hbmFileBuildAction = VSLangProj.prjBuildAction.prjBuildActionNone;

        #region Dependency properties
        // Déclarations

        /// <summary>
        /// Is the entity persistable ?
        /// </summary>
        public static readonly DependencyProperty<bool> EntityIsPersistableProperty;

        /// <summary>
        /// ID generator settings
        /// </summary>
        public static readonly DependencyProperty<GeneratorInfo> EntityIdGeneratorProperty;

        /// <summary>
        /// Enable lazy loading ?
        /// </summary>
        public static readonly DependencyProperty<bool> AssociationLazyLoadingProperty;

        public static readonly DependencyProperty<bool> AssociationUpdateProperty;
        public static readonly DependencyProperty<bool> AssociationInsertProperty;
        public static readonly DependencyProperty<bool> AssociationOuterJoinProperty;

        /// <summary>
        /// Instanciation
        /// </summary>
        static NHibernateStrategy()
        {
            EntityIsPersistableProperty = new DependencyProperty<bool>(ModelsNHibernateStrategyID, "Persistable");
            EntityIsPersistableProperty.Category = typeof(NHibernateStrategy).Name;
            EntityIsPersistableProperty.DefaultValue = true;

            EntityIdGeneratorProperty = new DependencyProperty<GeneratorInfo>(ModelsNHibernateStrategyID, "Generator");
            EntityIdGeneratorProperty.Category = typeof(NHibernateStrategy).Name;
            EntityIdGeneratorProperty.TypeEditor = typeof(DSLFactory.Candle.SystemModel.Strategies.NHibernate.GeneratorInfoTypeEditor);

            AssociationLazyLoadingProperty = new DependencyProperty<bool>(ModelsNHibernateStrategyID, "LazyLoading");
            AssociationLazyLoadingProperty.Category = typeof(NHibernateStrategy).Name;
            
            AssociationInsertProperty = new DependencyProperty<bool>(ModelsNHibernateStrategyID, "CanInsert");
            AssociationInsertProperty.Category = typeof(NHibernateStrategy).Name;
            AssociationInsertProperty.DisplayName = "Can insert";

            AssociationUpdateProperty = new DependencyProperty<bool>(ModelsNHibernateStrategyID, "CanUpdate");
            AssociationUpdateProperty.Category = typeof(NHibernateStrategy).Name;
            AssociationUpdateProperty.DisplayName = "Can update";

            AssociationOuterJoinProperty = new DependencyProperty<bool>(ModelsNHibernateStrategyID, "UseOuterJoin");
            AssociationOuterJoinProperty.DisplayName = "Use outer join";
            AssociationOuterJoinProperty.Category = typeof(NHibernateStrategy).Name;
        }

        /// <summary>
        /// Dynamic association of a dependency propertie with a model element.
        /// </summary>
        /// <param name="modelElement"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetCustomProperties(ModelElement modelElement)
        {
            PropertyDescriptorCollection collections = base.GetCustomProperties(modelElement);

            if (modelElement is Entity)
            {
                Entity clazz = modelElement as Entity;
                if (clazz.SuperClass == null && !clazz.IsAbstract)
                {
                    collections.Add(EntityIsPersistableProperty.Register(clazz));
                    if (clazz.PrimaryKeys.Count == 1)
                        collections.Add(EntityIdGeneratorProperty.Register(clazz));
                }
            }
            else if (modelElement is DSLFactory.Candle.SystemModel.Property)
            {
            }
            else if (modelElement is Association)
            {
                collections.Add(AssociationLazyLoadingProperty.Register(modelElement as Association));
                collections.Add(AssociationOuterJoinProperty.Register(modelElement as Association));
                collections.Add(AssociationInsertProperty.Register(modelElement as Association));
                collections.Add(AssociationUpdateProperty.Register(modelElement as Association));
            }

            return collections;
        }
        #endregion

        #region Strategy properties

        public VSLangProj.prjBuildAction HbmFileBuildAction
        {
            get { return _hbmFileBuildAction; }
            set { _hbmFileBuildAction = value; }
        }
	
        [Description("Generate a predefined session manager")]
        public bool GeneratePredefinedSessionManager
        {
            get { return _generateHttpModuleConfiguration; }
            set { _generateHttpModuleConfiguration = value; }
        }

        [Description("Generate a predefined nhibernate configuration")]
        public bool GenerateConfigurationSettings
        {
            get { return _generateConfigurationSettings; }
            set { _generateConfigurationSettings = value; }
        }
	
        public bool GenerateHbmFile
        {
            get { return _generateHbmFile; }
            set { _generateHbmFile = value; }
        }
	
        /// <summary>
        /// Name of the dao template file without extension
        /// </summary>
        [Description("Name of the dao template file without extension")]
        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DAOTemplate
        {
            get { return _daoTemplate; }
            set { _daoTemplate = value; }
        }

        public string EnumTemplate
        {
            get { return _enumTemplate; }
            set { _enumTemplate = value; }
        }
	
        /// <summary>
        /// Name of the entity template file without extension
        /// </summary>
        [Description("Name of the entity template file without extension")]
        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string EntityTemplate
        {
            get { return _template; }
            set { _template = value; }
        }

        #endregion

        #region ILayerStrategy Members

        // Création du fichier de mapping nhibernate en utilisant le mécanisme de 
        // mappage filtrée
        public void Execute()
        {

            try
            {
                DataLayer ml = CurrentElement as DataLayer;
                if (ml != null && Context.GenerationPass == GenerationPass.MetaModelUpdate && GenerateConfigurationSettings)
                {
                    GenerateNHibernateConfiguration(ml);

                    return;
                }

                PresentationLayer layer = CurrentElement as PresentationLayer;
                if (layer != null && Context.GenerationPass == GenerationPass.MetaModelUpdate)
                {
                    if (layer.HostingContext == HostingContext.Web && GeneratePredefinedSessionManager)
                    {
                        layer.AddXmlConfigurationContent("nhibernatehttpModule", @"<configuration>
<system.web><httpModules><add name=""NHibernateSessionModule"" type=""NHibernate.Web.NHibernateSessionModule""/></httpModules></system.web>
</configuration>");
                    }
                    else
                    {
                        layer.AddXmlConfigurationContent("nhibernatehttpModule", null);
                    }
                    return;
                }

                // DAO
                if (CurrentElement is DataAccessLayer && GeneratePredefinedSessionManager)
                {
                    DataAccessLayer dal = CurrentElement as DataAccessLayer;
                    CallT4Template(Context.Project, "NHibernateSessionManager", dal, "NHibernateSessionManager.cs");
                    PresentationLayer l = dal.Component.GetMainLayer() as PresentationLayer;
                    if (l != null && l.HostingContext == HostingContext.Web)
                        CallT4Template(Context.Project, "NHibernateHttpModule", dal, "NHibernateHttpModule.cs");
                    return;
                }

                ClassImplementation clazz = CurrentElement as ClassImplementation;
                if (clazz != null && !String.IsNullOrEmpty(DAOTemplate))
                {
                    DataAccessLayer dal = clazz.Layer as DataAccessLayer;
                    if (dal != null && dal.SoftwareComponent.IsDataLayerExists)
                    {
                        TemplateProperties properties = new TemplateProperties();
                        CallT4Template(Context.Project, DAOTemplate, clazz, clazz.Name + "Base");
                    }
                    return;
                }

                Entity entity = CurrentElement as Entity;
                if (entity != null && EntityIsPersistableProperty.GetValue(entity) && !String.IsNullOrEmpty(EntityTemplate))
                {
                    CallT4Template(Context.Project, EntityTemplate, entity, entity.Name);
                }

                Enumeration enumeration = CurrentElement as Enumeration;
                if (enumeration != null && !String.IsNullOrEmpty(EnumTemplate))
                {
                    CallT4Template(Context.Project, EnumTemplate, enumeration, enumeration.Name);
                }

                // Mappings
                if (CurrentElement is DataLayer && Context.GenerationPass == GenerationPass.CodeGeneration && GenerateHbmFile)
                {
                    // Nom du fichier de sortie
                    string fileName = String.Format("{0}.hbm.xml", System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Context.Project.FileName), Context.Project.Name));
                    NHibernate3Strategy strat = new NHibernate3Strategy();
                    if (File.Exists(fileName))
                    {
                        ServiceLocator.Instance.ShellHelper.EnsureCheckout(fileName);
                    }

                    strat.Execute(Context, fileName);

                    if (File.Exists(fileName))
                    {
                        ProjectItem item = ServiceLocator.Instance.ShellHelper.AddFileToProject(Context.Project, fileName);

                        try
                        {
                            item.Properties.Item("BuildAction").Value = _hbmFileBuildAction;
                        }
                        catch { }

                        if (_hbmFileBuildAction != VSLangProj.prjBuildAction.prjBuildActionEmbeddedResource)
                        {
                            ((DataLayer)CurrentElement).AddReference(fileName, ArtifactType.Content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static void GenerateNHibernateConfiguration(DataLayer ml)
        {
            ConfigurationPart cnfPart = ml.Configurations.Find(delegate(ConfigurationPart conf) { return conf.Name == "ConnectionStrings"; });
            bool isOracle = cnfPart != null && cnfPart.XmlContent != null && cnfPart.XmlContent.ToLower().IndexOf("Oracle") > 0;
            StringBuilder cnf = new StringBuilder();
            cnf.AppendLine(@"<configuration>");
            cnf.AppendLine(@"<configSections>");
            cnf.AppendLine(@"<section name=""hibernate-configuration"" type=""NHibernate.Cfg.ConfigurationSectionHandler, NHibernate""/>");
            cnf.AppendLine(@"</configSections>");
            cnf.AppendLine(@"<hibernate-configuration xmlns=""urn:nhibernate-configuration-2.0"">");
            cnf.AppendLine(@"<session-factory>");
            cnf.AppendLine(@"<property name=""connection.provider"">NHibernate.Connection.DriverConnectionProvider</property>");
            cnf.AppendLine(@"<property name=""connection.isolation"">ReadCommitted</property>");

            if (!isOracle)
            {
                cnf.AppendLine(@"<property name=""dialect"">NHibernate.Dialect.MsSql2000Dialect</property>");
                cnf.AppendLine(@"<property name=""connection.driver_class"">NHibernate.Driver.SqlClientDriver</property>");
            }
            else
            {
                cnf.AppendLine(@"<property name=""dialect"">NHibernate.Dialect.Oracle9Dialect</property>");
                cnf.AppendLine(@"<property name=""connection.driver_class"">NHibernate.Driver.OracleDataClientDriver</property>");
            }

            cnf.AppendLine(@"<!-- HBM Mapping Files -->");
            cnf.AppendLine(@"<mapping assembly=""" + ml.AssemblyName + @"""/>");
            cnf.AppendLine(@"</session-factory>");
            cnf.AppendLine(@"</hibernate-configuration></configuration>");

            ml.AddXmlConfigurationContent("nhibernateconfig", cnf.ToString());
        }

        public override bool IsModelGenerationExclusive(GenerationContext context, ICustomizableElement currentElement, string generatedFileName)
        {
            // On génére toutes les classes DA0
            if (context.GenerationPass == GenerationPass.CodeGeneration)
            {
                if (currentElement is ClassImplementation && Utils.StringCompareEquals(Path.GetFileNameWithoutExtension(generatedFileName), currentElement.Name + "Base"))
                    return ((ClassImplementation)currentElement).Layer is DataAccessLayer;
            }

            // Les enums
            if (currentElement is Enumeration)
                return true;

            // Et toutes les entités persistables
            if (currentElement is Entity && EntityIsPersistableProperty.GetValue(currentElement))
                return true;

            return base.IsModelGenerationExclusive(context, currentElement, generatedFileName);
        }
        #endregion

        public void OnElementAdded(ICustomizableElement owner, StrategyElementElementAddedEventArgs e)
        {
            Transaction trans = e.ModelElement.Store.TransactionManager.CurrentTransaction;

            object obj;
            if (owner is Entity)
            {
                Entity entity = (Entity)owner;
                if (trans.Context.ContextInfo.TryGetValue(DatabaseImporter.ImportedTableInfo, out obj))
                {
                    DbTable table = obj as DbTable;
                    EntityIsPersistableProperty.SetValue(owner, true);
                }
            }

            if (owner is Property)
            {
            }

            if (owner is Association)
            {
                Association association = owner as Association;
                if (trans.Context.ContextInfo.TryGetValue(DatabaseImporter.ImportedRelationInfo, out obj))
                {
                    //RelationShip relation = obj as RelationShip;
                    //StringBuilder sb = new StringBuilder();
                    //for( int i =0; i<relation.TargetColumns.Count; i++ )
                    //{
                    //    sb.Append( relation.TargetColumns[i].Name );
                    //    if( i<relation.TargetColumns.Count-1 )
                    //        sb.Append( ',' );
                    //}
                    //AssociationFKColumnNameProperty.SetValue( owner, sb.ToString() );
                }
            }
        }

        public IStrategyWizard GetWizard(ModelElement model)
        {
            return null;
        }

        #region IStrategyValidator Members

        public void Validate(ModelElement element, ValidationContext logInfo)
        {
            if (element is Entity)
            {
                Entity clazz = element as Entity;
                if (clazz.SuperClass == null && !clazz.IsAbstract)
                {
                    if (clazz.PrimaryKeys.Count == 1)
                        if (!EntityIdGeneratorProperty.HasValue(clazz) || String.IsNullOrEmpty(EntityIdGeneratorProperty.GetValue(clazz).Name))
                            logInfo.LogError("Generator is not defined for " + clazz.FullName, "NHI001", element);
                }
            }
        }

        #endregion
    }

    public class NHibernate3Strategy
    {
        protected DataLayer _modelsLayer;
        private SafeStreamWriter writer;
        private ClassInheritanceTree _inheritanceTree;

        /// <summary>
        /// Ecriture dans le fichier xml avec une indentation
        /// </summary>
        /// <param name="level">Niveau d'indentation</param>
        /// <param name="txt">Texte à écrire</param>
        private void WriteLineWithIndent(int level, string txt)
        {
            string s = new String(' ', level * 4);
            this.Write(s);
            this.WriteLine(txt);
        }

        private void WriteLine(string txt)
        {
            writer.WriteLine(txt);
        }

        private void Write(string txt)
        {
            writer.Write(txt);
        }


        /// <summary>
        /// Génération du mapping d'une classe
        /// </summary>
        /// <param name="root">Classe sur lequel s'effectue le mapping</param>
        /// <param name="level">Niveau d'indentation dans le fichier de sortie</param>
        private void GenerateClassMapping(ClassInheritanceNode root, int level, int discriminator)
        {
            if (root.IsExternal)
                return;

            // Si cette classe n'est pas persistable, on ne fait rien y compris sur ses sous classes
            bool isPersistable = NHibernateStrategy.EntityIsPersistableProperty.GetValue(root.clazz);
            if (isPersistable == false)
                return;

            string endTag = "</class>";
            if (level == 1)
            {
                bool isRootClass = root.childs.Count > 0;
                string tableName = root.clazz.TableName;
                if (String.IsNullOrEmpty(tableName))
                    tableName = root.clazz.Name;
                string schema = root.clazz.TableOwner;
                if (!String.IsNullOrEmpty(schema))
                    schema = String.Format("schema=\"{0}\"", schema);

                this.WriteLine("");
                string comment = String.Format("<!--                        Mapping de la classe {0}                  -->", root.clazz.Name);
                string asterix = new string('*', comment.Length - 9);
                this.WriteLineWithIndent(0, String.Format("<!-- {0} -->", asterix));
                this.WriteLineWithIndent(0, comment);
                this.WriteLineWithIndent(0, String.Format("<!-- {0} -->", asterix));
                this.WriteLineWithIndent(0, String.Format("<class name=\"{0}\" table=\"{1}\" {2}>", root.clazz.AssemblyQualifiedName, tableName, (isRootClass ? " discriminator-value=\"0\"" : "")));

                // Identifiant
                if (root.clazz.PrimaryKeys.Count == 0)
                {
                    this.WriteLineWithIndent(1, "<id><generator/></id>");
                }
                else if (root.clazz.PrimaryKeys.Count == 1)
                {
                    Property prop = root.clazz.PrimaryKeys[0];
                    string columnName = prop.ColumnName;
                    if (string.IsNullOrEmpty(columnName))
                        columnName = StrategyManager.GetInstance(root.clazz.Store).NamingStrategy.CreateSQLColumnName(root.clazz.Name, prop.Name);
                    this.WriteLineWithIndent(1, String.Format("<id name=\"{0}\" column=\"{1}\">", prop.Name, columnName));

                    GeneratorInfo gi = NHibernateStrategy.EntityIdGeneratorProperty.GetValue(root.clazz);
                    if (gi != null)
                    {
                        this.WriteLineWithIndent(2, String.Format("<generator class=\"{0}\">", gi.Name));
                        foreach (GeneratorInfo.GeneratorParm parm in gi.Parms)
                        {
                            this.WriteLineWithIndent(3, String.Format("<param name=\"{0}\">{1}</param>", parm.Name, parm.Value));
                        }
                        this.WriteLineWithIndent(2, "</generator>");
                    }
                    this.WriteLineWithIndent(1, "</id>");
                }
                else // Count > 1
                {
                    this.WriteLineWithIndent(1, String.Format("<composite-id name=\"Key\" class=\"{0}+PrimaryKey,{1}\">", root.clazz.FullName, root.clazz.DataLayer.AssemblyName));
                    foreach (Property property in root.clazz.PrimaryKeys)
                    {
                        string columnName = property.ColumnName;
                        if (string.IsNullOrEmpty(columnName))
                            columnName = StrategyManager.GetInstance(root.clazz.Store).NamingStrategy.CreateSQLColumnName(root.clazz.Name, property.Name);

                        this.WriteLineWithIndent(2, String.Format("<key-property name=\"{0}\" column=\"{1}\"/>", property.Name, columnName));
                    }
                    this.WriteLineWithIndent(1, "</composite-id>");
                }

                if (isRootClass)
                {
                    this.WriteLineWithIndent(1, "<!-- Mapping utilise la stratégie de mapping filtré -->");
                    this.WriteLineWithIndent(1, "<discriminator column=\"DiscriminatorValue\" type=\"Int16\" force=\"true\"/>");
                }
            }
            else
            {
                endTag = "</subclass>";
                this.WriteLine("");
                string comment = String.Format("<!--                       sub-class mapping : {0}                  -->", root.clazz.Name);
                string asterix = new string('*', comment.Length - 9);
                this.WriteLineWithIndent(0, String.Format("<!-- {0} -->", asterix));
                this.WriteLineWithIndent(0, comment);
                this.WriteLineWithIndent(0, String.Format("<!-- {0} -->", asterix));
                this.WriteLineWithIndent(level, String.Format("<subclass name=\"{0}\" discriminator-value=\"{1}\" >", root.clazz.AssemblyQualifiedName, (1 << (discriminator - 1))));
            }

            // Génération des propriétés de la classe
            GenerateMappingProperties(level, root.clazz, null);

            // Génération des sous classes
            foreach (ClassInheritanceNode childNode in root.childs)
            {
                GenerateClassMapping(childNode, level + 1, discriminator);
                discriminator++;
            }

            // Fermeture du tag
            this.WriteLineWithIndent(level - 1, endTag);
        }

        /// <summary>
        /// Génération du mapping des propriétés
        /// </summary>
        /// <param name="level"></param>
        /// <param name="root"></param>
        /// <param name="componentName"></param>
        private void GenerateMappingProperties(int level, Entity clazz, string componentName)
        {
            this.WriteLineWithIndent(level, "<!--  Properties    -->");

            foreach (DSLFactory.Candle.SystemModel.Property prop in clazz.Properties)
            {
                if (prop.IsPrimaryKey || prop.IsForeignKey)
                    continue;

                string typeName = prop.Type;
                DataType refType = _inheritanceTree.FindEntityByName(prop.Type);
                if (refType != null)
                {
                    typeName = refType.AssemblyQualifiedName;
                }
                else
                {
                    // C'est peut-être une énumération
                    refType = clazz.DataLayer.FindType(prop.Type) as Enumeration;
                    if (refType != null)
                        typeName = refType.FullName;
                }

                string columnName = prop.ColumnName;
                if (string.IsNullOrEmpty(columnName))
                    columnName = StrategyManager.GetInstance(clazz.Store).NamingStrategy.CreateSQLColumnName(componentName, prop.Name);

                // Propriété simple
                IList<string> types = ClrTypeParser.GetModelNamesFromClrType(prop.Type);
                if (types == null || types.Count == 0 || refType is Enumeration)
                {
                    this.WriteLineWithIndent(level, String.Format("<property name=\"{0}\" column=\"{1}\" type=\"{2}\"/>", prop.Name, columnName, typeName));
                }
                else
                {
                    // Component
                    this.WriteLineWithIndent(level, String.Format("<component name=\"{0}\" class=\"{1}\" >", prop.Name, typeName));

                    Entity entity = refType as Entity;
                    if (entity != null)
                    {
                        GenerateMappingProperties(level + 1, entity, columnName);

                        // Propriétés des classes de bases
                        while (entity.SuperClass != null)
                        {
                            entity = entity.SuperClass;
                            GenerateMappingProperties(level + 1, entity, columnName);
                        }
                    }
                    this.WriteLineWithIndent(level, "</component>");
                }
            }


            IList<Association> list = Association.GetLinksToTargets(clazz);
            foreach (Association association in list)
            {
                if (association.SourceMultiplicity == Multiplicity.OneMany || association.SourceMultiplicity == Multiplicity.ZeroMany)
                {
                    string lazy = NHibernateStrategy.AssociationLazyLoadingProperty.GetValue(association).ToString().ToLower();

                    this.WriteLine("");
                    this.WriteLineWithIndent(level, "<!-- Relation  0..* -->");
                    this.WriteLineWithIndent(level, "<bag ");
                    this.WriteLineWithIndent(level, String.Format("    name=\"{0}\" inverse=\"true\" lazy=\"{1}\" >", association.SourceRoleName, lazy));
                    this.WriteLineWithIndent(level, String.Format("    <key column=\"{0}\"/>", association.SourceRoleName));
                    this.WriteLineWithIndent(level, String.Format("    <one-to-many class=\"{0}\" />", association.Target.AssemblyQualifiedName));
                    this.WriteLineWithIndent(level, "</bag>");
                }
                else if (association.SourceMultiplicity != Multiplicity.NotApplicable)
                {
                    string insert = NHibernateStrategy.AssociationInsertProperty.GetValue(association).ToString().ToLower();
                    string update = NHibernateStrategy.AssociationUpdateProperty.GetValue(association).ToString().ToLower();
                    string outerJoin = NHibernateStrategy.AssociationOuterJoinProperty.GetValue(association).ToString().ToLower();
                    this.WriteLineWithIndent(level, String.Format("<many-to-one name=\"{0}\" class=\"{1}\" insert=\"{2}\" update=\"{3}\"  outer-join=\"{4}\">", association.SourceRoleName, association.Target.AssemblyQualifiedName, insert, update, outerJoin));
                    foreach (ForeignKey fk in association.ForeignKeys)
                    {
                        WriteColumn(level + 1, "<column name=\"{0}\"/>", fk.Column);
                    }
                    this.WriteLineWithIndent(level, "</many-to-one>");
                }
            }

            list = Association.GetLinksToSources(clazz);
            foreach (Association association in list)
            {
                if (association.TargetMultiplicity != Multiplicity.NotApplicable && !String.IsNullOrEmpty(association.TargetRoleName))
                {
                    string lazy = NHibernateStrategy.AssociationLazyLoadingProperty.GetValue(association).ToString().ToLower();

                    this.WriteLine("");
                    this.WriteLineWithIndent(level, "<!-- Relation  0..* -->");
                    this.WriteLineWithIndent(level, "<bag ");
                    this.WriteLineWithIndent(level, String.Format("    name=\"{0}\" inverse=\"true\" lazy=\"{1}\" >", association.TargetRoleName, lazy));
                    if (association.ForeignKeys.Count == 1)
                    {
                        WriteColumn(level + 1, "<key column=\"{0}\"/>", association.ForeignKeys[0].Column);
                    }
                    else
                    {
                        this.WriteLineWithIndent(level + 1, "<key>");
                        foreach (ForeignKey fk in association.ForeignKeys)
                        {
                            WriteColumn(level + 2, "<column name=\"{0}\"/>", fk.Column);
                        }
                        this.WriteLineWithIndent(level + 1, "</key>");
                    }
                    this.WriteLineWithIndent(level + 1, String.Format("<one-to-many class=\"{0}\" />", association.Source.AssemblyQualifiedName));
                    this.WriteLineWithIndent(level, "</bag>");
                }
            }
        }

        private void WriteColumn(int level, string format, Property property)
        {
            string colName = property.ColumnName;
            if (string.IsNullOrEmpty(colName))
                colName = property.Name;
            this.WriteLineWithIndent(level, String.Format(format, colName));
        }

        internal void Execute(GenerationContext context, string outputFile)
        {
            writer = new SafeStreamWriter(outputFile, false);
            try
            {
                this.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                this.WriteLine("<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.0\">");
                this.WriteLine("");

                CandleModel model = context.Model;
                if (model == null || !model.SoftwareComponent.IsDataLayerExists)
                    return;
                _modelsLayer = model.SoftwareComponent.DataLayer;

                _inheritanceTree = new ClassInheritanceTree(_modelsLayer);
                foreach (ClassInheritanceNode childNode in _inheritanceTree.Root.childs)
                {
                    GenerateClassMapping(childNode, 1, 1);
                }
                _inheritanceTree = null;

                this.WriteLine("");
                this.WriteLine("</hibernate-mapping>");
            }
            finally
            {
                writer.Close();
            }
        }

    }
}
