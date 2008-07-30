using System;
using System.Collections.Generic;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Noeud permettant de reconstituer la hiérarchie des classes
    /// </summary>
    class ClassInheritanceNode
    {
        // Indique si le modèle fait partie du modèle courant ou pas
        public bool IsExternal;
        // Nom de la classe 
        public string fullName;
        // Modéle de la classe
        public Entity clazz;
        // Sous classes
        public LinkedList<ClassInheritanceNode> childs;

        public static ClassInheritanceNode Empty = new ClassInheritanceNode(true);

        public bool IsEmpty { get { return childs == null; } }

        private ClassInheritanceNode(bool isEmpty)
        {
            IsExternal = false;
            childs = null;
            clazz = null;
            fullName = null;
        }

        public ClassInheritanceNode(Entity model, bool isExternal)
        {
            this.IsExternal = isExternal;
            childs = new LinkedList<ClassInheritanceNode>();
            clazz = model;
            fullName = String.Empty;
            if (model != null)
                fullName = model.FullName;
        }
    }
}
