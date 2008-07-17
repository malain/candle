using System;
using System.Collections.Generic;
using System.Text;
using DSLFactory.Candle.SystemModel.Utilities;
using System.Collections;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class Argument : IArgument
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return this.Operation; }
        }

        /// <summary>
        /// Copies the argument.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="copy">The copy.</param>
        /// <returns></returns>
        internal static Argument CopyArgument( Store store, Argument copy )
        {
            Argument arg = new Argument( store );
            arg.Name = copy.Name;
            arg.Comment = copy.Comment;
            arg.IsCollection = copy.IsCollection;
            arg.Direction = copy.Direction;
            arg.Type = copy.Type;
            return arg;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals( object obj )
        {
            if( obj is Argument && obj != null )
            {
                Argument other = (Argument)obj;
                return ( other.Name == this.Name && other.Type == this.Type );
            }

            return base.Equals( obj );
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return String.Concat( this.Name, this.Type ).GetHashCode();
        }
    }
}
