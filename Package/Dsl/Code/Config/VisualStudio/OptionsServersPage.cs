using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace DSLFactory.Candle.SystemModel.Configuration.VisualStudio
{
    /// <summary>
    /// Stockage des données de configuration dans l'IDE
    /// </summary>
    [CLSCompliant(false)]
    [Guid("BC6AE30C-6A2D-4b8f-A7D4-309720ABB636")]
    public class OptionsServersPage : DialogPage
    {
        //public event EventHandler PropertiesChanged;

        private static OptionsServersPage s_instance;
        private List<string> _globalServers = new List<string>();
        private readonly OptionsServersPageControl _optionsServersPageControl;
        private string _proxy;
        private string _proxyPassword;
        private string _proxyUser;
        private int _repositoryDelaiCache;
        private bool _useDefaultProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsServersPage"/> class.
        /// </summary>
        public OptionsServersPage()
        {
            _optionsServersPageControl = new OptionsServersPageControl();
            _optionsServersPageControl.Location = new Point(0, 0);
            _optionsServersPageControl.OptionsPage = this;

            // Nom de la sous-clé de registre où sont stockés les données
            SettingsRegistryPath = ModelConstants.ApplicationName;
        }

        /// <summary>
        /// Gets the page control.
        /// </summary>
        /// <value>The page control.</value>
        internal OptionsServersPageControl PageControl
        {
            get { return _optionsServersPageControl; }
        }

        //

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static OptionsServersPage Instance
        {
            get { return s_instance; }
        }


        /// <summary>
        /// Gets the window that is used as the user interface of the dialog page.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Windows.Forms.IWin32Window"></see> providing the handle to the window acting as the user interface for dialog page.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get { return _optionsServersPageControl; }
        }

        /// <summary>
        /// Gets or sets the proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        public string ProxyPassword
        {
            get { return _proxyPassword; }
            set { _proxyPassword = value; }
        }

        /// <summary>
        /// Gets or sets the proxy user.
        /// </summary>
        /// <value>The proxy user.</value>
        public string ProxyUser
        {
            get { return _proxyUser; }
            set { _proxyUser = value; }
        }

        /// <summary>
        /// Gets or sets the proxy address.
        /// </summary>
        /// <value>The proxy address.</value>
        public string ProxyAddress
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use default proxy].
        /// </summary>
        /// <value><c>true</c> if [use default proxy]; otherwise, <c>false</c>.</value>
        public bool UseDefaultProxy
        {
            get { return _useDefaultProxy; }
            set { _useDefaultProxy = value; }
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
        /// Gets or sets the global servers.
        /// </summary>
        /// <value>The global servers.</value>
        public List<string> GlobalServers
        {
            get { return _globalServers; }
            set { _globalServers = value; }
        }

        /// <summary>
        /// Called by Visual Studio to load a dialog page's settings from local storage, generally the registry.
        /// </summary>
        public override void LoadSettingsFromStorage()
        {
            try
            {
                base.LoadSettingsFromStorage();
            }
            catch
            {
            }
            _optionsServersPageControl.InitData();
        }

        /// <summary>
        /// Called by Visual Studio to store a dialog page's settings in local storage, generally the registry.
        /// </summary>
        public override void SaveSettingsToStorage()
        {
            if (_optionsServersPageControl != null)
            {
                _optionsServersPageControl.CommitChanges();
                ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
                if (notifier != null)
                    notifier.NotifyOptionsChanged(this);
            }

            base.SaveSettingsToStorage();
        }

        /// <summary>
        /// Sets the instance.
        /// </summary>
        /// <param name="dialogPage">The dialog page.</param>
        public static void SetInstance(DialogPage dialogPage)
        {
            s_instance = dialogPage as OptionsServersPage;
        }
    }
}