using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using DSLFactory.Candle.SystemModel.Repository;
using System.ComponentModel;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [TypeDescriptionProvider( typeof( StrategyProviderTypeDescriptorProvider ) )]
    public partial class CandleElement : ICustomizableElement
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public abstract ICustomizableElement Owner { get;}

        #region Gestion des strat�gies

        // Lors de la cr�ation d'un element, on va afficher un wizard que l'utilisateur peut annuler.
        // TIPS permet d'intercepter la cr�ation d'un enfant
        /// <summary>
        /// Execution d'un wizard lors de la cr�ation d'un �l�ment
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="defaultWizard">The default wizard.</param>
        /// <returns></returns>
        public  bool ExecuteWizard( ModelElement element, IStrategyWizard defaultWizard )
        {
            if (!(element is ICustomizableElement))
                return true;

            try
            {
                StrategyElementElementAddedEventArgs e = new StrategyElementElementAddedEventArgs( element );

                foreach( StrategyBase strategy in GetStrategies(false) )
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

                Generator.GenerateWhenElementAdded(this.Store, CandleModel.GetInstance(this.Store), element as ICustomizableElement);
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
        /// Ajout d'un �l�ment
        /// </summary>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public virtual bool OnElementAdded( StrategyElementElementAddedEventArgs e )
        {
            foreach( StrategyBase strategy in GetStrategies(false) )
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
        /// Full name with the namespace
        /// </summary>
        public virtual string FullName
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return Name; }
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
                Generator.ApplyStrategies( this, context );
                if( context.IsModelSelected( this.Id ) )
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public abstract CandleElement StrategiesOwner { get;}

        /// <summary>
        /// Liste des strat�gies concernant ce mod�le
        /// </summary>
        /// <param name="specific">Seules les strat�gies applicables ou toutes</param>
        /// <returns></returns>
        public virtual List<StrategyBase> GetStrategies(bool specific)
        {
            return StrategyManager.GetStrategies(StrategiesOwner, specific ? this : null);
        }

        /// <summary>
        /// Recherche la propri�t� personnalis� et la cr�e si elle n'existe pas
        /// </summary>
        /// <param name="strategyId">Identifiant de la strat�gie</param>
        /// <param name="propertyName">Nom de la propri�t�</param>
        /// <param name="createIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns></returns>
        public DependencyProperty GetStrategyCustomProperty(string strategyId, string propertyName, bool createIfNotExists)
        {
            foreach( StrategyBase strategy in GetStrategies(false) )
            {
                if( Utils.StringCompareEquals( strategy.StrategyId, strategyId ) )
                {
                    foreach( DependencyProperty property in DependencyProperties )
                    {
                        if( Utils.StringCompareEquals( property.Name, propertyName ) )
                            return property;
                    }
                }

                // Si pas trouv�, on cr�e
                if( createIfNotExists && strategy.CheckPropertyValid( this, propertyName ) )
                {
                    if (!this.Store.InUndoRedoOrRollback)
                    {
                        using (Transaction transaction = this.Store.TransactionManager.BeginTransaction("Initialise property value"))
                        {
                            DependencyProperty propertyInfo = new DependencyProperty(this.Store);
                            this.DependencyProperties.Add(propertyInfo);
                            propertyInfo.StrategyId = strategyId;
                            propertyInfo.Name = propertyName;
                            propertyInfo.Value = null;
                            transaction.Commit();
                            return propertyInfo;
                        }
                    }
                }
            }
            return null;
        }
    }
}
