using System;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Permet de traverser la hiérarchie et d'appeler un code à chaque itération
    /// </summary>
    /// <remarks>Issue de l'exemple du VSSDK : SolutionHierarchyTraversal</remarks>
    public class VSHierarchyWalker
    {
        private readonly IVSHierarchyVisitor _visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSHierarchyWalker"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public VSHierarchyWalker(IVSHierarchyVisitor visitor)
        {
            _visitor = visitor;
        }

        /// <summary>
        /// Traverses the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public void Traverse(IServiceProvider serviceProvider)
        {
            //Get the solution service so we can traverse each project hierarchy contained within.
            IVsSolution solution = (IVsSolution) serviceProvider.GetService(typeof (SVsSolution));
            if (null != solution)
            {
                IVsHierarchy solutionHierarchy = solution as IVsHierarchy;
                if (null != solutionHierarchy)
                {
                    EnumHierarchyItems(solutionHierarchy, VSConstants.VSITEMID_ROOT, 0, true, false);
                }
            }
        }

        /// <summary>
        /// Enumerates over the hierarchy items for the given hierarchy traversing into nested hierarchies.
        /// </summary>
        /// <param name="hierarchy">hierarchy to enmerate over.</param>
        /// <param name="itemid">item id of the hierarchy</param>
        /// <param name="recursionLevel">Depth of recursion. e.g. if recursion started with the Solution
        /// node, then : Level 0 -- Solution node, Level 1 -- children of Solution, etc.</param>
        /// <param name="hierIsSolution">true if hierarchy is Solution Node. This is needed to special
        /// case the children of the solution to work around a bug with VSHPROPID_FirstChild and
        /// VSHPROPID_NextSibling implementation of the Solution.</param>
        /// <param name="visibleNodesOnly">true if only nodes visible in the Solution Explorer should
        /// be traversed. false if all project items should be traversed.</param>
        private void EnumHierarchyItems(IVsHierarchy hierarchy, uint itemid, int recursionLevel, bool hierIsSolution,
                                        bool visibleNodesOnly)
        {
            int hr;
            IntPtr nestedHierarchyObj;
            uint nestedItemId;
            Guid hierGuid = typeof (IVsHierarchy).GUID;

            // Check first if this node has a nested hierarchy. If so, then there really are two 
            // identities for this node: 1. hierarchy/itemid 2. nestedHierarchy/nestedItemId.
            // We will recurse and call EnumHierarchyItems which will display this node using
            // the inner nestedHierarchy/nestedItemId identity.
            hr = hierarchy.GetNestedHierarchy(itemid, ref hierGuid, out nestedHierarchyObj, out nestedItemId);
            if (VSConstants.S_OK == hr && IntPtr.Zero != nestedHierarchyObj)
            {
                IVsHierarchy nestedHierarchy = Marshal.GetObjectForIUnknown(nestedHierarchyObj) as IVsHierarchy;
                Marshal.Release(nestedHierarchyObj);
                // we are responsible to release the refcount on the out IntPtr parameter
                if (nestedHierarchy != null)
                {
                    // Display name and type of the node in the Output Window
                    EnumHierarchyItems(nestedHierarchy, nestedItemId, recursionLevel, false, visibleNodesOnly);
                }
            }
            else
            {
                object pVar;

                // Display name and type of the node in the Output Window
                AcceptProjectItem(hierarchy, itemid);

                recursionLevel++;

                //Get the first child node of the current hierarchy being walked
                // NOTE: to work around a bug with the Solution implementation of VSHPROPID_FirstChild,
                // we keep track of the recursion level. If we are asking for the first child under
                // the Solution, we use VSHPROPID_FirstVisibleChild instead of _FirstChild. 
                // In VS 2005 and earlier, the Solution improperly enumerates all nested projects
                // in the Solution (at any depth) as if they are immediate children of the Solution.
                // Its implementation _FirstVisibleChild is correct however, and given that there is
                // not a feature to hide a SolutionFolder or a Project, thus _FirstVisibleChild is 
                // expected to return the identical results as _FirstChild.
                hr = hierarchy.GetProperty(itemid,
                                           ((visibleNodesOnly || (hierIsSolution && recursionLevel == 1)
                                                 ?
                                                     (int) __VSHPROPID.VSHPROPID_FirstVisibleChild
                                                 : (int) __VSHPROPID.VSHPROPID_FirstChild)),
                                           out pVar);
//                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure( hr );
                if (VSConstants.S_OK == hr)
                {
                    //We are using Depth first search so at each level we recurse to check if the node has any children
                    // and then look for siblings.
                    uint childId = GetItemId(pVar);
                    while (childId != VSConstants.VSITEMID_NIL)
                    {
                        EnumHierarchyItems(hierarchy, childId, recursionLevel, false, visibleNodesOnly);
                        // NOTE: to work around a bug with the Solution implementation of VSHPROPID_NextSibling,
                        // we keep track of the recursion level. If we are asking for the next sibling under
                        // the Solution, we use VSHPROPID_NextVisibleSibling instead of _NextSibling. 
                        // In VS 2005 and earlier, the Solution improperly enumerates all nested projects
                        // in the Solution (at any depth) as if they are immediate children of the Solution.
                        // Its implementation   _NextVisibleSibling is correct however, and given that there is
                        // not a feature to hide a SolutionFolder or a Project, thus _NextVisibleSibling is 
                        // expected to return the identical results as _NextSibling.
                        hr = hierarchy.GetProperty(childId,
                                                   ((visibleNodesOnly || (hierIsSolution && recursionLevel == 1))
                                                        ?
                                                            (int) __VSHPROPID.VSHPROPID_NextVisibleSibling
                                                        : (int) __VSHPROPID.VSHPROPID_NextSibling),
                                                   out pVar);
                        if (VSConstants.S_OK == hr)
                        {
                            childId = GetItemId(pVar);
                        }
                        else
                        {
                            ErrorHandler.ThrowOnFailure(hr);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function diplays the name of the Hierarchy node. This function is passed to the
        /// Hierarchy enumeration routines to process the current node.
        /// </summary>
        /// <param name="hierarchy">Hierarchy of the current node</param>
        /// <param name="itemid">Itemid of the current node</param>
        private void AcceptProjectItem(IVsHierarchy hierarchy, uint itemid)
        {
            object pVar;
            hierarchy.GetProperty(itemid, (int) __VSHPROPID.VSHPROPID_ExtObject, out pVar);
            ProjectItem pi = pVar as ProjectItem;
            if (pi != null)
                _visitor.Accept(pi);
        }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        /// <param name="pvar">VARIANT holding an itemid.</param>
        /// <returns>Item Id of the concerned node</returns>
        private static uint GetItemId(object pvar)
        {
            if (pvar == null)
                return VSConstants.VSITEMID_NIL;
            if (pvar is int)
                return (uint) (int) pvar;
            if (pvar is uint)
                return (uint) pvar;
            if (pvar is short)
                return (uint) (short) pvar;
            if (pvar is ushort)
                return (ushort) pvar;
            if (pvar is long)
                return (uint) (long) pvar;
            return VSConstants.VSITEMID_NIL;
        }
    }
}