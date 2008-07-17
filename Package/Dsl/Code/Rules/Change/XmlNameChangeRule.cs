using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (NamedElement), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class XmlNameChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            #region Condition

            if (e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                e.ModelElement.Store.InUndoRedoOrRollback)
                return;

            #endregion

            #region Traitement

            if (e.DomainProperty.Id == NamedElement.NameDomainPropertyId)
            {
                string name = ((NamedElement) e.ModelElement).Name;
                if (string.IsNullOrEmpty(name))
                    return;

                if (e.ModelElement is DataType)
                {
                    DataType type = (DataType) e.ModelElement;
                    ClassNameInfo cni = new ClassNameInfo(type.Package.Name, (string) e.OldValue);
                    string oldName = cni.FullName;
                    type.XmlName = name;

                    // Mise à jour de tous les types dans les opérations
                    foreach (SoftwareLayer layer in type.Package.Layer.Component.Layers)
                    {
                        if (layer is InterfaceLayer)
                        {
                            foreach (ServiceContract port in ((InterfaceLayer) layer).ServiceContracts)
                            {
                                foreach (Operation op in port.Operations)
                                {
                                    if (op.Type == oldName)
                                        op.Type = name;
                                    foreach (Argument arg in op.Arguments)
                                    {
                                        if (arg.Type == oldName)
                                            arg.Type = name;
                                    }
                                }
                            }
                        }
                    }
                }

                //if( e.ModelElement is DataLayer )
                //{
                //    ( (DataLayer)e.ModelElement ).XmlNamespace = name;
                //}

                if (e.ModelElement is Property)
                {
                    ((Property) e.ModelElement).XmlName = name;
                }
            }

            #endregion
        }
    }
}