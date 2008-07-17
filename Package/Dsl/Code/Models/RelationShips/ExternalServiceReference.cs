using System;
using System.Collections.Generic;
using System.Text;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [System.ComponentModel.TypeDescriptionProvider( typeof( StrategyProviderTypeDescriptorProvider ) )]
    partial class ExternalServiceReference : ICustomizableElement
    {
        #region ElementAdded
        // Lors de la création d'un element, on va afficher un wizard que l'utilisateur peut annuler.
        // TIPS permet d'intercepter la création d'un enfant
        /// <summary>
        /// Execution d'un wizard lors de la création d'un élément
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="defaultWizard">The default wizard.</param>
        /// <returns></returns>
        public bool ExecuteWizard(ModelElement element, IStrategyWizard defaultWizard)
        {
            if (!(element is ICustomizableElement))
                return true;

            try
            {
                StrategyElementElementAddedEventArgs e = new StrategyElementElementAddedEventArgs( element );

                foreach( StrategyBase strategy in GetStrategies( false ) )
                {
                    IStrategyAddElementInterceptor sa = strategy as IStrategyAddElementInterceptor;
                    if( sa == null )
                        continue;
                    IStrategyWizard wizard = sa.GetWizard( element );
                    if( wizard != null )
                    {
                        wizard.RunWizard( this, e );
                        if( e.UserCancel )
                            return false;

                        if( e.CancelBubble )
                            break;
                    }
                }

                if( !e.CancelBubble && defaultWizard != null )
                {
                    defaultWizard.RunWizard( this, e );
                    if( e.UserCancel )
                        return false;
                }

                OnElementAdded( e );
                return true;
            }
            catch( Exception ex )
            {
                IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                if (ide != null)
                {
                    ide.ShowMessage(ex.Message);
                } 
                return false;
            }
        }

        /// <summary>
        /// Ajout d'un élément
        /// </summary>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public virtual bool OnElementAdded( StrategyElementElementAddedEventArgs e )
        {
            foreach( StrategyBase strategy in GetStrategies( false ) )
            {
                IStrategyAddElementInterceptor sa = strategy as IStrategyAddElementInterceptor;
                if( sa != null )
                {
                    sa.OnElementAdded( this, e );
                    if( e.UserCancel )
                        return false;
                    if( e.CancelBubble )
                        break;
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public CandleElement StrategiesOwner
        {
            get { return this.Client.StrategiesOwner; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public ICustomizableElement Owner
        {
            get { return this.Client; }
        }

        /// <summary>
        /// Liste des stratégies concernant ce modèle
        /// </summary>
        /// <param name="specific"></param>
        /// <returns></returns>
        public List<StrategyBase> GetStrategies( bool specific )
        {
            return StrategyManager.GetStrategies(StrategiesOwner, specific ? this : null );
        }

        /// <summary>
        /// Recherche la propriété personnalisé et la crée si elle n'existe pas
        /// </summary>
        /// <param name="strategyId">Identifiant de la stratégie</param>
        /// <param name="propertyName">Nom de la propriété</param>
        /// <param name="createIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns></returns>
        public DependencyProperty GetStrategyCustomProperty(string strategyId, string propertyName, bool createIfNotExists)
        {
            foreach( StrategyBase strategy in GetStrategies( false ) )
            {
                if( Utils.StringCompareEquals( strategy.StrategyId, strategyId ) )
                {
                    foreach( DependencyProperty property in DependencyProperties )
                    {
                        if( Utils.StringCompareEquals( property.Name, propertyName ) )
                            return property;
                    }
                }

                // Si pas trouvé, on crée
                if( createIfNotExists && strategy.CheckPropertyValid( this, propertyName ) )
                {
                    using( Transaction transaction = this.Store.TransactionManager.BeginTransaction( "Initialise property value" ) )
                    {
                        DependencyProperty propertyInfo = new DependencyProperty( this.Store );
                        this.DependencyProperties.Add( propertyInfo );
                        propertyInfo.StrategyId = strategyId;
                        propertyInfo.Name = propertyName;
                        propertyInfo.Value = null;
                        transaction.Commit();
                        return propertyInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal virtual bool GenerateCode( GenerationContext context )
        {
            if( context.CanGenerate( this.Id ) )
            {
                DSLFactory.Candle.SystemModel.CodeGeneration.Generator.ApplyStrategies( this, context );
                if( context.IsModelSelected( this.Id ) )
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Nom de l'élément
        /// </summary>
        /// <value></value>
        public string Name
        {
            get
            {
                try
                {
                    return String.Format("{0} : {1} <-> {2}", this.GetType().Name, this.Client.Name, this.ExternalPublicPort.Parent.Name);
                }
                catch
                {
                    return this.GetType().Name;
                }
            }
        }

        /// <summary>
        /// Nom complet de l'élément
        /// </summary>
        /// <value></value>
        public string FullName
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
