using System;
using DSLFactory.Candle.SystemModel.Configuration;

namespace DSLFactory.Candle.SystemModel.VisualStudio
{
    /// <summary>
    /// Classe permettant de loguer la génération de code
    /// </summary>
    /// <remarks>
    /// Le log peut être générée sous 3 formes:
    ///  - Une fenetre spécifique
    ///  - La fenetre d'erreur de Visual Studio 
    ///  - La fenetre de sortie dédiée de Visual Studio
    /// </remarks>
    public class Logger : ILogger
    {
        /// <summary>
        /// Fenetre d'affichage
        /// </summary>
        private CandleLogWindow _wnd;

        /// <summary>
        /// Tabulation
        /// </summary>
        int _tab;

        /// <summary>
        /// Compteur de processus démarré
        /// </summary>
        private int _count;


        /// <summary>
        /// Débute un nouveau processus de génération de log
        /// </summary>
        /// <param name="autoClose"></param>
        /// <param name="showWindow"></param>
        public void BeginProcess(bool autoClose, bool showWindow)
        {
            IIDEHelper ide = ServiceLocator.Instance.IDEHelper;

            // Si il y a une fenetre, on l'affiche
            if (showWindow && _wnd == null)
            {
                _wnd = new CandleLogWindow(autoClose);
                _wnd.Show();
                _wnd.TopLevel = true;
            }

            if (ide != null && _count == 0)
                ServiceLocator.Instance.IDEHelper.ClearErrors();

            // Nbre de processus
            _count++;
        }

        /// <summary>
        /// Fin d'un processus
        /// </summary>
        public void EndProcess()
        {
            // On décrémente le compteur des processus
            _count--;

            // Si il y en a plus, on indique la fin du processus principal
            if (_count == 0)
            {
                Write("", "Terminated", LogType.Info);
                if (_wnd != null)
                    _wnd.End(); // Affiche le bouton close
                _wnd = null;

                // NON car cela peut arriver quand on modifie la fenetre detail (un GenerateWhenElementAdded est activé)
                //IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                //if (ide != null)
                //    ide.ShowErrorList();
            }
        }

        /// <summary>
        /// Affichage d'une trace
        /// </summary>
        /// <param name="origin">Décrit qui est à l'origine du message</param>
        /// <param name="message">Le message.</param>
        /// <param name="messageType">Le type du message.</param>
        public void Write(string origin, string message, LogType messageType)
        {
            if (_wnd != null && messageType != LogType.Debug) // sauf debug
                _wnd.AddText(message, messageType);

            try
            {
                IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                if (ide != null)
                {
                    if (messageType == LogType.Error)
                        ide.LogError(false, message, 0, 0, origin);
                    if (messageType == LogType.Warning)
                        ide.LogError(true, message, 0, 0, origin);

                    if (messageType != LogType.Debug || CandleSettings.GenerationTraceEnabled)
                    {
                        string pad = new string(' ', _tab);
                        ide.LogMessage(String.Concat(pad, message), origin);
                    }
                }
            }
            catch {// TODO créer un logger pour le repository
            }
        }


        /// <summary>
        /// Affiche d'un message d'erreur
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void WriteError(string origin, string message, Exception ex)
        {
            string exMessage = ex != null ? ex.Message : String.Empty;
            exMessage += " stack=" + ex.StackTrace;
            Write(origin, String.Concat("[error ", origin, "] - ", message, " ", exMessage), LogType.Error);
        }


        /// <summary>
        /// Begins the step.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="msgType">Type of the MSG.</param>
        public void BeginStep(string message, LogType msgType)
        {
            if (_wnd != null && (msgType != LogType.Debug || CandleSettings.GenerationTraceEnabled ))
                _wnd.AddStep(message);
            _tab += 2;
        }


        /// <summary>
        /// Termine une étape
        /// </summary>
        public void EndStep()
        {
            _tab -= 2;
        }
    }
}
