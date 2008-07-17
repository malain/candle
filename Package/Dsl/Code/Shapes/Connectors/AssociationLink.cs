using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class AssociationLink
    {
        #region decorateur

        /// <summary>
        /// 
        /// </summary>
        public class MultiplicityTextField : TextField
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MultiplicityTextField"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public MultiplicityTextField(string name)
                : base(name)
            {
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <param name="parentShape">The parent shape.</param>
            /// <returns></returns>
            public override object GetValue(ShapeElement parentShape)
            {
                object obj = base.GetValue(parentShape);
                if (obj is Multiplicity)
                {
                    switch ((Multiplicity) obj)
                    {
                        case Multiplicity.One:
                            return "1,1";
                        case Multiplicity.OneMany:
                            return "1,N";
                        case Multiplicity.ZeroMany:
                            return "0,N";
                        case Multiplicity.ZeroOne:
                            return "0,1";
                        case Multiplicity.NotApplicable:
                            return String.Empty;
                    }
                }
                return obj;
            }
        }

        #endregion

        /// <summary>
        /// Called once on each shape or connector class.
        /// Sets up pens, brushes, fonts (the Style Set) and other constants for this class.
        /// </summary>
        /// <param name="classStyleSet"></param>
        protected override void InitializeResources(StyleSet classStyleSet)
        {
            // First the generated behavior.
            base.InitializeResources(classStyleSet);

            // This will cause OnAssociatedPropertyChanged to be called when the user changes the Sort value.
            AssociationLink.AssociateValueWith(Store, Association.SortDomainPropertyId);
            // Non bouclage            UpdateDecorators();
        }

        /// <summary>
        /// Initialize the collection of decorators associated with this shape type.  This method also
        /// creates shape fields for outer decorators, because these are not part of the shape fields collection
        /// associated with the shape, so they must be created here rather than in InitializeShapeFields.
        /// </summary>
        /// <param name="shapeFields"></param>
        /// <param name="decorators"></param>
        protected override void InitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        {
            base.InitializeDecorators(shapeFields, decorators);

            // Suppression des anciens
            Decorator decorator = FindDecorator(decorators, "SourceMultiplicityDecorator");
            decorators.Remove(decorator);
            decorator = FindDecorator(decorators, "TargetMultiplicityDecorator");
            decorators.Remove(decorator);

            // Re création des nouveaux
            MultiplicityTextField field3 = new MultiplicityTextField("SourceMultiplicityDecorator");
            field3.DefaultText =
                CandleDomainModel.SingletonResourceManager.GetString(
                    "AssociationLinkSourceMultiplicityDecoratorDefaultText");
            field3.DefaultFocusable = true;
            field3.DefaultAutoSize = true;
            field3.AnchoringBehavior.MinimumHeightInLines = 1;
            field3.AnchoringBehavior.MinimumWidthInCharacters = 1;
            field3.DefaultAccessibleState = AccessibleStates.Invisible;
            Decorator decorator3 = new ConnectorDecorator(field3, ConnectorDecoratorPosition.SourceBottom, PointD.Empty);
            decorators.Add(decorator3);

            MultiplicityTextField field4 = new MultiplicityTextField("TargetMultiplicityDecorator");
            field4.DefaultText =
                CandleDomainModel.SingletonResourceManager.GetString(
                    "AssociationLinkTargetMultiplicityDecoratorDefaultText");
            field4.DefaultFocusable = true;
            field4.DefaultAutoSize = true;
            field4.AnchoringBehavior.MinimumHeightInLines = 1;
            field4.AnchoringBehavior.MinimumWidthInCharacters = 1;
            field4.DefaultAccessibleState = AccessibleStates.Invisible;
            Decorator decorator4 = new ConnectorDecorator(field4, ConnectorDecoratorPosition.TargetBottom, PointD.Empty);
            decorators.Add(decorator4);
        }

        /// <summary>
        /// Event handler called whenever any associated Domain Property is changed.
        /// Changes can be through user interface or assignment in program code.
        /// </summary>
        /// <param name="e">The property changed event arguments.</param>
        protected override void OnAssociatedPropertyChanged(PropertyChangedEventArgs e)
        {
            // This is called when any associated domain property is changed; so first find out which one.
            if ("Sort".Equals(e.PropertyName))
            {
                UpdateDecorators();
            }

            // Generated behavior - will redraw the shape.
            base.OnAssociatedPropertyChanged(e);
        }


        /// <summary>
        /// Alerts listeners when the mouse is double-clicked over the shape.
        /// </summary>
        /// <param name="e">The diagram point event arguments.</param>
        public override void OnDoubleClick(DiagramPointEventArgs e)
        {
            //    base.OnDoubleClick( e );
            ShowAssociationPropertiesCommand cmd = new ShowAssociationPropertiesCommand(ModelElement);
            cmd.Exec();
        }

        /// <summary>
        /// TIPS permet d'étendre une sélection
        /// Si l'utilisateur appuye sur Alt lors de la sélection, on sélectionne aussi les diffèrentes clés qui
        /// composent l'association
        /// </summary>
        /// <param name="diagramItem">The diagram item.</param>
        /// <param name="view">The view.</param>
        /// <param name="isAddition">if set to <c>true</c> [is addition].</param>
        public override void CoerceSelection(DiagramItem diagramItem, DiagramClientView view, bool isAddition)
        {
            // Ce fonctionnement n'est pas par défaut lors de la sélection car sinon la page de propriétés de l'association 
            // ne s'afficherait jamais (car il y a une sélection multiple)
            if (isAddition && Utils.IsKeyPressed(Keys.Alt))
            {
                // Sélection des clés associées à l'association
                Association association = ModelElement as Association;
                List<Property> foreignKeys = new List<Property>();
                List<Property> primaryKeys = new List<Property>();
                foreach (ForeignKey fk in association.ForeignKeys)
                {
                    foreignKeys.Add(fk.Column);
                    primaryKeys.Add(fk.PrimaryKey);
                }

                SelectProperties(view, association, association.Source, foreignKeys);
                SelectProperties(view, association, association.Target, primaryKeys);
            }
        }

        /// <summary>
        /// Sélection des propriétes dans la compartiment list
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="association">The association.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="properties">The properties.</param>
        private static void SelectProperties(DiagramClientView view, Association association, Entity entity,
                                             IList<Property> properties)
        {
            IList<PresentationElement> presentations = PresentationViewsSubject.GetPresentation(entity);
            Debug.Assert(presentations.Count > 0);
            Compartment compartment = ((CompartmentShape) presentations[0]).FindCompartment("Properties");
            Debug.Assert(compartment != null);
            foreach (ShapeField field in compartment.ShapeFields)
            {
                if (field is ListField)
                {
                    foreach (Property property in properties)
                    {
                        if (property != null)
                        {
                            int index = entity.Properties.IndexOf(property);
                            DiagramItem item = new DiagramItem(compartment, field, new ListItemSubField(index));
                            if (!view.Selection.Contains(item))
                                view.Selection.Add(item);
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the decorators.
        /// </summary>
        private void UpdateDecorators()
        {
            if (ModelElement == null)
                return;

            switch (((Association) ModelElement).Sort)
            {
                case AssociationSort.Aggregation:
                    DecoratorFrom = LinkDecorator.DecoratorEmptyDiamond;
                    break;
                case AssociationSort.Composition:
                    DecoratorFrom = LinkDecorator.DecoratorFilledDiamond;
                    break;
                case AssociationSort.Normal:
                    DecoratorFrom = null;
                    break;
                default:
                    Debug.Fail("Unrecognized value for Association.Sort");
                    break;
            }
        }
    }
}