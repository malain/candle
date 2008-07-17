using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Regroupement de composants organisé sous forme arborescente
    /// </summary>
    [Serializable]
    public class DomainItem
    {
        private DomainItemCollection childs;

        /// <summary>
        /// Nom 
        /// </summary>
        private string _name;

        private DomainItem _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainItem"/> class.
        /// </summary>
        public DomainItem()
        {
            childs = new DomainItemCollection(this);
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [XmlIgnore]
        public DomainItem Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the childs.
        /// </summary>
        /// <value>The childs.</value>
        [XmlElement("domain")]
        public DomainItemCollection Childs
        {
            get { return childs; }
            set { childs = value; }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                DomainItem current = this;
                while (current != null)
                {
                    sb.Insert(0, current.Name);
                    current = current.Parent;
                    if (current != null)
                        sb.Insert(0, DomainManager.PathSeparator);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DomainItem FindItem(string path)
        {
            string[] parts = path.Split(DomainManager.PathSeparator);
            DomainItem current = this;
            foreach (string part in parts)
            {
                current = current.FindByName(part);
                if (current == null)
                    break;
            }
            return current;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            Parent.Childs.Remove(this);
        }

        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DomainItem CreateItem(string path)
        {
            string[] parts = path.Split(DomainManager.PathSeparator);
            DomainItem current = this;
            DomainItem parent = this;
            foreach (string part in parts)
            {
                current = parent.FindByName(part);
                if (current == null)
                {
                    current = new DomainItem();
                    current.Name = part;
                    parent.Childs.Add(current);
                }
                parent = current;
            }
            return current;
        }

        /// <summary>
        /// Finds the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public DomainItem FindByName(string name)
        {
            if (Utils.StringCompareEquals(name, Name))
                return this;
            foreach (DomainItem item in childs)
            {
                DomainItem tmp = item.FindItem(name);
                if (tmp != null)
                    return tmp;
            }
            return null;
        }

        /// <summary>
        /// Retrieves the paths.
        /// </summary>
        /// <param name="result">The result.</param>
        internal void RetrievePaths(List<string> result)
        {
            if (Childs.Count == 0)
            {
                result.Add(Path);
            }
            else
            {
                foreach (DomainItem item in childs)
                {
                    item.RetrievePaths(result);
                }
            }
        }
    }
}