using System.Collections;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    class CopyCommand : ICommand
    {
        private readonly Store _store;
        private readonly ICollection _elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyCommand"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="objects">The objects.</param>
        public CopyCommand(Store store, ICollection objects)
        {
            this._store = store;
            this._elements = objects;
        }

        #region ICommand Members

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            if (_store == null)
                return false;

            foreach (object o in _elements)
            {
                // Pick out shapes representing Component model elements.
                ShapeElement element = o as ShapeElement;
                if (element != null && element.ModelElement != null || o is ModelElement)
                {
                    ModelElement mel = element != null ? element.ModelElement : o as ModelElement;
                    if (mel is Entity
                          || mel is Operation
                          || mel is Enumeration
                          || mel is ClassImplementation
                          || mel is ServiceContract
                          || mel is Property)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            ElementGroup elementGroup = new ElementGroup(_store);
            bool foundSome = false;
            foreach (object o in _elements)
            {
                // Pick out shapes representing Component model elements.
                ShapeElement element = o as ShapeElement;
                if (element != null && element.ModelElement != null || o is ModelElement)
                {
                    ModelElement mel = element != null ? element.ModelElement : o as ModelElement;
                    if (mel is Entity
                          || mel is Operation
                          || mel is Enumeration
                          || mel is ClassImplementation
                          || mel is ServiceContract
                          || mel is Property)
                    {
                        // add the element and its embedded children to the group
                        elementGroup.AddGraph(mel, true);
                        foundSome = true;
                    }
                }
            }
            if (!foundSome) return;

            // A DataObject carries a serialized version.
            System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject();
            data.SetData(elementGroup.CreatePrototype());
            System.Windows.Forms.Clipboard.SetDataObject
                  (data,   // serialized clones of our selected model elements 
                   false,  // we don’t want to export outside this application
                   10,     // retry 10 times on failure
                   50);    // waiting 50ms between retries
        }

        #endregion
    }
}
