using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Argument de l'�v�nement <see cref="IStrategyAddElementInterceptor"/>
    /// </summary>
    [CLSCompliant(true)]
    public class StrategyElementElementAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Indique si on arrete la propagation de l'�venement dans les autres strat�gies
        /// </summary>
        public bool CancelBubble;

        /// <summary>
        /// Indique si l'utilisateur a annul�
        /// </summary>
        public bool UserCancel;

        /// <summary>
        /// El�ment qui vient d'�tre ajout�
        /// </summary>
        public ModelElement ModelElement;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> ContextInfo;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="model">El�ment qui vient d'�tre ajout�</param>
        public StrategyElementElementAddedEventArgs( ModelElement model )
        {
            CancelBubble = false;
            UserCancel = false;
            ModelElement = model;
            ContextInfo = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant( false )]
    public interface IStrategyAddElementInterceptor
    {
        /// <summary>
        /// Ev�nement d�clench� lors d'un ajout d'un �l�ment dans le mod�le
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        void OnElementAdded( ICustomizableElement owner, StrategyElementElementAddedEventArgs e );

        /// <summary>
        /// Permet de fournir un wizard lors de l'insertion d'un �lement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IStrategyWizard GetWizard( ModelElement model );
    }
}
