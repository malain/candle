using System;
using System.Collections.Generic;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    class ClassInheritanceTree
    {
        private ClassInheritanceNode _classInheritanceRootNode;
        private DataLayer _modelsLayer;

        public ClassInheritanceTree(DataLayer modelsLayer)
        {
            _modelsLayer = modelsLayer;
        }

        /// <summary>
        /// Recherche d'un noeud dans la hiérarchie des classes
        /// </summary>
        /// <param name="root">Noeud de départ</param>
        /// <param name="fullName">Nom de la classe</param>
        /// <returns></returns>
        public DataType FindEntityByName(string fullName)
        {
            return FindEntityByName(_classInheritanceRootNode, fullName);
        }
        
        /// <summary>
        /// Recherche d'un noeud dans la hiérarchie des classes
        /// </summary>
        /// <param name="root">Noeud de départ</param>
        /// <param name="fullName">Nom de la classe</param>
        /// <returns></returns>
        private DataType FindEntityByName(ClassInheritanceNode root, string fullName)
        {
            if (_classInheritanceRootNode == null || root.IsEmpty || String.IsNullOrEmpty(fullName))
                return null;

            foreach (ClassInheritanceNode node in root.childs)
            {
                if (node.clazz.FullName == fullName)
                    return node.clazz;

                DataType node2 = FindEntityByName(node, fullName);
                if (node2 != null)
                    return node2;
            }
            return null;
        }

        /// <summary>
        /// Recherche d'un noeud dans la hiérarchie des classes
        /// </summary>
        /// <param name="root">Noeud de départ</param>
        /// <param name="fullName">Nom de la classe</param>
        /// <returns></returns>
        private ClassInheritanceNode FindNode(ClassInheritanceNode root, Entity entity)
        {
            if (root.IsEmpty || entity == null)
                return ClassInheritanceNode.Empty;

            foreach (ClassInheritanceNode node in root.childs)
            {
                if (node.clazz.Id == entity.Id)
                    return node;

                ClassInheritanceNode node2 = FindNode(node, entity);
                if (!node2.IsEmpty)
                    return node2;
            }
            return ClassInheritanceNode.Empty;
        }

        public ClassInheritanceNode  Root
        {
            get
            {
                if (_classInheritanceRootNode == null)
                    CreateInheritanceTree();
                return _classInheritanceRootNode;
            }
        }

        /// <summary>
        /// Création de l'arbre d'héritage
        /// </summary>
        /// <returns></returns>
        private void CreateInheritanceTree()
        {
            // C'est la racine
            _classInheritanceRootNode = new ClassInheritanceNode(null, false);

            // On commence par charger les modèles référencés
            foreach (ExternalComponent sys in _modelsLayer.Component.Model.ExternalComponents)
            {
                ModelLoader proxy = ModelLoader.GetLoader(sys);
                if (proxy != null && proxy.DataLayer != null)
                    CreateInheritanceTreeFromModel(proxy.DataLayer, _classInheritanceRootNode, true);
            }

            // Puis le modèle en cours
            CreateInheritanceTreeFromModel(_modelsLayer, _classInheritanceRootNode, false);
        }

        private void CreateInheritanceTreeFromModel(DataLayer modelsLayer, ClassInheritanceNode root, bool isExternal)
        {
            if (modelsLayer == null)
                return;

            foreach (DataType abstractType in modelsLayer.GetAllTypes(null, false))
            {
                Entity model = abstractType as Entity;
                if (model == null)
                    continue;

                ClassInheritanceNode node = new ClassInheritanceNode(model, isExternal);

                // Recherche du parent
                ClassInheritanceNode parentNode = FindNode(root, model.SuperClass);
                if (parentNode.IsEmpty)
                {
                    // Si le parent n'est pas trouvé, on met temporairement cette classe 
                    // sur la racine
                    root.childs.AddLast(node);
                }
                else
                {
                    parentNode.childs.AddLast(node);
                }

                // On regarde si cette classse n'est pas le parent de la classe
                // trouvée précedemment (qui ont été mises dans le root)
                LinkedListNode<ClassInheritanceNode> listNode = root.childs.First;
                while (listNode != null)
                {
                    if (listNode.Value.clazz.SuperClass != null && listNode.Value.clazz.SuperClass.Id == model.Id)
                    {
                        // Oui alors ils deviennent enfants de ce noeud
                        node.childs.AddLast(listNode.Value);
                        root.childs.Remove(listNode);
                        // On repart du début
                        listNode = root.childs.First;
                        continue;
                    }

                    listNode = listNode.Next;
                }
            }
        }
    }
}
