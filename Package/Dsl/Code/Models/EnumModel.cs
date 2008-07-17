using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using DSLFactory.Candle.SystemModel.Utilities;
using System.Collections;

namespace DSLFactory.Candle.SystemModel
{

    /// <summary>
    /// 
    /// </summary>
    partial class Enumeration : ISupportDetailView
    {
        /// <summary>
        /// 
        /// </summary>
        public class VirtualTreeGridEnumCategory : VirtualTreeGridCategory 
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="VirtualTreeGridEnumCategory"/> class.
            /// </summary>
            public VirtualTreeGridEnumCategory() : base(false)
            {
            }

            /// <summary>
            /// Gets the empty text.
            /// </summary>
            /// <param name="kind">The kind.</param>
            /// <returns></returns>
            internal override string GetEmptyText(ModelKind kind)
            {
                return "<add value>";
            }
        }

        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public IList GetChildrenForCategory( VirtualTreeGridCategory category )
        {
            return Values;
        }

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
        /// Type par défaut pour un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Un nom de type</returns>
        public string GetDefaultType( ModelKind modelKind )
        {
            return "System.Int32";
        }

        /// <summary>
        /// Création de l'instance d'un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Instance de modèle</returns>
        public ITypeMember CreateModel( ModelKind modelKind )
        {
            return new EnumValue( this.Store );
        }

        /// <summary>
        /// Initialise les propriétés de la fenêtre détail
        /// </summary>
        /// <param name="title">Titre de la fenetre</param>
        /// <param name="categories">Liste des catégories de membres à afficher ou null si une seule par défaut</param>
        /// <param name="memberSeparators">Liste des caractères séparateurs des membres</param>
        /// <param name="childSeparators">Liste des caractères séparateurs des enfants</param>
        public void GetEditProperties( out string title, out List<VirtualTreeGridCategory> categories, out string memberSeparators, out string childSeparators )
        {
            categories = new List<VirtualTreeGridCategory>();
            categories.Add(new VirtualTreeGridEnumCategory());

            childSeparators = memberSeparators = ";";
            title = this.FullName;
        }

    }
}
