using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Utilities;
using DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class Entity : ISupportDetailView
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public new string Type
        {
            get { return FullName; }
            set { }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is collection.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is collection; otherwise, <c>false</c>.
        /// </value>
        public new bool IsCollection
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Gets the primary keys.
        /// </summary>
        /// <value>The primary keys.</value>
        public IList<Property> PrimaryKeys
        {
            get
            {
                List<Property> properties = new List<Property>();
                foreach (Property property in Properties)
                {
                    if (property.IsPrimaryKey)
                        properties.Add(property);
                }
                return properties;
            }
        }

        #region ISupportDetailView Members

        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public IList GetChildrenForCategory(VirtualTreeGridCategory category)
        {
            return Properties;
        }

        /// <summary>
        /// Type par défaut pour un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Un nom de type</returns>
        public string GetDefaultType(ModelKind modelKind)
        {
            return "int";
        }

        /// <summary>
        /// Création de l'instance d'un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Instance de modèle</returns>
        public ITypeMember CreateModel(ModelKind modelKind)
        {
            return new Property(Store);
        }

        /// <summary>
        /// Initialise les propriétés de la fenêtre détail
        /// </summary>
        /// <param name="title">Titre de la fenetre</param>
        /// <param name="categories">Liste des catégories de membres à afficher ou null si une seule par défaut</param>
        /// <param name="memberSeparators">Liste des caractères séparateurs des membres</param>
        /// <param name="childSeparators">Liste des caractères séparateurs des enfants</param>
        public void GetEditProperties(out string title, out List<VirtualTreeGridCategory> categories,
                                      out string memberSeparators, out string childSeparators)
        {
            categories = new List<VirtualTreeGridCategory>();
            categories.Add(new VirtualTreeGridEntityCategory());
            childSeparators = ";";
            memberSeparators = ");";
            title = FullName;
        }

        #endregion

        /// <summary>
        /// Insertion d'une entité dans le modèle par drag and drop. On en profite pour demander le root name à lui affecter
        /// </summary>
        /// <param name="elementGroup">The group of source elements that have been added back into the target store.</param>
        protected override void MergeConfigure(ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);
            if (Store.TransactionManager.InTransaction)
            {
                TransactionContext ctx = Store.TransactionManager.CurrentTransaction.Context;

                if (!ctx.ContextInfo.ContainsKey(DatabaseImporter.ImportedTableInfo) &&
                    !ctx.ContextInfo.ContainsKey(DatabaseImporter.ImportedTableInfo))
                {
                    PromptBox prompt = new PromptBox("Entity root name :");
                    if (prompt.ShowDialog() == DialogResult.OK)
                    {
                        RootName = prompt.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the association to.
        /// </summary>
        /// <param name="targetModel">The target model.</param>
        /// <returns></returns>
        public Association AddAssociationTo(Entity targetModel)
        {
            Targets.Add(targetModel);
            IList<Association> assocs = Association.GetLinks(this, targetModel);
            return assocs[assocs.Count - 1];
        }

        #region Nested type: VirtualTreeGridEntityCategory

        /// <summary>
        /// 
        /// </summary>
        public class VirtualTreeGridEntityCategory : VirtualTreeGridCategory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="VirtualTreeGridEntityCategory"/> class.
            /// </summary>
            public VirtualTreeGridEntityCategory()
                : base(false)
            {
            }

            /// <summary>
            /// Gets the empty text.
            /// </summary>
            /// <param name="kind">The kind.</param>
            /// <returns></returns>
            internal override string GetEmptyText(ModelKind kind)
            {
                return "<add property>";
            }
        }

        #endregion
    }
}