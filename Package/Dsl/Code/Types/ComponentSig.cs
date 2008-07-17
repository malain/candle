using System;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Permet de manipuler la signature d'un modèle (Id+version)
    /// </summary>
    public struct ComponentSig
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly ComponentSig EmptySig = new ComponentSig(Guid.Empty, null);
        /// <summary>
        /// 
        /// </summary>
        public Guid Id;
        /// <summary>
        /// 
        /// </summary>
        public VersionInfo Version;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentSig"/> struct.
        /// </summary>
        /// <param name="model">The model.</param>
        public ComponentSig(CandleModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            Id = model.Id;
            Version = model.Version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentSig"/> struct.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public ComponentSig(ComponentModelMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");
            Id = metadata.Id;
            Version = metadata.Version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentSig"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        public ComponentSig(Guid id, VersionInfo version)
        {
            Id = id;
            Version = version;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return Id == Guid.Empty && Version == null; }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return String.Concat(Id.ToString(), Version.ToString()).GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if obj and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is CandleModel)
                return ((CandleModel) obj).Id == Id && ((CandleModel) obj).Version.Equals(Version);
            if (obj is ComponentSig)
                return ((ComponentSig) obj).Id == Id && ((ComponentSig) obj).Version.Equals(Version);
            return base.Equals(obj);
        }
    }
}