using System;
using System.Collections;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.Utilities;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class TypeWithOperations : IHasChildren, IHasNamespace
    {
        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public override string FullName
        {
            get { return String.Concat(NamespaceDeclaration, '.', Name); }
        }

        #region IHasChildren Members

        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public virtual IList GetChildrenForCategory(VirtualTreeGridCategory category)
        {
            return Operations;
        }

        #endregion

        #region IHasNamespace Members

        /// <summary>
        /// Recherche itérative du parent
        /// </summary>
        /// <value></value>
        public string NamespaceDeclaration
        {
            get
            {
                ICustomizableElement parent = Owner;
                IHasNamespace typeContainer = null;
                while (parent != null)
                {
                    if (parent is IHasNamespace)
                    {
                        typeContainer = parent as IHasNamespace;
                        break;
                    }
                    parent = parent.Owner;
                }

                return typeContainer.NamespaceDeclaration;
            }
        }

        #endregion

        /// <summary>
        /// Allows the model element to configure itself immediately after the Merge process has related it to the target element.
        /// </summary>
        /// <param name="elementGroup">The group of source elements that have been added back into the target store.</param>
        protected override void MergeConfigure(ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);
            RootName = Name;
        }

        /// <summary>
        /// Copie des opérations
        /// </summary>
        /// <param name="source">Opérations qui seront copiées</param>
        /// <param name="target">Cible de la copie</param>
        public static void CopyOperations(TypeWithOperations source, TypeWithOperations target)
        {
            // Copie des opérations
            foreach (Operation op in source.Operations)
            {
                target.Operations.Add(Operation.CopyOperation(target.Store, op));
            }
        }

        /// <summary>
        /// Recherche une opération identique
        /// </summary>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        public Operation FindOperation(Operation op)
        {
            foreach (Operation operation in Operations)
            {
                if (op.Equals(operation))
                    return operation;
            }
            return null;
        }

        /// <summary>
        /// Declares the operation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="isCollection">if set to <c>true</c> [is collection].</param>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        public Operation DeclareOperation(string name, string type, bool isCollection, string comment)
        {
            Operation op = new Operation(Store);
            op.Name = name;
            op.Type = type;
            op.IsCollection = isCollection;
            op.Comment = comment;
            Operations.Add(op);
            return op;
        }

        /// <summary>
        /// Teste si une opération existe
        /// </summary>
        /// <param name="name">Nom de l'opération</param>
        /// <param name="typeParameters">Liste des parametres</param>
        /// <returns></returns>
        /// <example>clazz.OperationExists("toto",new TypeDefinition(clazz.Name, true))</example>
        public bool OperationExists(string name, params TypeDefinition[] typeParameters)
        {
            foreach (Operation op in Operations)
            {
                if (op.Name == name)
                {
                    if (typeParameters != null)
                    {
                        for (int i = 0; i < typeParameters.Length; i++)
                        {
                            TypeDefinition typeParam = typeParameters[i];
                            Argument arg = op.Arguments[i];
                            if (typeParam.Name != arg.Type || typeParam.IsCollection != arg.IsCollection)
                                return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the default type.
        /// </summary>
        /// <param name="modelKind">Kind of the model.</param>
        /// <returns></returns>
        public virtual string GetDefaultType(ModelKind modelKind)
        {
            if (modelKind == ModelKind.Member)
                return "void";
            return "int";
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="modelKind">Kind of the model.</param>
        /// <returns></returns>
        public virtual ITypeMember CreateModel(ModelKind modelKind)
        {
            if (modelKind == ModelKind.Member)
                return new Operation(Store);
            return new Argument(Store);
        }

        /// <summary>
        /// Gets the edit properties.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="memberSeparators">The member separators.</param>
        /// <param name="childSeparators">The child separators.</param>
        public virtual void GetEditProperties(out string title, out List<VirtualTreeGridCategory> categories,
                                              out string memberSeparators, out string childSeparators)
        {
            // Par défaut pas de catégories
            categories = new List<VirtualTreeGridCategory>();
            categories.Add(new VirtualTreeGridCategory(false));

            title = "Methods for " + FullName;
            memberSeparators = "(";
            childSeparators = ",)";
        }
    }
}