using System;
using System.CodeDom.Compiler;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Designer;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider=Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    ///// <summary>
    ///// 
    ///// </summary>
    //public class NoIDEHelper : IIDEHelper
    //{
    //    #region IIDEHelper Membres
    //    /// <summary>
    //    /// Gets the build events.
    //    /// </summary>
    //    /// <value>The build events.</value>
    //    public BuildEvents BuildEvents
    //    {
    //        get { throw new Exception("The method or operation is not implemented."); }
    //    }
    //    /// <summary>
    //    /// Clears the errors list
    //    /// </summary>
    //    public void ClearErrors()
    //    {
    //    }
    //    /// <summary>
    //    /// Show a confirmation dialog box
    //    /// </summary>
    //    /// <returns>the confirmation response</returns>
    //    public bool Confirm()
    //    {
    //        return true;
    //    }
    //    public void LogError(bool isWarning, string message, int line, int column, string fileName)
    //    {
    //    }
    //    public void LogErrors(CompilerErrorCollection errors)
    //    {
    //    }
    //    public void LogMessage(string message)
    //    {
    //    }
    //    public void LogMessage(string message, string source)
    //    {
    //    }
    //    public void OpenModelsDiagram(string fileName, Guid editorFactoryGuid)
    //    {
    //    }
    //    public void ShowError(string msg)
    //    {
    //    }
    //    public void ShowErrorList()
    //    {
    //    }
    //    public void SetWaitCursor()
    //    {
    //    }
    //    public void DisplayProgress(string message, int processed, int total)
    //    {
    //    }
    //    public DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons)
    //    {
    //        return DialogResult.Cancel;
    //    }
    //    public void ShowMessage(string msg)
    //    {
    //    }
    //    public void ShowWindowPaneForPort(PresentationElement portShape)
    //    {
    //    }
    //    //public void LogError(string origin, string message)
    //    //{
    //    //}
    //    #endregion
    //}
    /// <summary>
    /// Classe permettant de manipuler la solution courante
    /// </summary>
    [CLSCompliant(false)]
    public class IDEHelper : IIDEHelper
    {
        private readonly DTE Dte;
        private readonly ErrorListProvider errorList;
        private readonly IVsOutputWindowPane outputPane;
        private readonly ModelingPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="IDEHelper"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        internal IDEHelper(ModelingPackage package)
        {
            this.package = package;

            Dte = (DTE) ModelingPackage.GetGlobalService(typeof (DTE));
            errorList = new ErrorListProvider(package);

            // Fenetre de sortie
            IVsOutputWindow outputWindowService = GetService<IVsOutputWindow>(typeof (SVsOutputWindow));
            if (outputWindowService != null)
            {
                Guid guid = Guid.NewGuid();
                outputWindowService.CreatePane(ref guid, "Candle", 1, 1);
                outputWindowService.GetPane(ref guid, out outputPane);
                outputPane.Clear();
            }
        }

        #region IIDEHelper Members

        /// <summary>
        /// Opens the models diagram.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="editorFactoryGuid">The editor factory GUID.</param>
        public void OpenModelsDiagram(string fileName, Guid editorFactoryGuid)
        {
            IVsUIShellOpenDocument document1 = GetService<IVsUIShellOpenDocument>();
            if (document1 != null)
            {
                IServiceProvider provider1;
                IVsUIHierarchy hierarchy1;
                uint num1;
                IVsWindowFrame frame1;
                Guid guid2 = Guid.Empty;
                if (
                    (ErrorHandler.Succeeded(
                         document1.OpenDocumentViaProjectWithSpecific(fileName, 3, ref editorFactoryGuid, string.Empty,
                                                                      ref guid2, out provider1, out hierarchy1, out num1,
                                                                      out frame1)) && (frame1 != null)) &&
                    !ErrorHandler.Failed(frame1.ShowNoActivate()))
                {
                    ErrorHandler.ThrowOnFailure(frame1.Show());
                }
            }
        }

        /// <summary>
        /// Gets the build events.
        /// </summary>
        /// <value>The build events.</value>
        public BuildEvents BuildEvents
        {
            get { return Dte.Events.BuildEvents; }
        }

        /// <summary>
        /// Affiche une messagebox et écrit le message dans la fenètre de sortie
        /// </summary>
        /// <param name="msg"></param>
        public void ShowError(string msg)
        {
            LogMessage(msg);
            ShowMessage(msg);
        }

        /// <summary>
        /// Affiche une message box
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(string msg)
        {
            ShowMessageBox(msg, String.Empty, MessageBoxButtons.OK);
            // Microsoft.SqlServer.MessageBox.ExceptionMessageBox msgbox = new ExceptionMessageBox();
        }

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons)
        {
            //string message = e.Message;
            //string title = string.Empty;
            //OLEMSGICON icon = OLEMSGICON.OLEMSGICON_CRITICAL;
            //OLEMSGBUTTON buttons = OLEMSGBUTTON.OLEMSGBUTTON_OK;
            //OLEMSGDEFBUTTON defaultButton = OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;
            //VsShellUtilities.ShowMessageBox(this.Site, title, message, icon, buttons, defaultButton);
            return MessageBox.Show(message, title, buttons);
        }

        /// <summary>
        /// Demande une confirmation
        /// </summary>
        /// <returns></returns>
        public bool Confirm()
        {
            return ShowMessageBox("Are you sure ?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

        #endregion

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetService<T>()
        {
            return GetService<T>(typeof (T));
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private T GetService<T>(Type type)
        {
            return (T) ((System.IServiceProvider) package).GetService(type);
        }

        #region Affichage des fenetres

        // Affichage de la fenêtre de détail pour les opérations d'un port
        /// <summary>
        /// Shows the window pane for port.
        /// </summary>
        /// <param name="portShape">The port shape.</param>
        public void ShowWindowPaneForPort(PresentationElement portShape)
        {
            PortWindowPane wnd = (PortWindowPane) package.GetToolWindow(typeof (PortWindowPane), true);
            if (wnd == null)
            {
                // Accés au package
                wnd = (PortWindowPane) package.GetToolWindow(typeof (PortWindowPane), true);
            }
            if (wnd != null)
            {
                wnd.ShowNoActivate();
                wnd.SetSelection(portShape);
            }
        }

        #endregion

        #region Log des erreurs

        /// <summary>
        /// Affiche un message dans la fenetre de sortie
        /// </summary>
        /// <param name="message">Texte du message</param>
        /// <param name="source">Source</param>
        public void LogMessage(string message, string source)
        {
            LogMessage(String.Format("{0} at {1}", message, source));
        }

        /// <summary>
        /// Ajoute les erreurs de compilation dans la task list
        /// </summary>
        /// <param name="errors">The errors list</param>
        public void LogErrors(CompilerErrorCollection errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            foreach (CompilerError error in errors)
            {
                LogError(error.IsWarning, error.ErrorText, error.Line, error.Column, error.FileName);
            }
        }

        /// <summary>
        /// Sets the wait cursor.
        /// </summary>
        public void SetWaitCursor()
        {
            IVsUIShell shell = GetService<IVsUIShell>(typeof (SVsUIShell));
            if (shell != null)
                shell.SetWaitCursor();
        }

        /// <summary>
        /// Vide la task list
        /// </summary>
        public void ClearErrors()
        {
            if (errorList != null)
            {
                errorList.Tasks.Clear();
                outputPane.Clear();
            }
        }

        /// <summary>
        /// Force l'affichage de la task list
        /// </summary>
        public void ShowErrorList()
        {
            if (errorList != null)
            {
                errorList.BringToFront();
                errorList.ForceShowErrors();
            }
        }


        /// <summary>
        /// Ajoute l'erreur dans la task list
        /// </summary>
        /// <param name="isWarning">true si c'est un avertissement sinon c'est une erreur</param>
        /// <param name="message">Texte du message</param>
        /// <param name="line">Ligne ou s'est produit l'erreur ou 0</param>
        /// <param name="column">Colonne ou s'est produit l'erreur ou 0</param>
        /// <param name="fileName">Nom du fichier contenant l'erreur ou chaine vide</param>
        public void LogError(bool isWarning, string message, int line, int column, string fileName)
        {
            if (line > 0)
            {
                line--;
            }
            if (column > 0)
            {
                column--;
            }

            foreach (ErrorTask task1 in errorList.Tasks)
            {
                // On ne met pas 2 fois le même message
                if ((task1.Text == message) && (task1.Document == fileName))
                {
                    return;
                }
            }
            ErrorTask task = new ErrorTask();
            task.IsPriorityEditable = false;
            task.IsTextEditable = false;
            task.IsCheckedEditable = false;
            task.Document = fileName;
            task.Category = TaskCategory.BuildCompile;
            task.CanDelete = false;
            task.Column = column;
            task.Line = line;
            task.Text = message;
            task.ErrorCategory = isWarning ? TaskErrorCategory.Warning : TaskErrorCategory.Error;
            //task1.Navigate += new EventHandler( this.task_Navigate );

            errorList.Tasks.Add(task);
        }

        /// <summary>
        /// Affiche un message dans la fenetre de sortie
        /// </summary>
        /// <param name="message">Texte du message</param>
        public void LogMessage(string message)
        {
            if (outputPane != null)
            {
#if DEBUG
                outputPane.OutputString( DateTime.Now.ToLongTimeString() );
                outputPane.OutputString( " : ");
#endif
                outputPane.OutputString(message);
                outputPane.OutputString("\r\n");
            }
        }

        #endregion

        #region Barre de progression

        private static IVsStatusbar statusbar;
        private static uint statusbarCookie = 0;

        // TIPS Progress bar
        /// <summary>
        /// Displays the progress bar.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="processed">The processed.</param>
        /// <param name="total">The total.</param>
        public void DisplayProgress(string message, int processed, int total)
        {
            if (message == null)
            {
                message = string.Empty;
            }
            if (processed < 0)
            {
                processed = 0;
            }
            if (total < 0)
            {
                total = 0;
            }
            if (processed < total)
            {
                if (statusbar == null)
                {
                    statusbar = GetService<IVsStatusbar>();
                }

                if (statusbar != null)
                {
                    statusbar.Progress(ref statusbarCookie, 1, message, (uint) processed, (uint) total);
                }
            }
            else
            {
                if (statusbarCookie != 0)
                {
                    if (statusbar == null)
                    {
                        statusbar = GetService<IVsStatusbar>();
                    }
                    if (statusbar != null)
                    {
                        statusbar.Progress(ref statusbarCookie, 0, string.Empty, (uint) total, (uint) total);
                    }
                }
                statusbar = null;
                statusbarCookie = 0;
            }
        }

        #endregion
    }
}