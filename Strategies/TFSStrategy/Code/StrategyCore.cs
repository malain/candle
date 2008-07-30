using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Client;
using System.ComponentModel;
using Microsoft.TeamFoundation.VersionControl.Common;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Strategy permettant de créer un label dans TFS lors de la publication du modèle sur le serveur
    /// </summary>
    [Strategy(TFSStrategy.StrategyID)]
    public class TFSStrategy : StrategyBase, IStrategyPublishEvents 
    {
        private const string StrategyID = "d9adb14f-3045-4d98-a133-585a05e9c6e9"; // or an unique name

        private int nbErrors;
        private string labelNameFormat;
        private LabelChildOption childOption;
        private string labelCommentFormat;
        private bool stopOnError;
        private bool forceCheckin;

        /// <summary>
        /// 
        /// </summary>
        public TFSStrategy()
        {
            forceCheckin = true;
            labelNameFormat = "Candle_{3}";
            labelCommentFormat = "Candle publication {0} version:{3} Changeset : {5}";
            childOption = LabelChildOption.Replace;
        }

        [Description("Force checkin on publication")]
        [DefaultValue(true)]
        public bool ForceCheckin
        {
            get { return forceCheckin; }
            set { forceCheckin = value; }
        }

        /// <summary>
        /// Format du commentaire du label avec {0}=datetime, {1}=workspace owner, {2}=machine name, {3}=model version, {4}=revision number, {5}=changeset"
        /// </summary>
        [Description("{0}=datetime, {1}=workspace owner, {2}=machine name, {3}=model version, {4}=revision number, {5}=changeset")]
        public string LabelCommentFormat
        {
            get { return labelCommentFormat; }
            set { labelCommentFormat = value; }
        }

        /// <summary>
        /// Indique si la publication est arrètêe en cas d'erreur
        /// </summary>
        [Description("Stop publication if errors")]
        public bool StopOnError
        {
            get { return stopOnError; }
            set { stopOnError = value; }
        }
	
        /// <summary>
        /// Option de création du label <see cref="Microsoft.TeamFoundation.VersionControl.Client.LabelChildOption"/>
        /// </summary>
        [Description("Label child option")]
        public LabelChildOption ChildOption
        {
            get { return childOption; }
            set { childOption = value; }
        }

        /// <summary>
        /// Format du nom du label avec {0}=datetime, {1}=workspace owner, {2}=machine name, {3}=model version, {4}=revision number, {5}=changeset"
        /// </summary>
        [Description("{0}=datetime, {1}=workspace owner, {2}=machine name, {3}=model version, {4}=revision number, {5}=changeset")]
        public string LabelNameFormat
        {
            get { return labelNameFormat; }
            set { labelNameFormat = value; }
        }

        /// <summary>
        /// Création d'un label lors de la publication
        /// </summary>
        /// <param name="model"></param>
        private void CreateLabel(CandleModel model, string modelFileName)
        {
            if (model == null || modelFileName == null)
                return;

            try
            {
                DTE dte = GetService<DTE>();

                string solutionFolder = Path.GetDirectoryName(modelFileName);

                // Récupère les caractèristiques du workspace contenant le fichier contenant le modèle
                if (Workstation.Current == null)
                    throw new Exception("TFS not installed");

                WorkspaceInfo wi = Workstation.Current.GetLocalWorkspaceInfo(solutionFolder);
                if (wi == null)
                {
                    LogError("The current solution is not in a Team System workspace");
                    return;
                }

                // Récupèration du server TFS à partir des infos du workspace
                TeamFoundationServer tfs = TeamFoundationServerFactory.GetServer(wi.ServerUri.AbsoluteUri);

                // Création d'un label sur la solution
                VersionControlServer vcs = tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;
                vcs.NonFatalError += new ExceptionEventHandler(vcs_NonFatalError);
                // On prend tous les fichiers de la solution
                ItemSpec itemSpec = new ItemSpec(solutionFolder, RecursionType.Full);
                LabelItemSpec labelItemSpec = new LabelItemSpec(itemSpec, VersionSpec.Latest, false);

                string changeSet = "-";
                // Calcul du nom du label
                string labelName = String.Format(labelNameFormat, DateTime.Now, wi.OwnerName, wi.Computer, model.Version, model.Version.Revision, changeSet);
                // Calcul du commentaire
                string labelComment = String.Format(labelCommentFormat, DateTime.Now, wi.OwnerName, wi.Computer, model.Version, model.Version.Revision, changeSet);

                //Checkin
                if (forceCheckin)
                {
                    Workspace ws = wi.GetWorkspace(tfs);

                    PendingChange[] pendingChanges = ws.GetPendingChanges(new ItemSpec[] { itemSpec }, false);
                    if (pendingChanges.Length > 0)
                    {
                        changeSet = ws.CheckIn(pendingChanges, labelComment).ToString();

                        // Mise à jour de l'explorateur de solution (icones)
                        Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte);
                        IVersionControlProvider versionControlProvider = (IVersionControlProvider)serviceProvider.GetService(typeof(IVersionControlProvider));
                        if( versionControlProvider!=null)
                            versionControlProvider.RefreshStatus();

                        // On intègre le changeset dans le commentaire
                        labelName = String.Format(labelNameFormat, DateTime.Now, wi.OwnerName, wi.Computer, model.Version, model.Version.Revision, changeSet);
                        labelComment = String.Format(labelCommentFormat, DateTime.Now, wi.OwnerName, wi.Computer, model.Version, model.Version.Revision, changeSet);
                    }
                }

                string scope;
                string label;
                LabelSpec.Parse(labelName, null, false, out label, out scope);
                VersionControlLabel vcl = new VersionControlLabel(vcs, label, null, scope, labelComment);

                // Et on applique le label.
                LabelResult[] results = vcs.CreateLabel(vcl, new LabelItemSpec[] { labelItemSpec }, childOption);                
                
            }
            catch (Exception ex)
            {
                LogError(ex);
                nbErrors++;
            }

            if (nbErrors > 0 && stopOnError)
                throw new PublishingException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vcs_NonFatalError(object sender, ExceptionEventArgs e)
        {
            string message = null;
            if (e.Exception != null)
            {
                message = e.Exception.Message;
            }

            if (e.Failure != null)
            {
                if (e.Failure.Severity == SeverityType.Warning)
                    return;
                message = e.Failure.Message;
            }

            LogError(message);
            nbErrors++;
        }


        #region IStrategyPublishEvents Members

        void IStrategyPublishEvents.OnAfterLocalPublication(CandleModel model, string modelFileName)
        {
        }

        void IStrategyPublishEvents.OnAfterServerPublication(CandleModel model, string modelFileName)
        {
        }

        void IStrategyPublishEvents.OnBeforeLocalPublication(CandleModel model, string modelFileName)
        {
        }

        void IStrategyPublishEvents.OnPublicationEnded(CandleModel model, string modelFileName)
        {
        }

        void IStrategyPublishEvents.OnPublicationError(CandleModel model, string modelFileName)
        {
        }

        void IStrategyPublishEvents.OnBeforeServerPublication(CandleModel model, string modelFileName)
        {
            // Juste avant la publication pour pouvoir la stopper si erreur
            CreateLabel(model, modelFileName);
        }

        #endregion
    }
}
