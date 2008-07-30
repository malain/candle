using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms.Design;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    [Strategy(EntLibPolicyInjectionStrategy.StrategyID)]
    public class EntLibPolicyInjectionStrategy : StrategyBase, IStrategyCodeInjector //, IStrategyAddElementInterceptor, IStrategyValidator
    {
        private const string StrategyID = "fd45eb79-59a9-42f9-b1cf-8143480a83da"; // or an unique name
        private const string displayName = "EnterpriseLibrary";
        private readonly Guid entLibId = new Guid("82bb6795-91c9-4fbd-b58e-f91c35c8ced3");

        private string _appName;

        private string _performanceCategoryName;

        [Description("Name of the category name for the performance counter")]
        public string PerformanceCategoryName
        {
            get { return _performanceCategoryName; }
            set { _performanceCategoryName = value; }
        }
	
        #region Dependency properties
        /// <summary>
        /// Dependency property
        /// </summary>
        public static readonly DependencyProperty<LogCallHandler> LogCallHandlerProperty;
        public static readonly DependencyProperty<CacheCallHandler> CacheCallHandlerProperty;
        public static readonly DependencyProperty<PerformanceCounterCallHandler> PerformanceCounterCallHandlerProperty;

        static EntLibPolicyInjectionStrategy()
        {
            LogCallHandlerProperty = new DependencyProperty<LogCallHandler>(StrategyID, "Logging");
            LogCallHandlerProperty.Category = displayName;
            LogCallHandlerProperty.DefaultValueCreatorHandler = new DependencyProperty<LogCallHandler>.CreateDefaultValueInstance(delegate() { return new LogCallHandler(); });
            LogCallHandlerProperty.TypeConverter = typeof(ExpandableElementConverter<LogCallHandler>);

            CacheCallHandlerProperty = new DependencyProperty<CacheCallHandler>(StrategyID, "Cache");
            CacheCallHandlerProperty.Category = displayName;
            CacheCallHandlerProperty.TypeConverter = typeof(ExpandableElementConverter<CacheCallHandler>);
            CacheCallHandlerProperty.DefaultValueCreatorHandler = new DependencyProperty<CacheCallHandler>.CreateDefaultValueInstance(delegate() { return new CacheCallHandler(); });

            PerformanceCounterCallHandlerProperty = new DependencyProperty<PerformanceCounterCallHandler>(StrategyID, "Instrumentation");
            PerformanceCounterCallHandlerProperty.Category = displayName;
            PerformanceCounterCallHandlerProperty.TypeConverter = typeof(ExpandableElementConverter<PerformanceCounterCallHandler>);
        }

        public EntLibPolicyInjectionStrategy()
        {
            DisplayName = displayName;
            PerformanceCounterCallHandlerProperty.DefaultValueCreatorHandler = new DependencyProperty<PerformanceCounterCallHandler>.CreateDefaultValueInstance(CreatePerformanceCounterDefaultValue);
        }

        private PerformanceCounterCallHandler CreatePerformanceCounterDefaultValue()
        {
            PerformanceCounterCallHandler h = new PerformanceCounterCallHandler();
            // _appName est initialisé avec le nom de l'appli lors de l'appel de GetCustomProperties
            h.InstanceName = _appName;
            h.CategoryName = _performanceCategoryName;
            return h;
        }

        /// <summary>
        /// Contextual dependency properties filter
        /// </summary>
        /// <param name="modelElement"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetCustomProperties(ModelElement modelElement)
        {
            PropertyDescriptorCollection collections = base.GetCustomProperties(modelElement);

            _appName = CandleModel.GetInstance(modelElement.Store).Name;

            if (modelElement is ServiceContract || modelElement is ClassImplementation ||
                modelElement is Operation || modelElement is Layer)
            {
                collections.Add(LogCallHandlerProperty.Register(modelElement));
                collections.Add(CacheCallHandlerProperty.Register(modelElement));
                collections.Add(PerformanceCounterCallHandlerProperty.Register(modelElement));
            }

            return collections;
        }
        #endregion

        #region IStrategyCodeInjector Members

        void IStrategyCodeInjector.OnGenerateClass(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeClass clazz)
        {
        }

        void IStrategyCodeInjector.OnGenerateEnum(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeEnum enumeration)
        {
        }

        T CheckHandlerEnabled<T>( DependencyProperty<T> property, ICustomizableElement element) where T : IPIABHandler
        {
            T handler = property.GetValue(element);
            if (handler != null && handler.Enabled)
                return handler;
            return default(T);
        }

        T CheckHandler<T>(DependencyProperty<T> property, ICustomizableElement element, ICustomizableElement parent) where T:IPIABHandler
        {
            T handler;
            if (element is Operation)
            {
                handler = CheckHandlerEnabled<T>(property, element);
                if (handler != null)
                    return handler;
                element = parent;
            }

            if (element is ClassImplementation)
            {
                handler = CheckHandlerEnabled<T>(property, element);
                if (handler != null)
                    return handler;
                handler = CheckHandlerEnabled<T>(property, parent);
                if (handler != null)
                    return handler;
                element = ((ClassImplementation)element).Contract;
            }

            if (element is ServiceContract)
            {
                handler = CheckHandlerEnabled<T>(property, element);
                if (handler != null)
                    return handler;
                return CheckHandlerEnabled<T>(property, parent);
            }

            return default(T);
        }

        void IStrategyCodeInjector.OnGenerateFunction(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        {
            // Service factory
            if (context.CurrentElement is Layer)
            {
                // On part du principe que le template du servicelocator à généré une classe nommée ServiceLocator et 
                // une méthode par création d'instance dont le nom commence par CreateInstanceOfxxxxxx
                if (function.Parent.Name == "ServiceLocatorBase" && function.Name.StartsWith("CreateInstanceOf"))
                {
                        string currentName = function.Name.Substring("CreateInstanceOf".Length);
                        foreach (ClassImplementation clazz in ((Layer)context.CurrentElement).Classes)
                        {
                            foreach (ClassUsesOperations service in ClassUsesOperations.GetLinksToServicesUsed(clazz))
                            {
                                if (!context.GenerationContext.Mode.CheckConfigurationMode(service.ConfigurationMode) || service.TargetService is ExternalServiceContract)
                                    continue;

                                ServiceContract contract = service.TargetService as ServiceContract;
                                if (contract == null)
                                    continue;

                                foreach (Implementation impl in Implementation.GetLinksToImplementations(contract))
                                {
                                    if (context.GenerationContext.Mode.CheckConfigurationMode(impl.ConfigurationMode) && impl.ClassImplementation.Name == currentName)
                                    {
                                        if (impl.ClassImplementation.Contract != null)
                                        {
                                            foreach (Operation op in impl.ClassImplementation.Contract.Operations)
                                            {
                                                if (CheckAllHandler(op, impl.ClassImplementation))
                                                {
                                                    function.ReplaceBody(
        @"           string path = ConfigurationManager.AppSettings[key];
            Type t = Assembly.Load(path).GetType(""" + impl.ClassImplementation.FullName + @""");
            PropertyInfo p = typeof(Microsoft.Practices.EnterpriseLibrary.PolicyInjection.PolicyInjection).GetProperty(""DefaultPolicyInjector"", BindingFlags.Static | BindingFlags.NonPublic);
            Microsoft.Practices.EnterpriseLibrary.PolicyInjection.PolicyInjector pi = (Microsoft.Practices.EnterpriseLibrary.PolicyInjection.PolicyInjector)p.GetValue(null, null);
            return (" + contract.FullName + ")pi.Create(t, typeof(" + contract.FullName + @"));
");
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
            }

            // Attribut 
            if (context.CurrentElement is ClassImplementation)
            {
                ClassImplementation clazz = context.CurrentElement as ClassImplementation;
                if (clazz.Name+"Base" == function.Parent.Name || clazz.Name == function.Parent.Name)
                {
                    Operation op = function.FindOperationFromContract(clazz.Contract);
                    if (op != null)
                    {
                        // LogCallHandler
                        IPIABHandler handler = CheckHandler<LogCallHandler>(LogCallHandlerProperty, op, clazz);
                        if (handler != null && handler.Enabled)
                        {
                            handler.SetAttribute(context, function);
                        }
                    

                        // CacheCallHandler
                        handler = CheckHandler<CacheCallHandler>(CacheCallHandlerProperty, op, clazz);
                        if (handler != null && handler.Enabled)
                        {
                            handler.SetAttribute(context, function);
                        }

                        // PerformanceCounterCallHandler
                        handler = CheckHandler<PerformanceCounterCallHandler>(PerformanceCounterCallHandlerProperty, op, clazz);
                        if (handler != null && handler.Enabled)
                        {
                            handler.SetAttribute(context, function);
                        }
                    }
                }
            }
        }


        void IStrategyCodeInjector.OnGenerateInterface(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeInterface interf)
        {
        }

        void IStrategyCodeInjector.OnGenerateParameter(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeParameter parm)
        {
        }

        void IStrategyCodeInjector.OnGenerateProperty(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeProperty prop)
        {
        }

        void IStrategyCodeInjector.OnGenerateStruct(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeStruct structure)
        {
        }

        string[] IStrategyCodeInjector.OnGenerateUsing(CodeInjectionContext context)
        {
            return null;
        }

        void IStrategyCodeInjector.OnGenerateVariable(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeVariable variable)
        {
        }

        void IStrategyCodeInjector.OnMetaModelUpdate(CodeInjectionContext context)
        {
            // Reference pour le ServiceLocator
            if (context.CurrentElement is Layer)
            {
                Layer layer = context.CurrentElement as Layer;
                foreach (ClassImplementation clazz in layer.Classes)
                {
                    foreach (ClassUsesOperations service in ClassUsesOperations.GetLinksToServicesUsed(clazz))
                    {
                        if (!context.GenerationContext.Mode.CheckConfigurationMode(service.ConfigurationMode) || service.TargetService is ExternalServiceContract)
                            continue;

                        foreach (Implementation impl in Implementation.GetLinksToImplementations((ServiceContract)service.TargetService))
                        {
                            if (context.GenerationContext.Mode.CheckConfigurationMode(impl.ConfigurationMode))
                            {
                                if (CheckAllHandler(impl.ClassImplementation, impl.ClassImplementation.Layer))
                                {
                                    layer.AddReferenceToService(entLibId, "EnterpriseLibrary", new VersionInfo(3, 1, 0, 0), "Microsoft.Practices.EnterpriseLibrary.PolicyInjection");
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (context.CurrentElement is ClassImplementation)
            {
                ClassImplementation clazz = context.CurrentElement as ClassImplementation;
                if (clazz.Contract == null)
                    return;

                bool flag = false;
                foreach (Operation op in clazz.Contract.Operations)
                {
                    IPIABHandler handler = CheckHandler<LogCallHandler>(LogCallHandlerProperty, op, clazz);
                    if (handler != null && handler.Enabled)
                    {
                        ((ClassImplementation)context.CurrentElement).Layer.AddReferenceToService(entLibId, "EnterpriseLibrary", new VersionInfo(3, 1, 0, 0), "Microsoft.Practices.EnterpriseLibrary.Logging", ReferenceScope.Runtime, "*");
                        flag = true;
                    }

                    handler = CheckHandler<CacheCallHandler>(CacheCallHandlerProperty, op, clazz);
                    if (handler != null && handler.Enabled)
                    {
                        ((ClassImplementation)context.CurrentElement).Layer.AddReferenceToService(entLibId, "EnterpriseLibrary", new VersionInfo(3, 1, 0, 0), "Microsoft.Practices.EnterpriseLibrary.Caching", ReferenceScope.Runtime, "*");
                        flag = true;
                    }

                    // PerformanceCounterCallHandler
                    handler = CheckHandler<PerformanceCounterCallHandler>(PerformanceCounterCallHandlerProperty, op, clazz);
                    if (handler != null && handler.Enabled)
                    {
                        ((ClassImplementation)context.CurrentElement).Layer.AddReferenceToService(entLibId, "EnterpriseLibrary", new VersionInfo(3, 1, 0, 0), "Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation", ReferenceScope.Runtime, "*");
                        flag = true;
                    }
                }

                if (flag)
                {
                    ((ClassImplementation)context.CurrentElement).Layer.AddReferenceToService(entLibId, "EnterpriseLibrary", new VersionInfo(3, 1, 0, 0), "Microsoft.Practices.EnterpriseLibrary.PolicyInjection");
                    ((ClassImplementation)context.CurrentElement).Layer.AddReferenceToService(entLibId, "EnterpriseLibrary", new VersionInfo(3, 1, 0, 0), "Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers", ReferenceScope.Runtime | ReferenceScope.Compilation, "*");
                }
            }
        }

        private bool CheckAllHandler(ICustomizableElement element, ICustomizableElement parent)
        {
            if (CheckHandler<LogCallHandler>(LogCallHandlerProperty, element, parent) != null)
                return true;
            if (CheckHandler<CacheCallHandler>(CacheCallHandlerProperty, element, parent) != null)
                return true;
            if (CheckHandler<PerformanceCounterCallHandler>(PerformanceCounterCallHandlerProperty, element, parent) != null)
                return true;

            return false;
        }

        #endregion

    }
}
