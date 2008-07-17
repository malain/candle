using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Data.Interop;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.Data;
using System.Data;
 
namespace DSLFactory.Candle.SystemModel.ServerExplorer.Utils
{
    /// <summary>
    /// The DSRefNavigator is used to enumerate the DSRef Consumer data source
    /// and provide details about the Server Explorer's selection.
    /// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=75909&SiteID=1
    /// </summary>

    internal class DSRefNavigator : DSLFactory.Candle.SystemModel.Utilities.IDSRefNavigator, IDisposable
    {
        private IDbConnection dataConnection;

        /// <summary>
        /// Clipboard format of the data source reference objects in the
        /// Server Explorer window of Visual Studio
        /// </summary>
        public static readonly string DataSourceReferenceFormat = "CF_DSREF";

        /// <summary>
        /// Root node of the DSRef Consumer selection result tree
        /// </summary>
        private static readonly IntPtr RootNode = IntPtr.Zero;

        /// <summary>
        /// Create a COM stream from a pointer in unmanaged memory
        /// </summary>
        [DllImport( "ole32.dll" )]
        private static extern int CreateStreamOnHGlobal( IntPtr ptr, bool delete,
          ref IntPtr stream );

        /// <summary>
        /// Load an OLE object from a COM stream
        /// </summary>
        [DllImport( "ole32.dll" )]
        private static extern int OleLoadFromStream( IntPtr stream, byte[] iid,
          ref IntPtr obj );

        /// <summary>
        /// DSRef Consumer that provides a tree-like structure of the selected items
        /// </summary>
        private IDSRefConsumer _consumer;

        /// <summary>
        /// Constructor to create the DSRef navigator from a stream
        /// </summary>
        /// <param name="data">Stream containing the DSRef consumer</param>
        public DSRefNavigator( Stream data )
        {
            _consumer = null;

            // Pointers to unmanaged resources
            IntPtr ptr = IntPtr.Zero;
            IntPtr stream = IntPtr.Zero;
            IntPtr native = IntPtr.Zero;

            // Get a reference to the DSRef Consumer from the stream
            try
            {
                // Read the stream containing the DSRef Consumer
                byte[] buffer = new byte[data.Length];
                data.Seek( 0, SeekOrigin.Begin );
                data.Read( buffer, 0, buffer.Length );

                // Copy the DSRef Consumer to native memory
                native = Marshal.AllocHGlobal( buffer.Length );
                Marshal.Copy( buffer, 0, native, buffer.Length );

                // Create a COM stream from the memory
                int result = CreateStreamOnHGlobal( native, false, ref stream );
                Marshal.ThrowExceptionForHR( result );

                // Load the DSRef Consumer from the COM stream
                result = OleLoadFromStream( stream,
                  typeof( IDSRefConsumer ).GUID.ToByteArray(), ref ptr );
                Marshal.ThrowExceptionForHR( result );

                // Get the DSRef consumer
                _consumer = Marshal.GetObjectForIUnknown( ptr ) as IDSRefConsumer;
            }
            finally
            {
                // Release all of the unmanaged resources
                Marshal.Release( ptr );
                Marshal.Release( stream );
                Marshal.Release( native );
            }
        }

        /// <summary>
        /// Dispose resources used by the navigator
        /// </summary>
        public void Dispose()
        {
            // Release the reference to the DSRef Consumer
            if( _consumer != null )
                Marshal.ReleaseComObject( _consumer );
        }

        public IDbConnection DataConnection
        {
            get { return dataConnection; }
        }

        /// <summary>
        /// Indicate whether or not the selection represented by this DSRef Consumer
        /// contains only tables as leaf nodes
        /// </summary>
        public bool ContainsOnlyTables
        {
            get
            {
                // Determine if one of the leaf nodes has a table by querying
                // the type of the root node
                return CheckRefType(__DSREFTYPE.DSREFTYPE_TABLE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ContainsOnlyStoredProcedures
        {
            get
            {
                // Determine if one of the leaf nodes has a proc stoc by querying
                // the type of the root node
                return CheckRefType(__DSREFTYPE.DSREFTYPE_STOREDPROCEDURE);
            }
        }

        private bool CheckRefType(__DSREFTYPE type)
        {
            if (_consumer != null)
            {
                try
                {
                    return (_consumer.GetType(RootNode) & type) == type;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// Enumerate the names of the tables in the selection represented by
        /// the DSRef Consumer
        /// </summary>
        public IEnumerable<DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbContainer> GetStoredProcedures(System.IServiceProvider serviceProvider)
        {
            foreach (IntPtr child in ChildNodes)
            {
                __DSREFTYPE type = _consumer.GetType(child);
                string name = String.Empty;
                if ((type & __DSREFTYPE.DSREFTYPE_HASNAME) == __DSREFTYPE.DSREFTYPE_HASNAME)
                    name = _consumer.GetName(child);

                if ((type & __DSREFTYPE.DSREFTYPE_STOREDPROCEDURE) == __DSREFTYPE.DSREFTYPE_STOREDPROCEDURE)
                {
                    DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbContainer sp = new DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbContainer();
                    sp.Name = name;
                    sp.Owner = string.Empty;
                    if ((type & __DSREFTYPE.DSREFTYPE_HASOWNER) == __DSREFTYPE.DSREFTYPE_HASOWNER)
                        sp.Owner = _consumer.GetOwner(child);

                    yield return sp;
                }
                else if ((type & __DSREFTYPE.DSREFTYPE_DATASOURCEROOT) == __DSREFTYPE.DSREFTYPE_DATASOURCEROOT)
                {
                    Guid guid1 = new Guid("B30985D6-6BBB-45f2-9AB8-371664F03270");
                    string text1 = _consumer.GetProperty(child, ref guid1) as string;
                    Guid guid2 = Guid.Empty;
                    if (!String.IsNullOrEmpty(text1))
                    {
                        guid2 = new Guid(text1);
                    }
                    DataConnection connection = ((DataConnectionManager)serviceProvider.GetService(typeof(DataConnectionManager))).GetDataConnection(guid2, name, false);
                    if (connection != null)
                        dataConnection = connection.ConnectionSupport.ProviderObject as IDbConnection;
                }
            }
        }

        /// <summary>
        /// Enumerate the names of the tables in the selection represented by
        /// the DSRef Consumer
        /// </summary>
        public IEnumerable<DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbTable> GetTables( System.IServiceProvider serviceProvider )
        {
            foreach( IntPtr child in ChildNodes )
            {
                __DSREFTYPE type = _consumer.GetType( child );
                string name = String.Empty;
                if( ( type & __DSREFTYPE.DSREFTYPE_HASNAME ) ==  __DSREFTYPE.DSREFTYPE_HASNAME )
                    name = _consumer.GetName( child );

                if( ( type & __DSREFTYPE.DSREFTYPE_TABLE ) == __DSREFTYPE.DSREFTYPE_TABLE )
                {
                    DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbTable table = new DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbTable();
                    table.Name = name;
                    table.Owner = string.Empty;
                    if( ( type & __DSREFTYPE.DSREFTYPE_HASOWNER ) ==  __DSREFTYPE.DSREFTYPE_HASOWNER )
                        table.Owner = _consumer.GetOwner( child );

                    yield return table;
                }
                else if( ( type & __DSREFTYPE.DSREFTYPE_DATASOURCEROOT ) == __DSREFTYPE.DSREFTYPE_DATASOURCEROOT )
                {
                    Guid guid1 = new Guid( "B30985D6-6BBB-45f2-9AB8-371664F03270" );
                    string text1 = _consumer.GetProperty( child, ref guid1 ) as string;
                    Guid guid2 = Guid.Empty;
                    if( !String.IsNullOrEmpty( text1 ) )
                    {
                        guid2 = new Guid( text1 );
                    }
                    DataConnection connection = ( (DataConnectionManager)serviceProvider.GetService( typeof( DataConnectionManager ) ) ).GetDataConnection( guid2, name, false );
                    if( connection != null )
                        dataConnection = connection.ConnectionSupport.ProviderObject as IDbConnection;
                }
            }
        }

        /// <summary>
        /// Enumerate all of the child nodes in the selection 
        /// </summary>
        /// <returns>Iterator for the child nodes in the selection</returns>
        private IEnumerable<IntPtr> ChildNodes
        {
            get
            {
                // Instead of recursing, we'll just continually iterate over the queue
                Queue<IntPtr> parents = new Queue<IntPtr>();
                parents.Enqueue( RootNode );
                while( parents.Count > 0 )
                {
                    IntPtr parent = parents.Dequeue();
                    IntPtr child = _consumer.GetFirstChildNode( parent );
                    while( child != IntPtr.Zero )
                    {
                        yield return child;
                        parents.Enqueue( child );
                        child = _consumer.GetNextSiblingNode( child );
                    }
                }
            }
        }

        /// <summary>
        /// Display the Server Explorer selection represented by the DSRef
        /// Consumer in a non-modal form. This should be implemented as a
        /// Debugger Visualizer but I'm too lazy to handle serializing the
        /// COM object.
        /// </summary>
        [Conditional( "DEBUG" )]
        public void ShowVisualizer()
        {
            ( new DSRefNavigatorVisualizer( _consumer ) ).Show();
        }

        /// <summary>
        /// Display the selection from the Server Explorer represented by the
        /// DSRef Consumer in a TreeView. We include both the name of the node
        /// as well as type and structural information.
        /// </summary>
        private class DSRefNavigatorVisualizer : System.Windows.Forms.Form
        {
            /// <summary>
            /// Constructor to initialize and display the form
            /// </summary>
            /// <param name="consumer">DSRef Consumer with selection to display</param>
            public DSRefNavigatorVisualizer( IDSRefConsumer consumer )
            {
                // Add the tree to the form
                Text = "DSRef Consumer Selection Debug Display";
                TreeView tree = new TreeView();
                tree.Dock = DockStyle.Fill;
                Controls.Add( tree );
                tree.BeginUpdate();

                // Create the root node and build the tree
                TreeNode root = new TreeNode();
                BuildTree( consumer, DSRefNavigator.RootNode, root );

                // Display the debuging form
                tree.Nodes.Add( root );
                tree.ExpandAll();
                tree.EndUpdate();
            }

            /// <summary>
            /// Build a tree of the DSRef Consumer's selection starting with the provided
            /// node in the DSRef Consumer and node in the TreeView
            /// </summary>
            /// <param name="consumer">DSRef Consumer containing the selection</param>
            /// <param name="node">Current node in the DSRef Consumer</param>
            /// <param name="treeNode">Node in the tree</param>
            private void BuildTree( IDSRefConsumer consumer, IntPtr node,
              System.Windows.Forms.TreeNode treeNode )
            {
                // Copy the properties for the current node given its type
                treeNode.Text = "";
                __DSREFTYPE type = consumer.GetType( node );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASNAME ) ==
            __DSREFTYPE.DSREFTYPE_HASNAME )
                    treeNode.Text = string.Format( "{0} ", consumer.GetName( node ) );
                treeNode.Text += string.Format( "(Type: {0}", GetNodeTypes( type ) );
                string structure = GetNodeStructure( type );
                if( !string.IsNullOrEmpty( structure ) )
                    treeNode.Text += string.Format( " || Structure: {0}", structure );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASOWNER ) ==
             __DSREFTYPE.DSREFTYPE_HASOWNER )
                    treeNode.Text += string.Format( " || Owner: {0}", consumer.GetOwner( node ) );
                treeNode.Text += ")";

                // Iterate over the children
                IntPtr child = consumer.GetFirstChildNode( node );
                while( child != IntPtr.Zero )
                {
                    // Create a tree node for the child and set its properties
                    TreeNode next = new TreeNode();
                    treeNode.Nodes.Add( next );
                    BuildTree( consumer, child, next );

                    // Move to the next child
                    child = consumer.GetNextSiblingNode( child );
                }
            }

            /// <summary>
            /// Convert the type of a node to a string
            /// </summary>
            /// <param name="type">Type of a DSRef node</param>
            /// <returns>String representation of the node type</returns>
            private string GetNodeTypes( __DSREFTYPE type )
            {
                // Convert the flag enumeration to a set of legible strings
                List<string> names = new List<string>();
                if( ( type & __DSREFTYPE.DSREFTYPE_COLLECTION ) ==
            __DSREFTYPE.DSREFTYPE_COLLECTION )
                    names.Add( "Collection" );
                if( ( type & __DSREFTYPE.DSREFTYPE_MULTIPLE ) ==
            __DSREFTYPE.DSREFTYPE_MULTIPLE )
                    names.Add( "Multiple Selection" );
                if( ( type & __DSREFTYPE.DSREFTYPE_MIXED ) == 
            __DSREFTYPE.DSREFTYPE_MIXED )
                    names.Add( "Collecion of Multiple Selections" );
                if( ( type & __DSREFTYPE.DSREFTYPE_DATABASE ) == 
            __DSREFTYPE.DSREFTYPE_DATABASE )
                    names.Add( "Database" );
                if( ( type & __DSREFTYPE.DSREFTYPE_DATASOURCEROOT ) == 
            __DSREFTYPE.DSREFTYPE_DATASOURCEROOT )
                    names.Add( "Data Source Root" );
                if( ( type & __DSREFTYPE.DSREFTYPE_EXTENDED ) == 
            __DSREFTYPE.DSREFTYPE_EXTENDED )
                    names.Add( "[Extended]" );
                if( ( type & __DSREFTYPE.DSREFTYPE_FIELD ) == 
            __DSREFTYPE.DSREFTYPE_FIELD )
                    names.Add( "Field" );
                if( ( type & __DSREFTYPE.DSREFTYPE_FUNCTION ) == 
            __DSREFTYPE.DSREFTYPE_FUNCTION )
                    names.Add( "Function" );
                if( ( type & __DSREFTYPE.DSREFTYPE_INDEX ) == 
            __DSREFTYPE.DSREFTYPE_INDEX )
                    names.Add( "Index" );
                if( ( type & __DSREFTYPE.DSREFTYPE_PACKAGE ) == 
            __DSREFTYPE.DSREFTYPE_PACKAGE )
                    names.Add( "Package" );
                if( ( type & __DSREFTYPE.DSREFTYPE_PACKAGEBODY ) == 
            __DSREFTYPE.DSREFTYPE_PACKAGEBODY )
                    names.Add( "Package Body" );
                if( ( type & __DSREFTYPE.DSREFTYPE_QUERY ) == 
            __DSREFTYPE.DSREFTYPE_QUERY )
                    names.Add( "Query" );
                if( ( type & __DSREFTYPE.DSREFTYPE_RELATIONSHIP ) == 
            __DSREFTYPE.DSREFTYPE_RELATIONSHIP )
                    names.Add( "Relationship" );
                if( ( type & __DSREFTYPE.DSREFTYPE_SCHEMADIAGRAM ) == 
            __DSREFTYPE.DSREFTYPE_SCHEMADIAGRAM )
                    names.Add( "Schema Diagram" );
                if( ( type & __DSREFTYPE.DSREFTYPE_STOREDPROCEDURE ) == 
            __DSREFTYPE.DSREFTYPE_STOREDPROCEDURE )
                    names.Add( "Stored Procedure" );
                if( ( type & __DSREFTYPE.DSREFTYPE_SYNONYM ) == 
            __DSREFTYPE.DSREFTYPE_SYNONYM )
                    names.Add( "Synonym" );
                if( ( type & __DSREFTYPE.DSREFTYPE_TABLE ) == 
            __DSREFTYPE.DSREFTYPE_TABLE )
                    names.Add( "Table" );
                if( ( type & __DSREFTYPE.DSREFTYPE_TRIGGER ) == 
            __DSREFTYPE.DSREFTYPE_TRIGGER )
                    names.Add( "Trigger" );
                if( ( type & __DSREFTYPE.DSREFTYPE_USERDEFINEDTYPE ) == 
            __DSREFTYPE.DSREFTYPE_USERDEFINEDTYPE )
                    names.Add( "User Defined Type" );
                if( ( type & __DSREFTYPE.DSREFTYPE_VIEW ) == 
            __DSREFTYPE.DSREFTYPE_VIEW )
                    names.Add( "View" );
                if( ( type & __DSREFTYPE.DSREFTYPE_VIEWINDEX ) == 
            __DSREFTYPE.DSREFTYPE_VIEWINDEX )
                    names.Add( "View Index" );
                if( ( type & __DSREFTYPE.DSREFTYPE_VIEWTRIGGER ) == 
            __DSREFTYPE.DSREFTYPE_VIEWTRIGGER )
                    names.Add( "View Trigger" );
                return string.Join( ", ", names.ToArray() );
            }

            /// <summary>
            /// Convert the node structure to a string. Note that these properties are
            /// not intended to be used by a normal application and are not necessarily
            /// stable. They are only provided to aid in debugging.
            /// </summary>
            /// <param name="type">DSRef node type</param>
            /// <returns>Description of the structure</returns>
            private string GetNodeStructure( __DSREFTYPE type )
            {
                // Convert the flag enumeration to a set of legible strings
                List<string> structure = new List<string>();
                if( ( type & __DSREFTYPE.DSREFTYPE_NODE ) ==
            __DSREFTYPE.DSREFTYPE_NODE )
                    structure.Add( "Node" );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASNAME ) == 
            __DSREFTYPE.DSREFTYPE_HASNAME )
                    structure.Add( "Name" );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASMONIKER ) == 
            __DSREFTYPE.DSREFTYPE_HASMONIKER )
                    structure.Add( "Moniker" );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASOWNER ) == 
            __DSREFTYPE.DSREFTYPE_HASOWNER )
                    structure.Add( "Owner" );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASPROP ) ==
            __DSREFTYPE.DSREFTYPE_HASPROP )
                    structure.Add( "Additional Properties" );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASFIRSTCHILD ) ==
            __DSREFTYPE.DSREFTYPE_HASFIRSTCHILD )
                    structure.Add( "Children" );
                if( ( type & __DSREFTYPE.DSREFTYPE_HASNEXTSIBLING ) ==
            __DSREFTYPE.DSREFTYPE_HASNEXTSIBLING )
                    structure.Add( "Sibling" );
                return string.Join( ", ", structure.ToArray() );
            }
        }
    }
}