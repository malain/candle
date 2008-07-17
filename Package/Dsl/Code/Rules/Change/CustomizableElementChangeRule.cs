using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (CandleElement), FireTime = TimeToFire.TopLevelCommit, InitiallyDisabled = false, Priority=1)]
    public class CustomizableElementChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            // Teste si on est en train de charger le modèle
            // Permet d'eviter de déclencher une régle lors du chargement du modèle 
            if (e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                e.ModelElement.Store.InUndoRedoOrRollback)
                return;
            object value;

            // On ignore
            if (
                e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.
                    TryGetValue("CustomizableElementChangeRule_Enabled", out value) &&
                (bool) value == false)
                return;

            CandleElement elem = e.ModelElement as CandleElement;
            if (elem == null)
                return;

            // Teste l'élément
            //if (e.DomainProperty.Id == ClassImplementation.NameDomainPropertyId)
            //{
            //    if (String.IsNullOrEmpty(elem.RootName) || elem.RootName == (string)e.OldValue)
            //        elem.RootName = elem.Name;
            //}

            if (e.DomainProperty.Id == CandleElement.RootNameDomainPropertyId)
            {
                string oldValue = (string) e.OldValue;
                ClassImplementation model = e.ModelElement as ClassImplementation;
                if (model != null)
                {
                    // NON si on force le root name, c'est qu'on veut changer le nom
                    //string oldName = String.IsNullOrEmpty(oldValue) ? null : StrategyManager.GetInstance(model.Store).NamingStrategy.CreateElementName(model.Layer, oldValue);
                    //if( String.IsNullOrEmpty(elem.Name) || elem.Name == oldName || String.IsNullOrEmpty(oldValue))
                    elem.Name =
                        StrategyManager.GetInstance(model.Store).NamingStrategy.CreateElementName(model.Layer,
                                                                                                  (string) e.NewValue);

                    if (model.Contract != null)
                        model.Contract.RootName = model.RootName;
                }

                ServiceContract contract = e.ModelElement as ServiceContract;
                if (contract != null && contract.Layer != null && contract.Name == "?")
                    // Layer est null si on a à faire à un ExternalServiceContract
                {
                    //string oldName = StrategyManager.GetInstance(clazz.Store).NamingStrategy.CreateElementName(contract.Layer, (string)e.OldValue);
                    //if (String.IsNullOrEmpty(elem.Name) || elem.Name == oldName)
                    elem.Name =
                        StrategyManager.GetInstance(contract.Store).NamingStrategy.CreateElementName(contract.Layer,
                                                                                                     (string) e.NewValue);
                }

                Entity entity = e.ModelElement as Entity;
                if (entity != null)
                {
                    DataLayer modelsLayer = entity.Package.Layer;
                    string oldName = String.IsNullOrEmpty(oldValue)
                                         ? null
                                         : StrategyManager.GetInstance(model.Store).NamingStrategy.CreateElementName(
                                               model.Layer, oldValue);
                    if (String.IsNullOrEmpty(elem.Name) || elem.Name == oldName || String.IsNullOrEmpty(oldValue))
                        elem.Name =
                            StrategyManager.GetInstance(elem.Store).NamingStrategy.CreateElementName(modelsLayer,
                                                                                                     (string) e.NewValue);
                }
            }
        }
    }
}