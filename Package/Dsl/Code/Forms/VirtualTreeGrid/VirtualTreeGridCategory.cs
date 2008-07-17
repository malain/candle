using System;
using System.Drawing;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(true)]
    public class VirtualTreeGridCategory
    {
        private string name;
        /// <summary>
        /// 
        /// </summary>
        private bool readOnly;
        private int id;
        private Bitmap icon;

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public Bitmap Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridCategory"/> class.
        /// </summary>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        public VirtualTreeGridCategory(bool readOnly)
        {
            this.readOnly = readOnly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridCategory"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="id">The id.</param>
        /// <param name="icon">The icon.</param>
        public VirtualTreeGridCategory(string name, bool readOnly, int id, Bitmap icon)
        {
            this.icon = icon;
            this.name = name;
            this.readOnly = readOnly;
            this.id = id;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return name == null; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set {
                if (value == null)
                    throw new ArgumentNullException("Name");
                name = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Gets the empty text.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <returns></returns>
        internal virtual string GetEmptyText(ModelKind kind)
        {
            return  kind == ModelKind.Member ? "<add member>" : "<add argument>";
        }
    }
}