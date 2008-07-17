using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class AssociationAsAttributeCommand : ICommand
    {
        private readonly Association _association;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociationAsAttributeCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="element">The element.</param>
        public AssociationAsAttributeCommand( IServiceProvider serviceProvider, object element )
        {
            AssociationLink connector = element as AssociationLink;
            if (connector == null)
                return;
            this._association = connector.ModelElement as Association;
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
            return _association != null && !String.IsNullOrEmpty( _association.SourceRoleName );
        }

        #region ICommand Members
        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            using (Transaction transaction = _association.Store.TransactionManager.BeginTransaction("Property to Association"))
            {
                Entity sourceModel = _association.Source;

                // Création de la propriété
                Property property = new Property(sourceModel.Store);
                property.Name = _association.SourceRoleName;
                if( _association.SourceMultiplicity == Multiplicity.OneMany || _association.SourceMultiplicity == Multiplicity.ZeroMany )
                {
                    property.Type = "List<" + _association.Target.Type + ">";
                }
                else
                {
                    property.Type = _association.Target.Name;
                }
                property.XmlName = _association.XmlName;
                sourceModel.Properties.Add(property);

                if( !String.IsNullOrEmpty( _association.TargetRoleName ) )
                {
                    Entity targetModel = _association.Target;

                    // Création de la propriété
                    property = new Property( targetModel.Store );
                    property.Name = _association.TargetRoleName;
                    property.Type = _association.Target.Name;
                    property.XmlName = _association.Target.Name;
                    targetModel.Properties.Add( property );
                }

                // Suppression de l'association
                _association.Delete();

                // TIPS sélection d'un composant
                // On enlève la sélection sur l'association car on vient de la supprimer
                IMonitorSelectionService monitorSelectionService = (IMonitorSelectionService)_serviceProvider.GetService( typeof( IMonitorSelectionService ) );
                if( monitorSelectionService != null )
                {
                    ISelectionService selectionService = monitorSelectionService.CurrentSelectionContainer as ISelectionService;
                    if (selectionService != null) selectionService.SetSelectedComponents( null );
                }
                transaction.Commit();
            } 
        }
        #endregion
    }
}
