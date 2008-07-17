using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainItemCollection : List<DomainItem>
    {
        private readonly DomainItem _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainItemCollection"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public DomainItemCollection(DomainItem owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Generic.List`1"></see>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:System.Collections.Generic.List`1"></see>. The value can be null for reference types.</param>
        public new void Add(DomainItem item)
        {
            item.Parent = _owner;
            base.Add(item);
        }


        /// <summary>
        /// Recherche d'un item par son chemin
        /// </summary>
        /// <param name="path">Chemin sous la forme (nom/nom2/....)</param>
        /// <returns></returns>
        public DomainItem FindItem(string path)
        {
            foreach (DomainItem item in this)
            {
                DomainItem tmp = item.FindItem(path);
                if (tmp != null)
                    return tmp;
            }
            return null;
        }


        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        internal DomainItem CreateItem(string path)
        {
            string[] parts = path.Split(DomainManager.PathSeparator);

            foreach (DomainItem item in this)
            {
                // Recherche du domaine
                if (Utils.StringCompareEquals(item.Name, parts[0]))
                {
                    return item.CreateItem(path);
                }
            }
            DomainItem domain = new DomainItem();
            domain.Name = parts[0];
            Add(domain);
            return domain.CreateItem(path);
        }
    }
}