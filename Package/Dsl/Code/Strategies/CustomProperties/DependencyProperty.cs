using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Description d'un propriété personnalisée
    /// </summary>
    /// <typeparam name="T">Type de la propriété</typeparam>
    public class DependencyProperty<T> : IDependencyProperty
    {
        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        public delegate T CreateDefaultValueInstance();

        #endregion

        private const string PropertyKeyFormat = "[{0}].{1}";

        /// <summary>
        /// ID for property shared by all strategies
        /// </summary>
        public const string SHARED_STRATEGY = "_SharedStrategy";

        private readonly List<Attribute> _attributes = new List<Attribute>();
        private readonly string _name;
        private readonly String _strategyId;
        private DefaultValueAttribute _defaultValue;
        private CreateDefaultValueInstance _defaultValueDelegate = null;
        private Type typeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="name">The name.</param>
        /// <param name="attributes">The attributes.</param>
        public DependencyProperty(String strategyId, string name, Attribute[] attributes)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid property name");

            if (String.IsNullOrEmpty(strategyId))
                throw new ArgumentException("Invalid strategy id");

            _strategyId = strategyId;
            _name = name;

            if (attributes != null)
                _attributes = new List<Attribute>(attributes);

            DependencyPropertyRegistry.Instance.Register(strategyId, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="name">The name.</param>
        public DependencyProperty(String strategyId, string name)
            : this(strategyId, name, null)
        {
        }

        /// <summary>
        /// Create a shared property
        /// </summary>
        /// <param name="name">The name.</param>
        public DependencyProperty(string name)
            : this(SHARED_STRATEGY, name, null)
        {
        }

        /// <summary>
        /// Create a shared property
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="attributes">The attributes.</param>
        public DependencyProperty(string name, Attribute[] attributes)
            : this(SHARED_STRATEGY, name, attributes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty&lt;T&gt;"/> class.
        /// </summary>
        public DependencyProperty()
        {
            _strategyId = String.Empty;
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public List<Attribute> Attributes
        {
            [DebuggerStepThrough()]
            get { return _attributes; }
        }

        // A remettre quand on pourra supprimer une propriété (voir SetValue)
        //public ShouldSerializeValue ShouldSerializeValueHandler
        //{
        //    [System.Diagnostics.DebuggerStepThrough()]
        //    set { _shouldSerializeValueDelegate = value; }
        //}

        /// <summary>
        /// Sets the default value creator handler.
        /// </summary>
        /// <value>The default value creator handler.</value>
        public CreateDefaultValueInstance DefaultValueCreatorHandler
        {
            [DebuggerStepThrough()]
            set { _defaultValueDelegate = value; }
        }

        /// <summary>
        /// Sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                CategoryAttribute att = new CategoryAttribute(value);
                _attributes.Add(att);
            }
        }

        /// <summary>
        /// Sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                DisplayNameAttribute att = new DisplayNameAttribute(value);
                _attributes.Add(att);
            }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public T DefaultValue
        {
            set
            {
                _defaultValue = new DefaultValueAttribute(value);
                _attributes.Add(_defaultValue);
            }
            get
            {
                if (HasDefaultValue)
                    return (T) GetDefaultValue();
                return default(T);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has default value.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has default value; otherwise, <c>false</c>.
        /// </value>
        public bool HasDefaultValue
        {
            get { return _defaultValue != null || _defaultValueDelegate != null; }
        }

        /// <summary>
        /// Sets the type editor.
        /// </summary>
        /// <value>The type editor.</value>
        public Type TypeEditor
        {
            set
            {
                if (value == null || !typeof (UITypeEditor).IsAssignableFrom(value))
                    throw new ArgumentException("Must derive from UITypeEditor");

                EditorAttribute att = new EditorAttribute(value, typeof (UITypeEditor));
                _attributes.Add(att);
            }
        }

        #region IDependencyProperty Members

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        public Type PropertyType
        {
            get { return typeof (T); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            //  set { _name = value; }
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns></returns>
        public object GetDefaultValue()
        {
            if (HasDefaultValue)
            {
                if (_defaultValueDelegate != null)
                    return _defaultValueDelegate();

                return _defaultValue.Value;
            }
            return null;
        }

        /// <summary>
        /// Gets or sets the type converter.
        /// </summary>
        /// <value>The type converter.</value>
        public Type TypeConverter
        {
            set
            {
                if (value == null)
                    throw new ArgumentException("Incorrect TypeConverter");

                typeConverter = value;
                _attributes.Add(new TypeConverterAttribute(value));
            }
            get { return typeConverter; }
        }

        #endregion

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public DependencyPropertyDescriptor<T> Register(ModelElement model)
        {
            if (model == null)
                throw new ArgumentNullException("Model");
            if (!(model is ICustomizableElement))
                throw new ArgumentException("Must implements ICustomizableElement", "model");
            if (_strategyId == String.Empty)
                throw new ArgumentNullException("Strategy");
            if (String.IsNullOrEmpty(_name))
                throw new ArgumentNullException("Name");

            Attribute[] attributes = new Attribute[_attributes.Count];
            _attributes.CopyTo(attributes);
            return new DependencyPropertyDescriptor<T>((ICustomizableElement) model, this);
        }

        /// <summary>
        /// Détermine si cette propriété a été initialisée
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// 	<c>true</c> if the specified element has value; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValue(ICustomizableElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            DependencyProperty property = element.GetStrategyCustomProperty(_strategyId, _name, false);
            return property != null && property.Value != null;
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryGetValue(ICustomizableElement element, string strategyId, string propertyName, out T value)
        {
            IDependencyProperty p = DependencyPropertyRegistry.Instance.FindDependencyProperty(strategyId, propertyName);
            if (p != null)
                return TryGetValue(p, element, strategyId, propertyName, out value);
            throw new Exception(String.Concat("Invalid property name ", propertyName, " for strategy ", strategyId));
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="element">The element.</param>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected static bool TryGetValue(IDependencyProperty self, ICustomizableElement element, string strategyId,
                                          string propertyName, out T value)
        {
            // Recherche si cette valeur est dans le modèle
            DependencyProperty property = element.GetStrategyCustomProperty(strategyId, propertyName, false);
            if (property != null)
            {
                if (property.Value != null) // Pas initialisée
                {
                    value = property.Value.GetValue<T>(self);
                    //    if (SerializationUtilities.TryGetValue<T>(SerializationUtilities.UnescapeXmlString(property.Value), out value))
                    if (value != null)
                        return true;
                }
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// Récupére la valeur de la propriété
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public virtual T GetValue(ICustomizableElement instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            T value;
            if (TryGetValue(this, instance, _strategyId, _name, out value))
                return value;

            // Valeur par défaut
            if (HasDefaultValue)
                value = (T) DefaultValue;

            //SetValue(instance, value);

            return value;
        }

        /// <summary>
        /// Mise à jour d'un paramètre personnalisé
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public virtual void SetValue(ICustomizableElement instance, T value)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            bool isDefaultValue = false;
                // HasDefaultValue && DefaultValue.Equals(value);// (Voir ShouldSerializeValue property) || (_shouldSerializeValueDelegate != null && !_shouldSerializeValueDelegate(value));
            DependencyProperty property = instance.GetStrategyCustomProperty(_strategyId, _name, !isDefaultValue);
            if (property != null && value != null)
            {
                Transaction transaction =
                    ((ModelElement) instance).Store.TransactionManager.BeginTransaction("Set custom value");
                try
                {
                    //property.Value = SerializationUtilities.GetString<T>(value);
                    DependencyPropertyValue dpv = new DependencyPropertyValue();
                    dpv.SetValue(this, value);
                    property.Value = dpv;

                    transaction.Commit();
                }
                catch (ArgumentOutOfRangeException exception)
                {
                    if (transaction.IsActive)
                    {
                        transaction.Rollback();
                    }
                    throw;
                }
                catch (Exception)
                {
                    if (transaction.IsActive)
                    {
                        transaction.Rollback();
                    }
                    throw;
                }
            }
        }
    }
}