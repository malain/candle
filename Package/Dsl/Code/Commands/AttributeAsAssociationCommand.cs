using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class AttributeAsAssociationCommand : ICommand
    {
        private readonly Entity _clazz;
        private readonly Property _property;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeAsAssociationCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="element">The element.</param>
        public AttributeAsAssociationCommand(IServiceProvider serviceProvider, object element)
        {
            Property property = element as Property;
            if (property == null)
                return;
            this._clazz = property.Parent as Entity;            
            this._property = property;
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible(); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            if (_property == null)
                return false;
            return FindModelClassByName(_property) != null;
        }

        #region ICommand Members
        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            using (Transaction transaction = _clazz.Store.TransactionManager.BeginTransaction("Property to Association"))
            {
                Entity targetModel = FindModelClassByName( _property );
                if (targetModel == null)
                    return;

                // Suppression en tant que propriété
                _clazz.Properties.Remove(_property);

                // Propriétés de l'association
                Association assoc = _clazz.AddAssociationTo( targetModel );
                assoc.SourceRoleName = _property.Name;
                assoc.SourceMultiplicity = Multiplicity.ZeroOne;
                assoc.XmlName = _property.XmlName;

                // TIPS sélection d'un composant
                // On enlève la sélection sur la propriété car on vient de la supprimer
                IMonitorSelectionService monitorSelectionService = (IMonitorSelectionService)_serviceProvider.GetService(typeof(IMonitorSelectionService));
                if( monitorSelectionService != null )
                {
                    ISelectionService selectionService = monitorSelectionService.CurrentSelectionContainer as ISelectionService;
                    if (selectionService != null) selectionService.SetSelectedComponents( null );
                }
                transaction.Commit();
            } 
        }
        #endregion

        /// <summary>
        /// Finds the name of the model class by.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private Entity FindModelClassByName(Property property)
        {
            DataType model = _clazz.DataLayer.FindType( property.Type );
            return model as Entity;
        }
    }
}
