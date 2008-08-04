using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DSLFactory.Candle.SystemModel;


namespace DSLFactory.Candle.Repository
{
    /// <summary>
    /// Summary description for Logger
    /// </summary>
    public class Logger : DSLFactory.Candle.SystemModel.ILogger
    {
        private HttpContext context;

        public Logger()
        {
            //System.Diagnostics.TextWriterTraceListener listener = new System.Diagnostics.TextWriterTraceListener(context.Server.MapPath("request.log"));
            //System.Diagnostics.Trace.Listeners.Add(listener);
            System.Diagnostics.Trace.AutoFlush = true;
        }

        public void SetContext(HttpContext context)
        {
            this.context = context;
        }
        #region ILogger Members

        public void BeginProcess(bool autoClose, bool showWindow)
        {
        }

        public void EndProcess()
        {
        }

        public void BeginStep(string message, LogType messageType)
        {
        }

        public void EndStep()
        {
        }

        public void WriteError(string origin, string message, Exception ex)
        {
            Write(origin, String.Format("{0} - {1}", message, ex.Message), LogType.Error);
        }

        public void Write(string origin, string message, LogType messageType)
        {
            string id = HttpContext.Current.Request.Params["id"];
            if (id == null)
                id = HttpContext.Current.Request.UserHostName;
            string txt = String.Format(">> {0} (Id={1}) - {2}", DateTime.Now, id, message);
            System.Diagnostics.Trace.WriteLine(txt);
            if (context != null)
                context.Trace.Warn(txt);
        }

        #endregion
    }
}