using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class CandleModelMerger
    {
        // dependencyProperty Name
        // configurationPart  Name
        // foreignKey ?

        /// <summary>
        /// Merges the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public static void Merge(CandleModel model)
        {
            foreach (ModelElement elem in model.Store.ElementDirectory.AllElements)
            {
                if (elem is ShapeElement || elem is ElementLink)
                    continue;

                DomainClassInfo ci = elem.GetDomainClass();
                Debug.WriteLine(String.Format("Elem '{0}' id={1}", ci.Name, elem.Id));
                foreach (DomainPropertyInfo p in ci.LocalDomainProperties)
                {
                    Debug.WriteLine(String.Format("     property '{0}'={1}", p.Name, p.GetValue(elem)));
                }

                foreach (DomainRoleInfo role in ci.LocalDomainRolesPlayed)
                {
                    foreach (ElementLink link in role.GetElementLinks(elem))
                    {
                        Debug.WriteLine(String.Format("     --> '{0}' id={1}", role.Name, link.Id));

                        foreach (DomainPropertyInfo p in link.GetDomainClass().LocalDomainProperties)
                        {
                            Debug.WriteLine(String.Format("     --> property '{0}'={1}", p.Name, p.GetValue(link)));
                        }
                    }
                }
            }
        }
    }
}