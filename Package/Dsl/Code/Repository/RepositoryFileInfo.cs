using System;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryFileInfo : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public string FileName;
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastWriteTimeUtc;

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo( object obj )
        {
            RepositoryFileInfo other = obj as RepositoryFileInfo;
            return String.Compare( FileName, other.FileName, StringComparison.CurrentCultureIgnoreCase );
        }
    }

}
