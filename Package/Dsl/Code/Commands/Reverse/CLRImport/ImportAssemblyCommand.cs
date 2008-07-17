using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes pour créer un composant binaire
    /// </summary>
    public class ImportAssemblyCommand : ICommand
    {
        private readonly NodeShape _shape;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportAssemblyCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public ImportAssemblyCommand(object shape)
        {
            _shape = shape as NodeShape;
        }

        #region ICommand Members

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
            CandleModel system = _shape.ModelElement as CandleModel;

            // Si la commande a été lancé sur le composant, on référence le modèle
            if (system == null)
            {
                BinaryComponent component = _shape.ModelElement as BinaryComponent;
                if (component != null)
                    system = component.Model;
            }

            // Peut etre à null si le composant n'est pas de type Binary
            if (system != null)
            {
                // Affiche la fenetre de dialogue permettant de choisir les assemblies
                IAssemblySelectorDialog selector = ServiceLocator.Instance.GetService<IAssemblySelectorDialog>();
                if (selector.ShowDialog(0))
                {
                    ImportAssemblies(system, selector.SelectedAssemblies);

                    // On arrange les shapes du composant
                    IList<PresentationElement> componentShapes = PresentationViewsSubject.GetPresentation(system.Component);
                    ArrangeShapesCommand cmd = new ArrangeShapesCommand((NodeShape) componentShapes[0]);
                    cmd.Exec();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            if (_shape == null)
                return false;

            if (_shape.ModelElement is CandleModel)
            {
                if (((CandleModel) _shape.ModelElement).Component == null)
                    return true; // Modéle vide
                return ((CandleModel) _shape.ModelElement).Component is BinaryComponent;
            }
            return _shape.ModelElement is BinaryComponent;
        }

        #endregion

        /// <summary>
        /// Importe des assemblies dans le modèle
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="assemblies">Liste des assemblies à importer</param>
        internal static void ImportAssemblies(CandleModel model, List<Assembly> assemblies)
        {
            if (assemblies.Count == 0)
                return;

            // Si il y a plusieurs assemblies, on va demander lequel est le public
            Assembly mainAssembly = assemblies[0];
            if (assemblies.Count > 1)
            {
                if (assemblies.Count > 1)
                {
                    SelectAssemblyForm form = new SelectAssemblyForm(assemblies);
                    if (form.ShowDialog() == DialogResult.Cancel)
                        throw new CanceledByUser();
                    mainAssembly = form.SelectedAssembly;
                }
            }

            // D'abord le composant
            BinaryComponent component = model.Component as BinaryComponent;
            if (component == null)
            {
                component = CreateComponent(model, mainAssembly);
            }
            else if (component.Assemblies.Count > 0)
            {
                // Si il y avait dèjà des assemblies, on n'initialisera pas l'assembly public
                mainAssembly = null;
            }

            // Puis insertion des assemblies, en deux passages car il peut y avoir des dépendances entre assemblies
            //  - Insertion des assemblies
            //  - Création des dépendances
            //
            using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Adding component"))
            {
                // Insertion des assemblies internes
                foreach (Assembly assembly in assemblies)
                {
                    if (component.FindDotNetAssemblyModelFromAssembly(assembly.GetName()) == null)
                    {
                        DotNetAssembly dotnetAssembly = new DotNetAssembly(model.Store);
                        component.Assemblies.Add(dotnetAssembly);
                        dotnetAssembly.InitFromAssembly(assembly, false);

                        // C'est l'assembly public --> Création d'un port public
                        dotnetAssembly.Visibility = assembly == mainAssembly ? Visibility.Public : Visibility.Private;
                    }
                }

                transaction.Commit();
            }

            // Création des dépendances externes
            using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Adding references"))
            {
                model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Add("Import assemblies", null);
                foreach (Assembly assembly in assemblies)
                {
                    DotNetAssembly externalAssembly = component.FindDotNetAssemblyModelFromAssembly(assembly.GetName());
                    Debug.Assert(externalAssembly != null);
                    externalAssembly.InsertDependencies(assembly);
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// Création du composant principal initialisé à partir d'une assembly
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="mainAssembly">The main assembly.</param>
        /// <returns></returns>
        private static BinaryComponent CreateComponent(CandleModel model, Assembly mainAssembly)
        {
            using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Init main component"))
            {
                // Création du composant
                BinaryComponent component = new BinaryComponent(model.Store);

                // Init avec les valeurs d'un assembly
                if (mainAssembly != null)
                {
                    if (string.IsNullOrEmpty(model.Name))
                    {
                        component.Name = component.Namespace = mainAssembly.GetName().Name;
                        int pos = component.Name.IndexOf('.');
                        if (pos > 0)
                            component.Name = component.Name.Substring(0, pos);
                        // Init du system
                        model.Name = component.Name;
                    }
                    else
                        component.Name = model.Name;

                    model.Version = new VersionInfo(mainAssembly.GetName().Version);
                }

                model.Component = component;
                transaction.Commit();
                return component;
            }
        }
    }
}