using System;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Classe data pour chaque ligne de VirtualTreeGrid
    /// </summary>
    [CLSCompliant(true)]
    public class VirtualTreeGridItem
    {
        private readonly VirtualTreeGridCategory category;
        private bool collapse;
        private ITypeMember _data;
        private readonly ModelKind kind;
        private readonly IHasChildren parent;
        private string tempComment = String.Empty;
        private string tempDirection = String.Empty;
        private string tempDisplayName;
        private bool tempIsCollection;
        private string tempName;
        private string tempType = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridItem"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public VirtualTreeGridItem(IHasChildren data)
        {
            category = new VirtualTreeGridCategory(true);
            kind = ModelKind.Root;
            DisplayName = Name = data.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridItem"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="category">The category.</param>
        /// <param name="kind">The kind.</param>
        public VirtualTreeGridItem(ITypeMember data, VirtualTreeGridCategory category, ModelKind kind)
        {
            this.category = category;
            this.kind = kind;
            DataItem = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridItem"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="data">The data.</param>
        /// <param name="kind">The kind.</param>
        public VirtualTreeGridItem(VirtualTreeGridCategory category, IHasChildren parent, ITypeMember data,
                                   ModelKind kind)
        {
            this.category = category;
            DataItem = data;
            this.parent = parent;
            this.kind = kind;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridItem"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="kind">The kind.</param>
        public VirtualTreeGridItem(VirtualTreeGridCategory category, IHasChildren parent, ModelKind kind)
        {
            this.category = category;
            this.parent = parent;
            this.kind = kind;
            tempName = EmptyName;
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return tempDisplayName; }
            set { tempDisplayName = value; }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public VirtualTreeGridCategory Category
        {
            get { return category; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VirtualTreeGridItem"/> is collapse.
        /// </summary>
        /// <value><c>true</c> if collapse; otherwise, <c>false</c>.</value>
        public bool Collapse
        {
            get { return collapse; }
            set { collapse = value; }
        }

        /// <summary>
        /// Gets or sets the data item.
        /// </summary>
        /// <value>The data item.</value>
        public ITypeMember DataItem
        {
            get { return _data; }
            private set
            {
                _data = value;
                tempComment = _data.Comment;
                tempName = _data.Name;
                tempDisplayName = _data.FullTypeName;
                tempType = _data.Type;
                tempIsCollection = _data.IsCollection;
                if (_data is IArgument)
                    tempDirection = ((IArgument) _data).Direction.ToString();
            }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public IHasChildren Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return tempName; }
            set { tempName = value; }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get { return tempComment; }
            set { tempComment = value; }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get { return tempType; }
            set { tempType = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is collection.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is collection; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollection
        {
            get { return tempIsCollection; }
            set { tempIsCollection = value; }
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public string Direction
        {
            get { return tempDirection; }
            set { tempDirection = value; }
        }

        /// <summary>
        /// Gets the empty name.
        /// </summary>
        /// <value>The empty name.</value>
        public string EmptyName
        {
            get { return ReadOnly || category == null ? String.Empty : category.GetEmptyText(Kind); }
        }

        /// <summary>
        /// Gets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly
        {
            get { return category != null && category.ReadOnly; }
        }

        /// <summary>
        /// Gets the kind.
        /// </summary>
        /// <value>The kind.</value>
        public ModelKind Kind
        {
            get { return kind; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is new value.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is new value; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewValue
        {
            get { return _data == null; }
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
            parent.GetChildrenForCategory(category).Remove(_data);
        }

        /// <summary>
        /// Commits the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Commit(ITypeMember data)
        {
            if (IsNewValue)
            {
                parent.GetChildrenForCategory(category).Add(data);
                DataItem = data;
            }
        }
    }
}