using System;
using System.Windows.Forms;
namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Service permettant de manipuler l'interface graphique de Visual Studio
    /// </summary>
    public interface IIDEHelper
    {
        /// <summary>
        /// Gets the build events.
        /// </summary>
        /// <value>The build events.</value>
        EnvDTE.BuildEvents BuildEvents { get; }
        /// <summary>
        /// Clears the errors list
        /// </summary>
        void ClearErrors();
        /// <summary>
        /// Show a confirmation dialog box
        /// </summary>
        /// <returns>the confirmation response</returns>
        bool Confirm();
        /// <summary>
        /// Logs the error in the error window
        /// </summary>
        /// <param name="isWarning">if set to <c>true</c> [is warning].</param>
        /// <param name="message">The message.</param>
        /// <param name="line">The line (or 0)</param>
        /// <param name="column">The column (or 0)</param>
        /// <param name="fileName">Name of the file</param>
        void LogError(bool isWarning, string message, int line, int column, string fileName);
        /// <summary>
        /// Writes errors in the errors window
        /// </summary>
        /// <param name="errors">The errors list</param>
        void LogErrors(System.CodeDom.Compiler.CompilerErrorCollection errors);
        /// <summary>
        /// Logs the message in the output window
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        void LogMessage(string message, string source);
        /// <summary>
        /// Opens the models diagram.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="editorFactoryGuid">The editor factory GUID.</param>
        void OpenModelsDiagram(string fileName, Guid editorFactoryGuid);
        /// <summary>
        /// Display an error window and logs the message in the output window
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void ShowError(string msg);
        /// <summary>
        /// Activate the errors window
        /// </summary>
        void ShowErrorList();
        /// <summary>
        /// Sets the wait cursor.
        /// </summary>
        void SetWaitCursor();
        /// <summary>
        /// Displays the progress bar.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="processed">The processed.</param>
        /// <param name="total">The total.</param>
        void DisplayProgress(string message, int processed, int total);
        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void ShowMessage(string msg);
        /// <summary>
        /// Shows the window pane for port.
        /// </summary>
        /// <param name="portShape">The port shape.</param>
        void ShowWindowPaneForPort(Microsoft.VisualStudio.Modeling.Diagrams.PresentationElement portShape);
        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        DialogResult ShowMessageBox(string message, string title, MessageBoxButtons buttons);
    //void LogError(string origin, string message);
    }
}
