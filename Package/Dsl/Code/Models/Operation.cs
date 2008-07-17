using System;
using System.Collections;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.Utilities;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class Operation : IHasChildren
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return Parent; }
        }

        /// <summary>
        /// Gets the software component.
        /// </summary>
        /// <value>The software component.</value>
        public SoftwareComponent SoftwareComponent
        {
            get { return CandleModel.GetInstance(Store).SoftwareComponent; }
        }

        #region IHasChildren Members

        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public IList GetChildrenForCategory(VirtualTreeGridCategory category)
        {
            return Arguments;
        }

        #endregion

        /// <summary>
        /// Copies the operation.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="copy">The copy.</param>
        /// <returns></returns>
        public static Operation CopyOperation(Store store, Operation copy)
        {
            Operation op = new Operation(store);
            op.Name = copy.Name;
            op.Comment = copy.Comment;
            op.Type = copy.Type;
            op.IsCollection = copy.IsCollection;
            foreach (Argument arg in copy.Arguments)
            {
                op.Arguments.Add(Argument.CopyArgument(store, arg));
            }
            return op;
        }

        /// <summary>
        /// Declares the argument.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public Argument DeclareArgument(ITypeMember source)
        {
            Argument arg = new Argument(Store);
            arg.Name = StrategyManager.GetInstance(Store).NamingStrategy.ToCamelCasing(source.Name);
            arg.Type = source.Type;
            arg.IsCollection = source.IsCollection;
            arg.Comment = source.Comment;
            Arguments.Add(arg);
            return arg;
        }

        /// <summary>
        /// Declares the argument.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="isCollection">if set to <c>true</c> [is collection].</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        public Argument DeclareArgument(string name, string type, bool isCollection, string comment)
        {
            Argument arg = new Argument(Store);
            arg.Name = name;
            arg.Type = type;
            arg.IsCollection = isCollection;
            arg.Comment = comment;
            return arg;
        }

        /// <summary>
        /// Declares the argument.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Argument DeclareArgument(Entity entity)
        {
            Argument arg = new Argument(Store);
            arg.Name = StrategyManager.GetInstance(Store).NamingStrategy.ToCamelCasing(entity.Name);
            arg.Type = entity.Type;
            arg.IsCollection = false;
            arg.Comment = entity.Comment;
            Arguments.Add(arg);

            return arg;
        }

        /// <summary>
        /// Definition parameters list (paramType1 paramName1, paramType2 paramName2...)
        /// </summary>
        /// <returns></returns>
        public string CreateParametersDefinition()
        {
            return StrategyManager.GetInstance(Store).TargetLanguage.CreateParametersDefinition(this);
        }

        /// <summary>
        /// Calling paremeters list (paramName1, paramName2...)
        /// </summary>
        /// <returns></returns>
        public string CreateParameterList()
        {
            return StrategyManager.GetInstance(Store).TargetLanguage.CreateParameterList(this);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Compare deux opérations (Méme nom, mémes parametres, même type)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Operation && obj != null && Store != null)
            {
                Operation op = (Operation) obj;
                if (op.Store != null)
                {
                    if (op.Name == Name && op.Type == Type &&
                        (op.Arguments != null && Arguments != null && op.Arguments.Count == Arguments.Count))
                    {
                        for (int i = 0; i < op.Arguments.Count; i++)
                        {
                            if (!Arguments[i].Equals(op.Arguments[i]))
                                return false;
                        }
                        return true;
                    }
                }
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return
                String.Format("{0} {1}( {2} )", CodeGenerationUtils.ResolveTypeName(SoftwareComponent, Type), Name,
                              CodeGenerationUtils.GetParametersDefinition(this));
        }
    }
}