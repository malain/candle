using System;

namespace DSLFactory.Candle.SystemModel.Utilities
{

    #region OnDataChanged

    /// <summary>
    /// 
    /// </summary>
    public class VirtualTreeGridDataChangedEventsArgs : EventArgs
    {
        private readonly bool _isDelete;
        private readonly VirtualTreeGridItem _item;
        private bool _cancel = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridDataChangedEventsArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="delete">if set to <c>true</c> [delete].</param>
        public VirtualTreeGridDataChangedEventsArgs(VirtualTreeGridItem item, bool delete)
        {
            _isDelete = delete;
            _item = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGridDataChangedEventsArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public VirtualTreeGridDataChangedEventsArgs(VirtualTreeGridItem item) : this(item, false)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VirtualTreeGridDataChangedEventsArgs"/> is cancel.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is delete.
        /// </summary>
        /// <value><c>true</c> if this instance is delete; otherwise, <c>false</c>.</value>
        public bool IsDelete
        {
            get { return _isDelete; }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>The item.</value>
        [CLSCompliant(false)]
        public VirtualTreeGridItem Item
        {
            get { return _item; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void DataChangedEventHandler(object sender, VirtualTreeGridDataChangedEventsArgs e);

    #endregion
}