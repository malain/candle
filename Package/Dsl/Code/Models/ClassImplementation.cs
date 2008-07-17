using System;
using System.Collections.Generic;
using System.Text;
using DSLFactory.Candle.SystemModel.Utilities;
using System.Collections;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class ClassImplementation :  IShowCodeProperties, ISupportDetailView
    {
        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public override IList GetChildrenForCategory(VirtualTreeGridCategory category)
        {
            if (category.Id == 0 && this.Store != null && this.Contract != null)
                return this.Contract.GetChildrenForCategory(category);
            
            return base.GetChildrenForCategory(category);
        }

        /// <summary>
        /// Gets the edit properties.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="memberSeparators">The member separators.</param>
        /// <param name="childSeparators">The child separators.</param>
        public override void GetEditProperties(out string title, out List<VirtualTreeGridCategory> categories, out string memberSeparators, out string childSeparators)
        {
            base.GetEditProperties(out title, out categories, out memberSeparators, out childSeparators);
            // Surcharge des catégories
            categories.Clear();
            categories.Add( new VirtualTreeGridCategory("Contracts", true, 0, VirtualTreeGridResource.VSObject_Interface));
            categories.Add( new VirtualTreeGridCategory("Operations", false, 1, VirtualTreeGridResource.VSObject_Method));
        }

        /// <summary>
        /// Gets the name of the assembly qualified.
        /// </summary>
        /// <value>The name of the assembly qualified.</value>
        public string AssemblyQualifiedName
        {
            get { return String.Format( "{0}, {1}", this.FullName, this.Layer.AssemblyName ); }
        }

        /// <summary>
        /// Gets the name of the layer.
        /// </summary>
        /// <value>The name of the layer.</value>
        public string LayerName
        {
            get {return this.Layer.Name; }
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="initialName">The initial name.</param>
        /// <returns></returns>
        public string GetMemberName(string initialName)
        {
            return String.Format("{0}", initialName);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}
