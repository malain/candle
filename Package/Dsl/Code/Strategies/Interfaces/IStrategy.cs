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
//    /// Interface décrivant une stratégie
//    /// </summary>
//    /// <remarks>
//    /// Pour créer une nouvelle stratégie, vous devez implémenter cette interface ou plus simplement hériter
//    /// de <see cref="StrategyBase"/>
//    /// </remarks>
//    [CLSCompliant( false )]
//    public interface IStrategy
//    {
//        /// <summary>
//        /// Identifiant unique de la stratégie
//        /// </summary>
//        String StrategyId { get;set;}

//        /// <summary>
//        /// Description rapide de ce que fait la stratègie
//        /// </summary>
//        string Description { get;}

//        /// <summary>
//        /// Chemin pour l'affichage dans l'arbre des stratégies
//        /// </summary>
//        string StrategyPath { get;}

//        /// <summary>
//        /// Lien vers le site d'aide
//        /// </summary>
//        string HelpUrl { get;}

//        /// <summary>
//        /// Nom de la stratégie
//        /// </summary>
//        string DisplayName { get;}

//       ///// <summary>
//       // /// Indique si cette stratégie est applicable pour une élément
//       // /// </summary>
//       // /// <param name="element">Element concerné</param>
//       // /// <returns>True si elle s'applique pour ce type de couche</returns>
//       // bool IsApplicableOn(ICustomizableElement element);

//        /// <summary>
//        /// Lise des propriétés personnalisées de la couche
//        /// </summary>
//        /// <param name="modelElement">Element du modéle</param>
//        /// <returns></returns>
//        PropertyDescriptorCollection GetCustomProperties( ModelElement modelElement );

//        /// <summary>
//        /// Permet de bloquer la génération d'un modèle aux autres stratègies
//        /// </summary>
//        ///<param name="context">Current code generation context</param>
//        ///<param name="model">target model</param>
//        /// <param name="generatedFileName">Potential generated file name</param>
//        /// <returns>true is this generation is exclusive</returns>
//        bool IsModelGenerationExclusive( GenerationContext context, ICustomizableElement model, string generatedFileName );

//        /// <summary>
//        /// Initialise le contexte dans lequel va s'exécuter la stratégie
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
