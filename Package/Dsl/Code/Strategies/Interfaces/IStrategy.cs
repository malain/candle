//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.VisualStudio.Modeling;
//using System.ComponentModel;
//using EnvDTE;
//using Microsoft.VisualStudio.Modeling.Validation;

//namespace DSLFactory.Candle.SystemModel.Strategies
//{
//    /// <summary>
//    /// Interface d�crivant une strat�gie
//    /// </summary>
//    /// <remarks>
//    /// Pour cr�er une nouvelle strat�gie, vous devez impl�menter cette interface ou plus simplement h�riter
//    /// de <see cref="StrategyBase"/>
//    /// </remarks>
//    [CLSCompliant( false )]
//    public interface IStrategy
//    {
//        /// <summary>
//        /// Identifiant unique de la strat�gie
//        /// </summary>
//        String StrategyId { get;set;}

//        /// <summary>
//        /// Description rapide de ce que fait la strat�gie
//        /// </summary>
//        string Description { get;}

//        /// <summary>
//        /// Chemin pour l'affichage dans l'arbre des strat�gies
//        /// </summary>
//        string StrategyPath { get;}

//        /// <summary>
//        /// Lien vers le site d'aide
//        /// </summary>
//        string HelpUrl { get;}

//        /// <summary>
//        /// Nom de la strat�gie
//        /// </summary>
//        string DisplayName { get;}

//       ///// <summary>
//       // /// Indique si cette strat�gie est applicable pour une �l�ment
//       // /// </summary>
//       // /// <param name="element">Element concern�</param>
//       // /// <returns>True si elle s'applique pour ce type de couche</returns>
//       // bool IsApplicableOn(ICustomizableElement element);

//        /// <summary>
//        /// Lise des propri�t�s personnalis�es de la couche
//        /// </summary>
//        /// <param name="modelElement">Element du mod�le</param>
//        /// <returns></returns>
//        PropertyDescriptorCollection GetCustomProperties( ModelElement modelElement );

//        /// <summary>
//        /// Permet de bloquer la g�n�ration d'un mod�le aux autres strat�gies
//        /// </summary>
//        ///<param name="context">Current code generation context</param>
//        ///<param name="model">target model</param>
//        /// <param name="generatedFileName">Potential generated file name</param>
//        /// <returns>true is this generation is exclusive</returns>
//        bool IsModelGenerationExclusive( GenerationContext context, ICustomizableElement model, string generatedFileName );

//        /// <summary>
//        /// Initialise le contexte dans lequel va s'ex�cuter la strat�gie
//        /// </summary>
//        /// <param name="context"></param>
//        void InitializeContext( GenerationContext context, ICustomizableElement currentElement );

//        /// <summary>
//        /// Chargement de la strategy
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="eventArgs"></param>
//        void OnLoading(object sender, EventArgs eventArgs);
//    }
//}
