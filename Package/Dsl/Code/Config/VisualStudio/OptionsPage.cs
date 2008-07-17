using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Shell;

namespace DSLFactory.Candle.SystemModel.Configuration.VisualStudio
{
    /// <summary>
    /// Stockage des données de configuration dans l'IDE
    /// </summary>
    [CLSCompliant(false)]
    [Guid("5142D00D-D403-430c-91F8-5429B5F23AB5")]
    public class OptionsPage : DialogPage
    {
        private string _baseDirectory;
        private string _localRepositoryPath;
        private string _repositoryUrl;
        private bool _repositoryEnabled;
        private readonly OptionsPageControl _optionsPageControl;
        private static OptionsPage s_instance;
        private bool _isInitialized;
        private string _licenseId;
        private int _repositoryDelaiCache;
        private String _currentDomainId = String.Empty;
        private bool _generationTraceEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPage"/> class.
        /// </summary>
        public OptionsPage()
        {
            _optionsPageControl = new OptionsPageControl();
            _optionsPageControl.Location = new Point( 0, 0 );
            _optionsPageControl.OptionsPage = this;

            // Nom de la sous-clé de registre où sont stockés les données
            SettingsRegistryPath = ModelConstants.ApplicationName;
        }

        /// <summary>
        /// Gets the page control.
        /// </summary>
        /// <value>The page control.</value>
        internal OptionsPageControl PageControl
        {
            get { return _optionsPageControl; }
        }

        //
        /// <summary>
        /// Called by Visual Studio to load a dialog page's settings from local storage, generally the registry.
        /// </summary>
        public override void LoadSettingsFromStorage()
        {
            try
            {
                base.LoadSettingsFromStorage();
                _isInitialized = !String.IsNullOrEmpty( this._baseDirectory );
            }
            catch
            {
                _isInitialized = false;
            }
            _optionsPageControl.OptionsPage=this;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return this._isInitialized; }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static OptionsPage Instance
        {
            get { return s_instance; }
        }

        /// <summary>
        /// Called by Visual Studio to store a dialog page's settings in local storage, generally the registry.
        /// </summary>
        public override void SaveSettingsToStorage()
        {
            if (_optionsPageControl != null)
            {
                _optionsPageControl.CommitChanges();
                ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
                if( notifier !=null)
                    notifier.NotifyOptionsChanged(this);
            }

             base.SaveSettingsToStorage();
        }


        /// <summary>
        /// Gets the window that is used as the user interface of the dialog page.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Windows.Forms.IWin32Window"></see> providing the handle to the window acting as the user interface for dialog page.</returns>
        [Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        protected override IWin32Window Window
        {
            get
            {
                return _optionsPageControl;
            }
        }


        /// <summary>
        /// Gets or sets the repository URL.
        /// </summary>
        /// <value>The repository URL.</value>
        public string RepositoryUrl
        {
            get { return _repositoryUrl; }
            set
            {
                if( value != null && value.EndsWith( "/" ) )
                    value = value.Substring( 0, value.Length-1 );
                _repositoryUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the repository path.
        /// </summary>
        /// <value>The repository path.</value>
        [Editor( typeof( FileNameEditor ), typeof( UITypeEditor ) )]
        public string RepositoryPath
        {
            get { return _localRepositoryPath; }
            set
            {
                _localRepositoryPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string BaseDirectory
        {
            get { return _baseDirectory; }
            set
            {
                _baseDirectory = value;
            }
        }

        /// <summary>
        /// Gets or sets the repository delai cache.
        /// </summary>
        /// <value>The repository delai cache.</value>
        public int RepositoryDelaiCache
        {
            get { return _repositoryDelaiCache; }
            set { _repositoryDelaiCache = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [generation trace enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generation trace enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerationTraceEnabled
        {
            get { return _generationTraceEnabled; }
            set { _generationTraceEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the current domain id.
        /// </summary>
        /// <value>The current domain id.</value>
        public string CurrentDomainId
        {
            get { return _currentDomainId; }
            set { _currentDomainId = value; }
        }

        /// <summary>
        /// Gets or sets the license id.
        /// </summary>
        /// <value>The license id.</value>
        public string LicenseId
        {
            get { return this._licenseId; }
            set { this._licenseId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [repository enabled].
        /// </summary>
        /// <value><c>true</c> if [repository enabled]; otherwise, <c>false</c>.</value>
        public bool RepositoryEnabled
        {
            get { return _repositoryEnabled; }
            set
            {
                _repositoryEnabled = value;
            }
        }

        /// <summary>
        /// Sets the instance.
        /// </summary>
        /// <param name="dialogPage">The dialog page.</param>
        public static void SetInstance( DialogPage dialogPage )
        {
            s_instance = dialogPage as OptionsPage;            
        }
    }
}
