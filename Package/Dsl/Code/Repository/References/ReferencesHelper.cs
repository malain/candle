using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReferencesHelper
    {
        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="solutionFolder">The solution folder.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static ReferencesCollection GetReferences(ConfigurationMode mode, ReferenceScope scope,
                                                         string solutionFolder, ModelElement element)
        {
            ReferenceVisitor visitor = new ReferenceVisitor(scope, solutionFolder);
            ReferenceWalker walker = new ReferenceWalker(scope, mode);
            walker.Traverse(visitor, element);
            return visitor.References;
        }

        /// <summary>
        /// Checks the references.
        /// </summary>
        /// <param name="loadModelIfNotPresents">if set to <c>true</c> [load model if not presents].</param>
        /// <param name="context">The context.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="element">The element.</param>
        internal static void CheckReferences(bool loadModelIfNotPresents, ValidationContext context,
                                             ConfigurationMode mode, ReferenceScope scope, ModelElement element)
        {
            CheckReferenceVisitor visitor = new CheckReferenceVisitor(loadModelIfNotPresents, scope, context);
            ReferenceWalker walker = new ReferenceWalker(scope, mode);
            walker.Traverse(visitor, element);
        }

        /// <summary>
        /// Gets all dependencies.
        /// </summary>
        /// <returns></returns>
        public static List<DependencyGraphVisitor.RelationShip> GetAllDependencies()
        {
            List<CandleModel> models = new List<CandleModel>();

            foreach (ComponentModelMetadata md in RepositoryManager.Instance.ModelsMetadata.Metadatas)
            {
                ModelLoader loader = ModelLoader.GetLoader(md);
                if (loader != null && loader.Model != null)
                    models.Add(loader.Model);
            }
            return GetDependencies(models.ToArray());
        }

        /// <summary>
        /// Liste des dépendances
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns></returns>
        public static List<DependencyGraphVisitor.RelationShip> GetDependencies(params CandleModel[] models)
        {
            DependencyGraphVisitor visitor = new DependencyGraphVisitor();
            ReferenceWalker walker = new ReferenceWalker(ReferenceScope.All, new ConfigurationMode("*"));
            foreach (CandleModel model in models)
            {
                // Ce n'est pas la peine de traiter les modeles qui ont une relation
                // dont ils sont la source car cela veut dire qu'on a dèjà traiter leurs
                // dépendences
                if (
                    !visitor.Relations.Exists(
                         delegate(DependencyGraphVisitor.RelationShip r) { return r.Source.Id == model.Id && r.Source.Version == model.Version; }))
                {
                    walker.Traverse(visitor, model);
                }
            }
            return visitor.Relations;
        }
    }
}