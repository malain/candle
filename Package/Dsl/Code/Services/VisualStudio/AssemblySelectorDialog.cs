using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AssemblySelectorDialog : IVsComponentUser, IAssemblySelectorDialog
    {
        private IServiceProvider _serviceProvider;
        private List<Assembly> _selectedAssemblies;
        private int _max;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblySelectorDialog"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public AssemblySelectorDialog(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Liste des assemblies sélectionnées
        /// </summary>
        /// <value></value>
        public List<Assembly> SelectedAssemblies
        {
            get { return _selectedAssemblies; }
        }

        /// <summary>
        /// Affichage de la fenetre de sélection
        /// </summary>
        /// <param name="max">Nbre maximun d'assembly à sélectionner</param>
        /// <returns></returns>
        public bool ShowDialog(int max)
        {
            this._max = max;
            _selectedAssemblies = new List<Assembly>();
            IVsComponentSelectorDlg componentDialog;
            Guid guidEmpty = Guid.Empty;
            VSCOMPONENTSELECTORTABINIT[] tabInit = new VSCOMPONENTSELECTORTABINIT[0];
            string strBrowseLocations = ""; // Path.GetDirectoryName(this.BaseURI.Uri.LocalPath);

            //tabInit[0].dwSize = (uint)Marshal.SizeOf(tabInit[0]);
            //// Tell the Add Reference dialog to call hierarchies GetProperty with the following 
            //// propID to enablefiltering out ourself from the Project to Project reference 
            //tabInit[0].varTabInitInfo = (int)__VSHPROPID.VSHPROPID_ShowProjInSolutionPage;
            //tabInit[0].guidTab = VSConstants.GUID_SolutionPage;

            componentDialog = _serviceProvider.GetService( typeof( IVsComponentSelectorDlg ) ) as IVsComponentSelectorDlg;
            try
            {
                // call the container to open the add reference dialog. 
                if( componentDialog != null )
                {
                    // call the container to open the add reference dialog. 
                    UInt32 flag = max == 1 ? (UInt32)__VSCOMPSELFLAGS.VSCOMSEL_IgnoreMachineName : (UInt32)( __VSCOMPSELFLAGS.VSCOMSEL_MultiSelectMode | __VSCOMPSELFLAGS.VSCOMSEL_IgnoreMachineName );
                    ErrorHandler.ThrowOnFailure(
                    componentDialog.ComponentSelectorDlg(
                        flag,
                        (IVsComponentUser)this,
                        "Add reference",                             // Title 
                        "VS.AddReference",                           // Help topic 
                        ref guidEmpty,
                        ref guidEmpty,
                        String.Empty,                                // Machine Name 
                        (uint)tabInit.Length,
                        tabInit,
                        "*.dll",
                        ref strBrowseLocations )
                        );
                }
                componentDialog = null;
                return _selectedAssemblies.Count > 0;
            }
            catch( COMException  )
            {
                return false;
            }
        }

        #region IVsComponentUser Members
        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="dwAddCompOperation">The dw add comp operation.</param>
        /// <param name="cComponents">The c components.</param>
        /// <param name="rgpcsdComponents">The RGPCSD components.</param>
        /// <param name="hwndPickerDlg">The HWND picker DLG.</param>
        /// <param name="pResult">The p result.</param>
        /// <returns></returns>
        int IVsComponentUser.AddComponent( VSADDCOMPOPERATION dwAddCompOperation, uint cComponents, IntPtr[] rgpcsdComponents, IntPtr hwndPickerDlg, VSADDCOMPRESULT[] pResult )
        {
            VSADDCOMPRESULT result = VSADDCOMPRESULT.ADDCOMPRESULT_Success;
            int returnValue = VSConstants.S_OK;
            try
            {
                for( int cCount = 0; cCount < cComponents; cCount++ )
                {
                    VSCOMPONENTSELECTORDATA selectorData = new VSCOMPONENTSELECTORDATA();
                    IntPtr ptr = rgpcsdComponents[cCount];
                    selectorData = (VSCOMPONENTSELECTORDATA)Marshal.PtrToStructure( ptr, typeof( VSCOMPONENTSELECTORDATA ) );
                    _selectedAssemblies.Add( Assembly.LoadFile( selectorData.bstrFile ) );
                    if( _max > 0 && cCount == _max)
                        break;
                }
            }
            catch( BadImageFormatException )
            {
                //  Trace.WriteLine("Exception : " + e.Message);
                result = VSADDCOMPRESULT.ADDCOMPRESULT_Failure;
                //string message = e.Message;
                //string title = string.Empty;
                //OLEMSGICON icon = OLEMSGICON.OLEMSGICON_CRITICAL;
                //OLEMSGBUTTON buttons = OLEMSGBUTTON.OLEMSGBUTTON_OK;
                //OLEMSGDEFBUTTON defaultButton = OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;
                //VsShellUtilities.ShowMessageBox(this.Site, title, message, icon, buttons, defaultButton);
            }
            pResult[0] = result;
            return returnValue;
        }

        #endregion
    }
}
