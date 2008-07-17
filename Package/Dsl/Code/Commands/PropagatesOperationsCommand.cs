using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Propagation des opérations entre les roles d'une liaison de type <see cref="ClassUsesOperations"/> ou <see cref="Implementation"/>
    /// </summary>
    public class PropagatesOperationsCommand : ICommand
    {
        private readonly List<TypeWithOperations> _sources = new List<TypeWithOperations>();
        private readonly List<TypeWithOperations> _targets = new List<TypeWithOperations>();

        /// <summary>
        /// Constructeur prenant le shape courant
        /// </summary>
        /// <param name="obj">The obj.</param>
        public PropagatesOperationsCommand(object obj)
        {
            LinkShape shape = obj as LinkShape;
            if (shape != null && shape.ModelElement is ClassUsesOperations)
            {
                ClassImplementation clazz = ((ClassUsesOperations)shape.ModelElement).Source as ClassImplementation;
                if (clazz == null)
                    return;

                _sources.Add(clazz);
                if (clazz.Contract != null)
                    _sources.Add(clazz.Contract);

                TypeWithOperations target = (TypeWithOperations) ((ClassUsesOperations)shape.ModelElement).TargetService;
                if (target == null)
                {
                    Debug.Assert(((ClassUsesOperations)shape.ModelElement).ExternalTargetService != null);
                    _targets.Add(((ClassUsesOperations)shape.ModelElement).ExternalTargetService.ReferencedServiceContract);
                }
                else
                    _targets.Add(target);
            }

            if (shape != null && shape.ModelElement is Implementation)
            {
                _sources.Add(((Implementation)shape.ModelElement).Contract);
                ClassImplementation clazz = ((Implementation)shape.ModelElement).ClassImplementation;
                _targets.Add(clazz);
                if (clazz.Contract != null)
                    _targets.Add(clazz.Contract);
            }
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
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            PropagatesOperationsDialog dlg = new PropagatesOperationsDialog(_targets, _sources);
            if( dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
                return;

            // On récupère les roles 
            TypeWithOperations source = dlg.Source;
            TypeWithOperations target = dlg.Target;

            using (Transaction transaction = target.Store.TransactionManager.BeginTransaction("Model affectation"))
            {
                // Suppression avant copie
                if (dlg.ClearTargetOperations)
                {
                    using (Transaction clearTransaction = target.Store.TransactionManager.BeginTransaction("Clear operations"))
                    {
                        target.Operations.Clear();
                        clearTransaction.Commit();
                    }
                }

                // Copie
                foreach (Operation op in dlg.SelectedOperations)
                {
                    target.Operations.Add(Operation.CopyOperation(target.Store, op));
                }
                transaction.Commit();                
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _targets.Count > 0 && _sources.Count > 0;
        }       
    }
}
