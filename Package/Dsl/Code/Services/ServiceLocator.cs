using System;
using System.Collections.Generic;
using System.Diagnostics;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Fournisseur de services
    /// </summary>
    public class ServiceLocator : IServiceProvider 
    {
        private static readonly ServiceLocator s_instance = new ServiceLocator();
        private IServiceProvider _serviceProvider;
        private IRepositorySettingsStorage _repositorySettingsStorage;
        private Dictionary<Type, object> _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        private ServiceLocator()
        {
        }

#if DEBUG
        internal static bool IsDesignMode
        {
            get{return s_instance == null || s_instance._services == null;}
        }
#endif
        /// <summary>
        /// Singleton
        /// </summary>
        public static ServiceLocator Instance
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return s_instance; }
        }

        /// <summary>
        /// Gets or sets the repository settings storage.
        /// </summary>
        /// <value>The repository settings storage.</value>
        public IRepositorySettingsStorage RepositorySettingsStorage
        {
            get
            {
                if (_repositorySettingsStorage == null)
                    _repositorySettingsStorage = GetService<IRepositorySettingsStorage>();
                return _repositorySettingsStorage;
            }
            set
            {
                _repositorySettingsStorage = value;
            }
        }

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="impl">The impl.</param>
        public void AddService(Type type, object impl)
        {
            if (_services == null)
                _services = new Dictionary<Type, object>();
            if (_services.ContainsKey(type))
                _services.Remove(type);
            _services.Add(type, impl);
        }

        /// <summary>
        /// Initializes for visual studio.
        /// </summary>
        /// <param name="package">The package.</param>
        internal void InitializeForVisualStudio(Microsoft.VisualStudio.Modeling.Shell.ModelingPackage package)
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)ModelingPackage.GetGlobalService(typeof(EnvDTE.DTE));
            _serviceProvider = package;

            _services = new Dictionary<Type, object>();

            // Tjs ces 2 en premier car ils peuvent être utilisées par les autres
            _services.Add(typeof(ILogger), new DSLFactory.Candle.SystemModel.VisualStudio.Logger());
            _services.Add(typeof(ICandleNotifier), new CandleNotifier());

            _services.Add(typeof(ICacheProvider), new DSLFactory.Candle.SystemModel.Utilities.BasicCache());
            _services.Add(typeof(IShellHelper), new ShellHelper());
            _services.Add(typeof(IDatabaseImporter), new DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DatabaseImporter());
            _services.Add(typeof(EnvDTE.DTE), dte);
            _services.Add( typeof(IIDEHelper), new DSLFactory.Candle.SystemModel.CodeGeneration.IDEHelper(package));
            _services.Add(typeof(IImportEntityHelper), new DSLFactory.Candle.SystemModel.Commands.ImportEntityHelper());
            _services.Add(typeof(IImportInterfaceHelper), new DSLFactory.Candle.SystemModel.Commands.ImportInterfaceHelper());
            _services.Add(typeof(IAssemblySelectorDialog), new DSLFactory.Candle.SystemModel.VisualStudio.AssemblySelectorDialog(_serviceProvider));
            _services.Add(typeof(IRepositorySettingsStorage), new DSLFactory.Candle.SystemModel.Config.IDERepositorySettingsStorage());
            _services.Add(typeof(IDialogService), new DSLFactory.Candle.SystemModel.VisualStudio.DialogService());

            //try
            //{
            //    System.Reflection.Assembly asm = System.Reflection.Assembly.Load("DSLFactory.Candle.TFSVersionControl");
            //    Type type = asm.GetType("DSLFactory.Candle.TFSVersionControl.TFSService");
            //    services.Add(typeof(IVersionControl), Activator.CreateInstance(type));
            //}
            //catch { }

        }

        /// <summary>
        /// Gets the IDE helper.
        /// </summary>
        /// <value>The IDE helper.</value>
        public IIDEHelper IDEHelper
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get
            {
                IIDEHelper service = GetService<IIDEHelper>();
                return service;                    
            }
        }

        /// <summary>
        /// Gets the shell helper.
        /// </summary>
        /// <value>The shell helper.</value>
        public IShellHelper ShellHelper
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get
            {
                IShellHelper service = GetService<IShellHelper>();
                return service;  
            }
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <value>The cache.</value>
        public ICacheProvider Cache
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get
            {
                ICacheProvider service = GetService<ICacheProvider>();
                return service;
            }
        }
        /// <summary>
        /// Gets the database importer.
        /// </summary>
        /// <value>The database importer.</value>
        public IDatabaseImporter DatabaseImporter
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get
            {
                IDatabaseImporter service = GetService<IDatabaseImporter>();
                return service;
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() 
        {
            Debug.Assert(typeof(T).IsInterface, "Le type passé dans GetService doit être une interface");
            return GetService<T>(typeof(T));
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public T GetService<T>(Type type)
        {
            Debug.Assert(_services != null, "Services not initialized");
            object obj;
            if (_services.TryGetValue(type, out obj))
                return (T)obj;
            return (T)GetService(type);
        }

        #region IServiceProvider Members

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type serviceType.-or- null if there is no service object of type serviceType.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (_serviceProvider == null)
                return null;
            return _serviceProvider.GetService(serviceType);
        }

        #endregion
    }
}
