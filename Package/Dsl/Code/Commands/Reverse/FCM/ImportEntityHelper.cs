using EnvDTE;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation des opérations d'un port à partir d'un fichier source
    /// </summary>
    public class ImportEntityHelper : IImportEntityHelper
    {
        /// <summary>
        /// Imports the properties.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>true if the import is ok</returns>
        public bool ImportProperties( Package package, Entity entity, string fileName )
        {
            FileCodeModel fcm = ServiceLocator.Instance.ShellHelper.GetFileCodeModel(fileName);
            if (fcm == null)
                return false;

            foreach (CodeElement cn in fcm.CodeElements)
            {
                if (cn is CodeNamespace)
                {
                    foreach (CodeElement ci in ((CodeNamespace)cn).Members)
                    {
                        if (ci is CodeClass)
                        {
                            CodeClass cc = ci as CodeClass;
                            {
                                if( entity == null )
                                {
                                    entity = new Entity(package.Store);
                                    entity.Name = ci.Name;
                                    entity.RootName = ci.Name;
                                    entity.Comment = ImportInterfaceHelper.NormalizeComment(cc.DocComment);
                                    package.Types.Add(entity);
                                }

                                RetrieveProperties(entity, cc.Members);
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Retrieves the properties.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="members">The members.</param>
        private static void RetrieveProperties(Entity entity, CodeElements members)
        {
            foreach (CodeElement codeElement in members)
            {
                CodeProperty prop = codeElement as CodeProperty;
                if( prop == null )
                    continue;

                if (prop.Access != vsCMAccess.vsCMAccessPublic)
                    continue;

                // Operation
                Property p = FindProperty(entity, prop);
                if (p == null)
                {
                    p = new Property(entity.Store);
                    p.Name = prop.Name;
                    p.ColumnName = prop.Name;
                    p.Comment = ImportInterfaceHelper.NormalizeComment(prop.DocComment);
                    p.Type = prop.Type.AsString;
                    p.IsCollection = prop.Type.TypeKind == vsCMTypeRef.vsCMTypeRefArray;
                    entity.Properties.Add(p);
                }
            }
        }

        /// <summary>
        /// Finds the property.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private static Property FindProperty( Entity entity, CodeProperty property )
        {
            foreach( Property p in entity.Properties )
            {
                if( p.Name == property.Name && p.Type == property.Type.AsString )
                {
                    return p;
                }
            }
            return null;
        }
    }
}
