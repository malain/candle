using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe gérant les metadata des modèles dans le repository
    /// </summary>
    class ModelsRepositoryService : IModelsMetadata
    {
        /// <summary>
        /// Nom du fichier compréssé contenant le modèle et ses artifacts
        /// </summary>
        private const string ZipFileName = "models.zip";

        /// <summary>
        /// Liste des métadatas
        /// </summary>
        private readonly MetadataCollection _metadatas = new MetadataCollection();

        /// <summary>
        /// Liste des metadatas
        /// </summary>
        /// <value></value>
        public MetadataCollection Metadatas
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _metadatas; }
        }

        /// <summary>
        /// Publication d'un modele en tant que template
        /// </summary>
        /// <param name="modelFileName"></param>
        /// <param name="remoteName"></param>
        /// <param name="strategiesFileName"></param>
        /// <param name="remoteStrategiesFileName"></param>
        /// <remarks>
        /// Tout est mis dans un fichier compréssé.
        /// </remarks>
        public void PublishModelAsTemplate(string modelFileName, string remoteName, string strategiesFileName, string remoteStrategiesFileName)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            IRepositoryProvider repositoryProvider = RepositoryManager.Instance.GetMainRemoteProvider();

            // On peut publier en local ou à distance
            PublishModelForm form = null;
            IDialogService service = ServiceLocator.Instance.GetService<IDialogService>();
            if (service != null)
            {
                form = service.CreatePublishModelForm(repositoryProvider != null);
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            // Choix du provider de publication
            if (form == null || !form.PublishOnServer)
                repositoryProvider = RepositoryManager.Instance.GetLocalProvider();

            if (logger != null)
            {
                logger.BeginProcess(false, true);
                logger.BeginStep("Publish as template", LogType.Info);
            }

            try
            {
                // Le fichier des strategies
                if (!String.IsNullOrEmpty(remoteStrategiesFileName))
                {
                    remoteStrategiesFileName = Path.GetFileName(remoteStrategiesFileName);
                    if (repositoryProvider != null)
                        repositoryProvider.PublishFile(strategiesFileName, RepositoryCategory.Configuration, String.Format("Strategies/{0}{1}", remoteStrategiesFileName, StrategyManager.DefaultStrategiesFileNameExtension));
                }

                // Le modèle
                if (remoteName != null)
                {
                    remoteName = Path.GetFileName(remoteName);

                    // Le fichier modèle
                    string tempFile = Path.GetTempFileName();
                    try
                    {
                        File.Copy(modelFileName, tempFile, true);
                        ModelLoader loader = ModelLoader.GetLoader(tempFile, false);
                        using (Transaction transaction = loader.Model.Store.TransactionManager.BeginTransaction("set properties"))
                        {
                            // Modification du nom pour qu'on puisse le remplacer lors de l'initialisation (voir LayerNameChangeRule)
                            loader.Model.Component.Namespace = "?";
                            loader.Model.Component.Name = "?";
                            loader.Model.Name = String.Empty;

                            loader.Model.StrategyTemplate = Path.GetFileName(remoteStrategiesFileName);

                            foreach (SoftwareLayer layer in loader.Model.SoftwareComponent.Layers)
                            {
                                layer.Namespace = "?";
                            }

                            // Le fichier stratégie associé
                            if (!String.IsNullOrEmpty(remoteStrategiesFileName))
                                loader.Model.StrategyTemplate = Path.GetFileNameWithoutExtension(remoteStrategiesFileName);

                            transaction.Commit();
                        }

                        loader.Save(tempFile);

                        if (repositoryProvider != null)
                            repositoryProvider.PublishFile(tempFile, RepositoryCategory.Configuration, String.Format("Models/{0}{1}", remoteName, ModelConstants.FileNameExtension));
                    }
                    finally
                    {
                        File.Delete(tempFile);
                    }

                    // Liste des diagrammes dans le repository local
                    string diagramFilter = String.Format("{0}.diagram", Path.GetFileName(modelFileName));
                    foreach (string diagramFile in Directory.GetFiles(Path.GetDirectoryName(modelFileName), diagramFilter))
                    {
                        if (repositoryProvider != null)
                            repositoryProvider.PublishFile(diagramFile, RepositoryCategory.Configuration, String.Format("Models/{0}{1}{2}", remoteName, ModelConstants.FileNameExtension, ".diagram"));
                    }
                }

                if (logger != null)
                    logger.Write("Publish template", "published", LogType.Info);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Publish template", "Publish template", ex);
            }
            finally
            {
                if (logger != null)
                {
                    logger.EndStep();
                    logger.EndProcess();
                }
            }
        }

        /// <summary>
        /// Publication d'un modele
        /// </summary>
        /// <param name="model">Modèle à publier</param>
        /// <param name="fileName">Fichier initial contenant le modèle</param>
        /// <param name="promptWarning">Indique si il faut demander une confirmation à l'utilisateur</param>
        /// <remarks>
        /// Tout est mis dans un fichier compréssé.
        /// </remarks>
        public void PublishModel(CandleModel model, string fileName, bool promptWarning)
        {
            // La publication copie tous les artifacts dans le repository local puis le contenu de ce repository
            // est ensuite publié sur le serveur distant.

            // On publie tjs sur le serveur principal.
            // TODO voir si on ne peut pas publier sur le serveur d'origine du modèle (metadata.ServerUrl) ????
            IRepositoryProvider wsRepository = RepositoryManager.Instance.GetMainRemoteProvider();
            
            PublishModelForm form = null;
            if (promptWarning)
            {
                IDialogService service = ServiceLocator.Instance.GetService<IDialogService>();
                if (service != null)
                {
                    form = service.CreatePublishModelForm(wsRepository != null);
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }
            }

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.BeginProcess(false, true);

            try
            {
                // Sauvegarde 
                ServiceLocator.Instance.ShellHelper.Solution.DTE.Documents.SaveAll();

                Generator.NotifyPublishEvents(model, fileName, PublishEventType.BeforeLocal);

                // On copie d'abord en local
                PublishLocally(model, fileName);

                // Certaines strategies peuvent effectuer des actions lors de la publication
                Generator.NotifyPublishEvents(model, fileName, PublishEventType.AfterLocal);

                bool publishOnServer = true;
                if (form == null || !form.PublishOnServer)
                {
                    if (logger != null)
                        logger.Write("Custom strategy publishing actions", String.Format("Model {0} published locally.", Path.GetFileName(fileName)), LogType.Info);
                    publishOnServer = false;
                }

                if (publishOnServer)
                {
                    Generator.NotifyPublishEvents(model, fileName, PublishEventType.BeforeServer);
                    // Publication du repository local
                    PublishLocalModel(model);
                    Generator.NotifyPublishEvents(model, fileName, PublishEventType.AfterServer);
                }

                // Increment du numero de version du serveur
                using (Transaction transaction = model.Store.TransactionManager.BeginTransaction("Increment model number"))
                {
                    model.Version++;
                    transaction.Commit();
                }
                // On sauvegarde la modif
                ServiceLocator.Instance.ShellHelper.Solution.DTE.Documents.SaveAll();

                Generator.NotifyPublishEvents(model, fileName, PublishEventType.EndPublish);
            }
            catch
            {
                Generator.NotifyPublishEvents(model, fileName, PublishEventType.PublishFailed);
                if (logger != null)
                    logger.Write("PublishModel", "Model publication stopped", LogType.Error);
            }
            finally
            {
                if (logger != null)
                    logger.EndProcess();
                IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                if (ide != null)
                    ide.ShowErrorList();
            }
        }

        /// <summary>
        /// Copie du modèle dans le repository local et mise à jour du repository local
        /// avec les modèles des références externes du modèle et des artifacts
        /// </summary>
        /// <remarks>
        /// Cet événement se produit lors de la sauvegarde du modèle
        /// </remarks>
        /// <param name="model">Modèle à copier</param>
        /// <param name="fileName">Path du fichier contenant le modèle</param>
        private void PublishLocally(CandleModel model, string fileName)
        {
            Debug.Assert(model != null);
            Debug.Assert(fileName != null);

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
            {
                logger.BeginProcess(false, true);
                logger.BeginStep("Local publication", LogType.Info);
            }

            try
            {
                IRepositoryProvider localProvider = RepositoryManager.Instance.GetLocalProvider();
                Debug.Assert(localProvider != null);

                string sourceFolder = Path.GetDirectoryName(fileName);

                string folder = Path.GetDirectoryName(model.MetaData.GetFileName(PathKind.Absolute));
                Utils.RemoveDirectory(folder);

                // D'abord le modele. 
                try
                {
                    fileName = localProvider.PublishModel(model, fileName, fileName);
                    if (logger != null)
                        logger.Write("Local publication", "Publish model " + Path.GetFileName(fileName), LogType.Info);
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("Local publication", "Publish model " + Path.GetFileName(fileName), ex);
                }

                // Puis les diagrammes
                foreach (string diagramFile in Directory.GetFiles(sourceFolder, String.Format("{0}*.diagram", Path.GetFileName(fileName))))
                {
                    try
                    {
                        localProvider.PublishModel(model, diagramFile, diagramFile);
                        if (logger != null)
                            logger.Write("Local publication", "Publish diagram file " + Path.GetFileName(diagramFile), LogType.Info);
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError("Local publication", "Publish diagram file " + Path.GetFileName(diagramFile), ex);
                    }
                }

                ReferencesCollection references = DSLFactory.Candle.SystemModel.Dependencies.ReferencesHelper.GetReferences(new ConfigurationMode(), ReferenceScope.Publish, sourceFolder, model);
                //if (model.SoftwareComponent != null)
                //{
                //    DateTime lastModified = File.GetLastWriteTime(fileName);
                //    foreach (string referenceFileName in references)
                //    {
                //        if (File.Exists(referenceFileName) && lastModified < File.GetLastWriteTime(referenceFileName))
                //        {
                //            upToDate = false;
                //            break;
                //        }
                //    }
                //}

                // Ensuite les binaires et les artifacts
                foreach (string referenceFileName in references)
                {
                    try
                    {
                        localProvider.PublishModel(model, referenceFileName, referenceFileName);
                        if (logger != null)
                            logger.Write("Local publication", "Publish file " + Path.GetFileName(referenceFileName), LogType.Info);
                        
                        // Il y a peut-être des fichiers additionnels
                        string additionalFileName = Path.ChangeExtension(referenceFileName, ".xml");
                        if( File.Exists(additionalFileName))
                            localProvider.PublishModel(model, additionalFileName, additionalFileName);
                        additionalFileName = Path.ChangeExtension(referenceFileName, ".pdb");
                        if (File.Exists(additionalFileName))
                            localProvider.PublishModel(model, additionalFileName, additionalFileName);
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError("Local publication", "Publish file " + Path.GetFileName(referenceFileName), ex);
                    }
                }

                // On n'oublie pas de mettre à jour les metadata
                ComponentModelMetadata data = model.MetaData;
                Metadatas.Update(data, fileName);
            }
            finally
            {
                //if (!upToDate)
                //    logger.Write("Local publication", "Some files are more recents than the models", LogType.Warning);
                if (logger != null)
                {
                    logger.EndStep();
                    logger.EndProcess(); 
                    IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                    if (ide != null)
                        ide.ShowErrorList();
                }
            }
        }


        /// <summary>
        /// Publication du modèle sur le serveur
        /// </summary>
        /// <param name="model"></param>
        public void PublishLocalModel(CandleModel model)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
            {
                logger.BeginProcess(false, true);
                logger.BeginStep("Publish on remote server", LogType.Info);
            }

            try
            {
                // On récupère ses métadonnées
                ComponentModelMetadata data = model.MetaData;
                if (data != null)
                {
                    //if( !data.IsLastVersion())
                    //{
                    //    if (System.Windows.Forms.MessageBox.Show("You want to publish an older version than the server. Do you want to continue ?", "Warning", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    //        return;
                    //}

                    ModelLoader.Save(model, data.GetFileName(PathKind.Absolute));

                    // On prend tout ce qu'il y a dans le repository local (==> Il faut que la publication locale
                    // se fasse obligatoirement chaque fois avant )
                    string[] fileNames = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(data.GetFileName(PathKind.Absolute)));
                    PublishZip(model, new List<string>(fileNames), ZipFileName);

                    // Mise à jour des métadatas
                    Metadatas.SetMetadataPublished(data, model);

                    if (logger != null)
                        logger.Write("Publish on remote server", "Published on remote server ", LogType.Info);
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Publish on remote server ", "Publish on remote server", ex);
            }
            finally
            {
                if (logger != null)
                {
                    logger.EndStep();
                    logger.EndProcess();
                    IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                    if (ide != null)
                        ide.ShowErrorList();
                }
            }
        }

        /// <summary>
        /// Création d'un fichier zip et envoi sur le serveur
        /// </summary>
        /// <param name="systemDefinition"></param>
        /// <param name="fileNames"></param>
        /// <param name="remoteName"></param>
        private static void PublishZip(CandleModel systemDefinition, List<string> fileNames, string remoteName)
        {
            IRepositoryProvider wsRepository = RepositoryManager.Instance.GetMainRemoteProvider();
            if (wsRepository == null)
                return;

            string tempFile = Path.GetTempFileName();

            try
            {
                RepositoryZipFile zipFile = new RepositoryZipFile(tempFile);
                zipFile.ArchiveFiles(fileNames);
                wsRepository.PublishModel(systemDefinition, tempFile, remoteName);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /// <summary>
        /// Sélection d'un modèle
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns></returns>
        bool IModelsMetadata.SelectModel(ExternalComponent component, out ComponentModelMetadata metaData)
        {
            metaData = null;

            // Ici on veut saisir le fichier décrivant le système
            ComponentType? ct = null;
            if (component.MetaData != null)
                ct = component.MetaData.ComponentType;

            RepositoryTreeForm wizard = new RepositoryTreeForm(true, ct );
            if (wizard.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // On doit le créer
                if (wizard.SelectedItem == null)
                {
                    //
                    // Création d'un composant externe
                    //
                    if (component.Name == String.Empty)
                    {
                        PromptBox pb = new PromptBox("Name of the new component");
                        if (pb.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            return false;

                        // Vérification des doublons.
                        if (_metadatas.NameExists(pb.Value))
                        {
                            IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                            if (ide != null)
                                ide.ShowMessage("This model already exists in this model.");
                            return false;
                        }

                        component.Name = pb.Value;
                    }

                    // Création d'un modèle vide dans un répertoire temporaire
                    string relativeModelFileName = component.Name + ModelConstants.FileNameExtension;
                    string fn = Utils.GetTemporaryFileName(relativeModelFileName);
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fn));

                    // Persistence du modèle
                    CandleModel model = ModelLoader.CreateModel(component.Name, component.Version);
                    component.ModelMoniker = model.Id;                    
                    SerializationResult result = new SerializationResult();
                    CandleSerializationHelper.Instance.SaveModel(result, model, fn);

                    // Et ouverture du modèle
                    ServiceLocator.Instance.ShellHelper.Solution.DTE.ItemOperations.OpenFile(fn, EnvDTE.Constants.vsViewKindDesigner);
                }
                else // Sinon affectation
                {
                    metaData = wizard.SelectedItem;
                    ExternalComponent externalComponent = component.Model.FindExternalComponent(metaData.Id);
                    if (externalComponent != null)
                    {
                        IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                        if (ide != null)
                            ide.ShowMessage("This model already exists in this model.");
                        return false;
                    }                
                    metaData.UpdateComponent(component);
                }

                return true; // On ne passe pas dans le rollback
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un modele en local
        /// </summary>
        /// <param name="data"></param>
        public void RemoveModelInLocalRepository(ComponentModelMetadata data )
        {
            if (data == null)
                return;

            Metadatas.RemoveLocal(data);

            //Suppression physique du répertoire local
            Utils.RemoveDirectory(System.IO.Path.GetDirectoryName(data.GetFileName(PathKind.Absolute)));
        }

        /// <summary>
        /// Chargement du modèle en local
        /// </summary>
        /// <param name="metaData"></param>
        public bool GetModelInLocalRepository(ComponentModelMetadata metaData)
        {
            if (metaData == null)
                return false;

            if (GetModelInFolder(RepositoryManager.GetFolderPath(RepositoryCategory.Models), metaData))
            {
                Metadatas.SetMetadataLoaded(metaData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Récupére un modèle dans un répertoire
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns></returns>
        private static bool GetModelInFolder(string basePath, ComponentModelMetadata metaData)
        {
            // Si le modèle est plus récent en local, on demande à le récupérer
            // Non si l'utilisateur veut la dernière version, il doit faire un getlastversion
            //if (metaData.IsLastVersion())
            //{
            //    using (ModelSynchronizationForm confirm = new ModelSynchronizationForm(metaData))
            //    {
            //        if (confirm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            //            return null;
            //        if (confirm.ServerVersionSelected)
            //            RepositoryManager.Instance.ModelsMetadata.GetModelInLocalRepository(metaData);
            //    }
            //}

            IRepositoryProvider wsProvider = RepositoryManager.Instance.GetRemoteProvider(metaData.ServerUrl);
            if (wsProvider != null)
            {
                // Récupération et extraction du modele
                string zipFilePath = metaData.GetFileName(PathKind.Relative);
                zipFilePath = Path.Combine(Path.GetDirectoryName(zipFilePath), ZipFileName);

                RepositoryFile rfi = new RepositoryFile(Path.Combine(basePath, zipFilePath), RepositoryCategory.Models, zipFilePath);

                // Extraction
                string fullPath = rfi.LocalPhysicalPath;
                string folder = Path.GetDirectoryName(fullPath);

                // Si il existe en local (si il a bien été récupéré, on le décompresse)
                if (File.Exists(fullPath))
                {
                    RepositoryZipFile zipFile = new RepositoryZipFile(fullPath, false);
                    zipFile.ExtractAll(folder);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Récupération d'un modèle dans un répertoire temporaire pour visualisation
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public string CopyModelInTemporaryFolder(ComponentModelMetadata metaData)
        {
            if (metaData == null)
                return null;

            // Si le modéle n'existe pas dans le repository local, on le récupère dans un répertoire
            // temporaire pour ne pas polluer le répertoire local.
            string basePath = Utils.GetTemporaryFolder();
            string targetFileName = Path.Combine(basePath, metaData.GetFileName(PathKind.Relative));
            if (!metaData.ExistsLocally)
            {
                if (!GetModelInFolder(basePath, metaData))
                {
                    ServiceLocator.Instance.IDEHelper.ShowError("Model file name does not exist.");
                    return null;
                }
            }
            else
            {
                // Si le modèle est présent en local, on le copie pour le rendre read-only
                Utils.CopyDirectory(Path.GetDirectoryName(metaData.GetFileName(PathKind.Absolute)), Path.GetDirectoryName(targetFileName));
            }

            // Récupération du modèle dans un fichier temporaire pour pouvoir l'éditer
            if (File.Exists(targetFileName))
            {
                File.SetAttributes(targetFileName, FileAttributes.ReadOnly);
                return targetFileName;
            }
            return null;
        }
    }
}
