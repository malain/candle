using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DSLFactory.Candle.SystemModel.Dependencies;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Repository.Providers;
using System.IO;

namespace DSLFactory.Candle.Repository
{
    /// <summary>
    /// Stockage des informations issues des modèles
    /// </summary>
    /// <remarks>
    /// Le cache sera remis à jour à chaque nouvelle publication d'un modèle
    /// </remarks>
    public class RepositoryCache
    {
        private int _minTagWeight;
        private int _maxTagWeight;
        private Dictionary<string, int> _taggings;
        private List<ComponentModelMetadata> _models;
        private object sync = new object();
        private List<DependencyGraphVisitor.RelationShip> _dependencies;
        private Dictionary<ComponentSig, List<string>> _artifactsCache = new Dictionary<ComponentSig, List<string>>();
        private IRepositoryProvider _provider;

        public RepositoryCache()
        {
            _provider = new FileRepositoryProvider(CandleSettings.BaseDirectory, RepositoryManager.GetFolderPath(RepositoryCategory.Models));
            HttpContext.Current.Trace.Write(_provider.Name);
        }

        /// <summary>
        /// Liste des relations entre composants
        /// </summary>
        /// <returns></returns>
        public List<DependencyGraphVisitor.RelationShip> GetAllRelations()
        {
            if (_dependencies == null)
            {
                lock (sync)
                {
                    if (_dependencies == null)
                        _dependencies = ReferencesHelper.GetAllDependencies();
                }
            }
            return _dependencies;
        }

        /// <summary>
        /// Liste des tags avec leurs poids
        /// </summary>
        /// <param name="minWeight"></param>
        /// <param name="maxWeight"></param>
        /// <returns></returns>
        public Dictionary<string,int> GetTaggings(out int minWeight, out int maxWeight)
        {
            if (_taggings == null)
            {
                lock (sync)
                {
                    if (_taggings == null)
                    {
                        _taggings = CalculateTaggings(GetAllMetadata(), out _minTagWeight,  out _maxTagWeight);
                    }
                }
            }

            minWeight = _minTagWeight;
            maxWeight = _maxTagWeight;

            return _taggings;
        }

        /// <summary>
        /// Calcul du 'tagCloud' à partir d'une liste
        /// </summary>
        /// <param name="list"></param>
        /// <param name="minWeight"></param>
        /// <param name="maxWeight"></param>
        /// <returns></returns>
        public static Dictionary<string, int> CalculateTaggings(List<ComponentModelMetadata> list, out int minWeight, out int maxWeight)
        {
            Dictionary<string, int> taggings = new Dictionary<string, int>();
            foreach (ComponentModelMetadata m in list)
            {
                string[] words = m.Path.Split(DomainManager.PathSeparator);
                foreach (string word in words)
                {
                    if (taggings.ContainsKey(word))
                        taggings[word]++;
                    else
                        taggings.Add(word, 1);
                }
            }

            minWeight = maxWeight = 0;
            foreach (int val in taggings.Values)
            {
                if (val > maxWeight) maxWeight = val;
                if (val < minWeight) minWeight = val;
            }
            return taggings;
        }

        /// <summary>
        /// Liste de tous les métadata des modèles du repository (Réactualisé lors de chaque publication)
        /// </summary>
        /// <returns></returns>
        public List<ComponentModelMetadata> GetAllMetadata()
        {
            if (_models == null)
            {
                lock (sync)
                {
                    if (_models == null)
                        _models = _provider.GetAllMetadata();
                }
            }
            return _models;
        }

        /// <summary>
        /// Liste des artifacts d'un modèle
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public List<string> GetArtifacts(ComponentModelMetadata metadata)
        {
            ComponentSig sig = new ComponentSig(metadata);
            List<string> artifacts;
            if (!_artifactsCache.TryGetValue(sig, out artifacts))
            {
                ModelLoader loader = ModelLoader.GetLoader(metadata);
                if (loader != null && loader.Model != null)
                {
                    ReferenceWalker walker = new ReferenceWalker(ReferenceScope.Publish, new ConfigurationMode("*"));
                    ReferenceVisitor visitor = new ReferenceVisitor(ReferenceScope.Publish);
                    walker.Traverse(visitor, loader.Model);
                    artifacts = new List<string>();
                    foreach (string artifactFileName in visitor.References)
                    {
                        artifacts.Add(Path.GetFileName(artifactFileName));
                    }
                }
                _artifactsCache.Add(sig, artifacts);
            }
            return artifacts;
        }

        /// <summary>
        /// Nettoyage du cache
        /// </summary>
        public void Clear()
        {
            lock (sync)
            {
                _taggings = null;
                _models = null;
                _dependencies = null;
                _artifactsCache.Clear();
                ModelLoader.ClearCache();
            }
        }
    }
}