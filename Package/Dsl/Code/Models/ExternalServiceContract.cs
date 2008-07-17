using System;
using System.Collections;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Utilities;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class ExternalServiceContract : ISupportDetailView, IHasChildren
    {
        private static readonly IList s_emptyList = new ArrayList(0);

        /// <summary>
        /// Recherche du service 
        /// </summary>
        /// <returns></returns>
        public TypeWithOperations ReferencedServiceContract
        {
            get
            {
                CandleModel model = this.Parent.ReferencedModel;
                if (model != null)
                    return model.SoftwareComponent.PublicContracts.Find(delegate(TypeWithOperations port) { return this.ComponentPortMoniker == port.Id; });
                return null;
            }
        }

        #region ISupportDetailView Members

        /// <summary>
        /// Type par d�faut pour un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Un nom de type</returns>
        public string GetDefaultType(ModelKind modelKind)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Cr�ation de l'instance d'un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Instance de mod�le</returns>
        public ITypeMember CreateModel(ModelKind modelKind)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Initialise les propri�t�s de la fen�tre d�tail
        /// </summary>
        /// <param name="title">Titre de la fenetre</param>
        /// <param name="categories">Liste des cat�gories de membres � afficher ou null si une seule par d�faut</param>
        /// <param name="memberSeparators">Liste des caract�res s�parateurs des membres</param>
        /// <param name="childSeparators">Liste des caract�res s�parateurs des enfants</param>
        public void GetEditProperties(out string title, out List<VirtualTreeGridCategory> categories, out string memberSeparators, out string childSeparators)
        {
            categories = new List<VirtualTreeGridCategory>();
            categories.Add(new VirtualTreeGridCategory(true));
            title = "Operations for " + this.FullName;
            memberSeparators = "(";
            childSeparators = ",)";
        }

        #endregion

        #region IHasChildren Members

        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public System.Collections.IList GetChildrenForCategory(VirtualTreeGridCategory category)
        {
            return ReferencedServiceContract != null ? ReferencedServiceContract.GetChildrenForCategory(category) : s_emptyList;
        }

        #endregion

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public override string FullName
        {
            get {return ReferencedServiceContract != null ? ReferencedServiceContract.FullName : "<<unknow>>"; }
        }
    }
}