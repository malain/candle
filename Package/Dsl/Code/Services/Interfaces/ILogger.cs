using System;
using System.Collections.Generic;
using System.Text;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Status of the log entry
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// This log represents an error
        /// </summary>
        Error = 0,
        /// <summary>
        /// This log represents a warning
        /// </summary>
        Warning = 1,
        /// <summary>
        /// This log represents an info
        /// </summary>
        Info = 2,
        /// <summary>
        /// This log represents a debug information
        /// </summary>
        Debug = 3
    }

    /// <summary>
    /// Interface pour la g�n�ration de log
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// D�bute un nouveau processus de g�n�ration de log
        /// </summary>
        /// <param name="autoClose"></param>
        /// <param name="showWindow"></param>
        void BeginProcess(bool autoClose, bool showWindow);

        /// <summary>
        /// Termine le processus
        /// </summary>
        void EndProcess();

        /// <summary>
        /// D�bute une nouvelle �tape dans le log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        void BeginStep(string message, LogType messageType);

        /// <summary>
        /// Termine une �tape
        /// </summary>
        void EndStep();
        
        /// <summary>
        /// Affiche d'un message d'erreur
        /// </summary>
        /// <param name="message"></param>
        /// <param name="origin"></param>
        /// <param name="ex"></param>
        void WriteError(string origin, string message, Exception ex);

        /// <summary>
        /// Affichage d'une trace
        /// </summary>
        /// <param name="origin">D�crit qui est � l'origine du message</param>
        /// <param name="message">Le message.</param>
        /// <param name="messageType">Le type du message.</param>
        void Write(string origin, string message, LogType messageType);
    }
}
