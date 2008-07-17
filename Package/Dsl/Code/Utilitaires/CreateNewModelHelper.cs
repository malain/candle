using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Wizard;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    /// <summary>
    /// Classe permettant de créer un nouveau modèle dans une solution en permettant la sélection 
    /// de template
    /// </summary>
    internal sealed class CreateNewModelHelper
    {
        private const string ModelKey = "Model";
        private const string StrategiesKey = "Strategies";

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="projectFolder">The project folder.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <param name="template">The template.</param>
        /// <param name="strategyTemplate">The strategy template.</param>
        /// <param name="showDialog">if set to <c>true</c> [show dialog].</param>
        /// <returns></returns>
        internal static string CreateModel(string projectFolder, string solutionName, string template,
                                           string strategyTemplate, bool showDialog)
        {
            // Affichage de la boite de de dialogue
            //
            if (showDialog)
            {
                using (CandleWizardForm wizard = new CandleWizardForm("Candle project wizard"))
                {
                    wizard.SetUserData(ModelKey, template);
                    wizard.SetUserData(StrategiesKey, strategyTemplate);

                    wizard.AddPage(new ModelWizardPage(wizard, ModelKey, ModelKey));
                    wizard.AddPage(new StrategyWizardPage(wizard, StrategiesKey));

                    wizard.WizardFinished += delegate
                                                 {
                                                     template = wizard.GetUserData<string>(ModelKey);
                                                     strategyTemplate = wizard.GetUserData<string>(StrategiesKey);

                                                     if (template != null && Utils.StringStartsWith(template, "err:"))
                                                         wizard.SetCanceled();
                                                 };

                    if (wizard.Start() == DialogResult.Cancel)
                        throw new Exception("Canceled by User");
                }
            }

            string targetFileName =
                Path.Combine(projectFolder, String.Format("{0}{1}", solutionName, ModelConstants.FileNameExtension));

            // Récupération du fichier type sur le référentiel                
            CreateModelFileName(template, targetFileName);

            //Chargement des stratégies
            strategyTemplate = Path.GetFileNameWithoutExtension(strategyTemplate);
            if (!String.IsNullOrEmpty(strategyTemplate))
                ChangeStrategyTemplate(targetFileName, strategyTemplate);

            return targetFileName;
        }

        /// <summary>
        /// Création du fichier modèle à partir du nom sélectionné ou un fichier vide
        /// si problème
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="targetFileName">Name of the target file.</param>
        private static void CreateModelFileName(string template, string targetFileName)
        {
            bool createEmpty = true;
            if (File.Exists(targetFileName))
            {
                createEmpty = false;
            }
            else if (template != null)
            {
                try
                {
                    createEmpty =
                        !RepositoryManager.Instance.GetFileFromRepository(RepositoryCategory.Configuration, template,
                                                                          targetFileName);
                }
                catch
                {
                }
            }

            if (createEmpty)
            {
                CandleModel model = ModelLoader.CreateModel(Path.GetFileNameWithoutExtension(targetFileName), new VersionInfo(1,0,0,0));
                SerializationResult result = new SerializationResult();
                CandleSerializationHelper.Instance.SaveModel(result, model, targetFileName);
            }

            // Modification de l'identifiant du modele
            Dictionary<string, Guid> affectedIds = ReplaceAllIdsInModel(targetFileName);

            // Le diagramme associé
            template = template + ".diagram";
            targetFileName += ".diagram";
            if (
                RepositoryManager.Instance.GetFileFromRepository(RepositoryCategory.Configuration, template,
                                                                 targetFileName))
            {
                ReplaceAllIdsInDiagram(targetFileName, affectedIds);
            }
        }

        /// <summary>
        /// Replaces all ids in diagram.
        /// </summary>
        /// <param name="targetFileName">Name of the target file.</param>
        /// <param name="affectedIds">The affected ids.</param>
        private static void ReplaceAllIdsInDiagram(string targetFileName, IDictionary<string, Guid> affectedIds)
        {
            if (affectedIds == null) throw new ArgumentNullException("affectedIds");

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(targetFileName);
            ReplaceMonikers(affectedIds, xdoc);
            xdoc.Save(targetFileName);
        }

        /// <summary>
        /// Replaces the monikers.
        /// </summary>
        /// <param name="affectedIds">The affected ids.</param>
        /// <param name="xdoc">The xdoc.</param>
        private static void ReplaceMonikers(IDictionary<string, Guid> affectedIds, XmlDocument xdoc)
        {
            if (affectedIds == null) throw new ArgumentNullException("affectedIds");
            if (xdoc == null) throw new ArgumentNullException("xdoc");

            foreach (XmlNode node in xdoc.SelectNodes("//node()[@Id]"))
            {
                XmlAttribute attr = node.Attributes["Id"];
                Guid newId;
                string key = node.LocalName + ":" + attr.Value;
                if (affectedIds.TryGetValue(key, out newId))
                {
                    attr.Value = newId.ToString();
                }
            }
        }

        /// <summary>
        /// Replaces all ids in model.
        /// </summary>
        /// <param name="targetFileName">Name of the target file.</param>
        /// <returns></returns>
        private static Dictionary<string, Guid> ReplaceAllIdsInModel(string targetFileName)
        {
            Dictionary<string, Guid> affectedIds = new Dictionary<string, Guid>();

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(targetFileName);

            foreach (XmlNode node in xdoc.SelectNodes("//node()[@Id]"))
            {
                if (node.LocalName.EndsWith("Moniker"))
                    continue;

                XmlAttribute attr = node.Attributes["Id"];
                Guid newId = Guid.NewGuid();
                string key = node.LocalName + "Moniker:" + attr.Value;
                affectedIds.Add(key, newId);
                attr.Value = newId.ToString();
            }

            ReplaceMonikers(affectedIds, xdoc);

            xdoc.Save(targetFileName);
            return affectedIds;
        }

        /// <summary>
        /// Changes the strategy template.
        /// </summary>
        /// <param name="targetFileName">Name of the target file.</param>
        /// <param name="template">The template.</param>
        private static void ChangeStrategyTemplate(string targetFileName, string template)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(targetFileName);
            XmlNode node = xdoc.DocumentElement;
            if (node != null)
            {
                XmlAttribute attr = node.Attributes["strategyTemplate"];
                if (attr == null)
                {
                    attr = xdoc.CreateAttribute("strategyTemplate");
                    node.Attributes.Append(attr);
                }
                attr.Value = template;
                xdoc.Save(targetFileName);
            }
        }
    }
}