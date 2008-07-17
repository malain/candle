using System;
using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Vérification de la cohèrence des composants référencés
    /// </summary>
    public class DependencyGraphVisitor : IReferenceVisitor
    {
        #region structures internes

        #region Nested type: ModelKey

        /// <summary>
        /// 
        /// </summary>
        private struct ModelKey
        {
            /// <summary>
            /// 
            /// </summary>
            public readonly Guid Id;
            /// <summary>
            /// 
            /// </summary>
            public readonly VersionInfo Version;

            /// <summary>
            /// Initializes a new instance of the <see cref="ModelKey"/> struct.
            /// </summary>
            /// <param name="id">The id.</param>
            /// <param name="vi">The vi.</param>
            public ModelKey(Guid id, VersionInfo vi)
            {
                Id = id;
                Version = vi;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                return String.Concat(Id, Version.ToString()).GetHashCode();
            }
        }

        #endregion

        #region Nested type: RelationShip

        /// <summary>
        /// 
        /// </summary>
        public struct RelationShip
        {
            #region RelationType enum

            /// <summary>
            /// 
            /// </summary>
            public enum RelationType
            {
                /// <summary>
                /// 
                /// </summary>
                Framework,
                /// <summary>
                /// 
                /// </summary>
                Composants,
                /// <summary>
                /// 
                /// </summary>
                Artifacts
            }

            #endregion

            /// <summary>
            /// 
            /// </summary>
            public ReferenceScope Scope;

            /// <summary>
            /// 
            /// </summary>
            public CandleModel Source;
            /// <summary>
            /// 
            /// </summary>
            public CandleModel Target;
            /// <summary>
            /// 
            /// </summary>
            public string TargetAsString;
            /// <summary>
            /// 
            /// </summary>
            public RelationType Type;

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> containing a fully qualified type name.
            /// </returns>
            public override string ToString()
            {
                return Source.Name + " -> " + (Target == null ? TargetAsString : Target.Name);
            }
        }

        #endregion

        #endregion

        private readonly Stack<CandleModel> _context = new Stack<CandleModel>();
        private readonly List<ModelKey> _models = new List<ModelKey>();
        private readonly List<RelationShip> _relations = new List<RelationShip>();


        /// <summary>
        /// Gets the relations.
        /// </summary>
        /// <value>The relations.</value>
        public List<RelationShip> Relations
        {
            get { return _relations; }
        }

        #region IReferenceVisitor Members

        /// <summary>
        /// Accepts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool IReferenceVisitor.Accept(ReferenceItem item)
        {
            if (item.Element is DataLayer)
            {
                if (item.Source is DataLayer)
                {
                    RelationShip relation;
                    relation.Source = ((DataLayer) item.Source).Component.Model;
                    relation.Target = ((DataLayer) item.Element).Component.Model;
                    ;
                    relation.Type = RelationShip.RelationType.Composants;
                    relation.Scope = item.Scope;
                    relation.TargetAsString = String.Empty;
                    if (!RelationExists(relation))
                    {
                        _relations.Add(relation);
                    }
                    // On n'empile pas le modèle sur le contexte car une couche modèle ne peut avoir que des références sur d'autres modèles
                    // et on a pas besoin de contexte pour calculer la relation
                }
            }

            if (item.Element is CandleModel)
            {
                CandleModel model = item.Element as CandleModel;
                if (model == null)
                    return false;

                if (_context.Count > 0)
                {
                    RelationShip relation;
                    relation.Source = _context.Peek();
                    relation.Target = model;
                    relation.Type = RelationShip.RelationType.Composants;
                    relation.Scope = item.Scope;
                    relation.TargetAsString = String.Empty;
                    if (!RelationExists(relation))
                        _relations.Add(relation);
                }
                else
                {
                    //modele initial
                    RelationShip rel;
                    rel.Type = RelationShip.RelationType.Framework;
                    rel.Source = model;
                    rel.Target = null;
                    rel.TargetAsString = model.DotNetFrameworkVersion.ToString();
                    rel.Scope = item.Scope;
                    _relations.Add(rel);
                }

                // Si dèja vu, on arrete
                ModelKey key = new ModelKey(model.Id, model.Version);
                if (_models.Exists(delegate(ModelKey k) { return key.GetHashCode() == k.GetHashCode(); }))
                    return false;

                _models.Add(key); // Liste des modèles dèja traités
                _context.Push(model);
            }

            return true;
        }

        /// <summary>
        /// Exits the element.
        /// </summary>
        /// <param name="item">The item.</param>
        void IReferenceVisitor.ExitElement(ReferenceItem item)
        {
            if (item.Element is CandleModel)
                _context.Pop();
        }

        #endregion

        /// <summary>
        /// Relations the exists.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        private bool RelationExists(RelationShip relation)
        {
            return relation.Source == relation.Target ||
                   _relations.Exists(delegate(RelationShip rel)
                                         {
                                             return rel.Source.Id == relation.Source.Id &&
                                                    (rel.Target != null && rel.Target.Id == relation.Target.Id) ||
                                                    (rel.Target == null && rel.TargetAsString == relation.TargetAsString);
                                         });
        }
    }
}