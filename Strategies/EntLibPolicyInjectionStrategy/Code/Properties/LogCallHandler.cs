using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms.Design;
using System.Reflection;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    interface IPIABHandler
    {
        bool Enabled {get;}
        void SetAttribute(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function);
    }

    public class SerializablePropertyBase : ISerializableProperty 
    {
        #region ISerializableProperty Members
     
        public void ConvertFromString(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                string[] parts = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    string[] pair = part.Split(new char[] { '=' });
                    string name = pair[0].Trim();
                    PropertyInfo prop = this.GetType().GetProperty(name);
                    Debug.Assert(prop != null);
                    try
                    {
                        if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(this, Enum.Parse(prop.PropertyType, pair[1].Trim()), null);
                            continue;
                        }

                        if (prop.PropertyType.IsArray)
                        {
                            //continue;
                        }

                        TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyType);
                        if (((converter != null) && converter.CanConvertFrom(typeof(string))) && converter.CanConvertTo(typeof(string)))
                        {
                            object v = converter.ConvertFromInvariantString(pair[1].Trim());
                            if ((prop.PropertyType == typeof(bool) && (bool)v == false) || (prop.PropertyType == typeof(string) && String.IsNullOrEmpty((string)v)))
                                continue;
                            prop.SetValue(this, v, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                        if( logger!=null)
                            logger.WriteError("EnterpriseLibraryLogHandler" ,"Error when converting dependency property value", ex);
                    }
                    //if (!prop.PropertyType.IsArray)
                    //{
                    //    if (prop.PropertyType.IsEnum)
                    //        prop.SetValue(obj, Enum.Parse(prop.PropertyType, pair[1].Trim()), null);
                    //    else
                    //        prop.SetValue(obj, Convert.ChangeType(pair[1].Trim(), prop.PropertyType), null);
                    //}
                }
            }
        }

        public string ConvertToString()
        {
            PropertyInfo[] properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo prop in properties)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(prop.Name);
                sb.Append('=');

                if (prop.PropertyType.IsArray)
                {
                    //continue;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyType);
                if (((converter != null) && converter.CanConvertFrom(typeof(string))) && converter.CanConvertTo(typeof(string)))
                {
                    object v = prop.GetValue(this, null);
                    if( v != null)
                        sb.Append(converter.ConvertToInvariantString(v));
                }
            }
            return sb.ToString();
        }
        #endregion
    }

    [TypeConverter(typeof(ExpandableElementConverter<LogCallHandler>))]
    public class LogCallHandler : SerializablePropertyBase, IPIABHandler 
    {
        private int eventId;
        private bool logBeforeCall = true;
        private bool logAfterCall = false;
        private string beforeMessage = "Enter";
        private string afterMessage = "Exit";
        private bool includeParameters = true;
        private bool includeCallStack = false;
        private bool includeCallTime = true;
        private int priority = 100;
        private TraceEventType severity = TraceEventType.Error;
        private string categorie;

        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Gets or sets the collection of categories to place the log entries into.
        /// </summary>
        /// <remarks>The category strings can include replacement tokens. See
        /// the <see cref="Logging.CategoryFormatter"/> class for the list of tokens.</remarks>
        /// <value>The list of category strings.</value>
        public string Categorie
        {
            get { return categorie; }
            set { categorie = value; }
        }

        /// <summary>
        /// Event ID to include in log entry.
        /// </summary>
        /// <value>event id</value>
        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        /// <summary>
        /// Should there be a log entry before calling the target?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool LogBeforeCall
        {
            get { return logBeforeCall; }
            set { logBeforeCall = value; }
        }

        /// <summary>
        /// Should there be a log entry after calling the target?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool LogAfterCall
        {
            get { return logAfterCall; }
            set { logAfterCall = value; }
        }

        /// <summary>
        /// Message to include in a pre-call log entry.
        /// </summary>
        /// <value>The message</value>
        public string BeforeMessage
        {
            get { return beforeMessage; }
            set { beforeMessage = value; }
        }

        /// <summary>
        /// Message to include in a post-call log entry.
        /// </summary>
        /// <value>the message.</value>
        public string AfterMessage
        {
            get { return afterMessage; }
            set { afterMessage = value; }
        }

        /// <summary>
        /// Should the log entry include the parameters to the call?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool IncludeParameters
        {
            get { return includeParameters; }
            set { includeParameters = value; }
        }

        /// <summary>
        /// Should the log entry include the call stack?
        /// </summary>
        /// <remarks>Logging the call stack requires full trust code access security permissions.</remarks>
        /// <value>true = yes, false = no</value>
        public bool IncludeCallStack
        {
            get { return includeCallStack; }
            set { includeCallStack = value; }
        }

        /// <summary>
        /// Should the log entry include the time to execute the target?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool IncludeCallTime
        {
            get { return includeCallTime; }
            set { includeCallTime = value; }
        }

        /// <summary>
        /// Priority for the log entry.
        /// </summary>
        /// <value>priority</value>
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// Severity to log at.
        /// </summary>
        /// <value><see cref="TraceEventType"/> giving the severity.</value>
        public TraceEventType Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        public void SetAttribute(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        {
            function.AddAttribute("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.LogCallHandler", context.Strategy.StrategyId, false,  PrepareLogAttribute(function.ToString()));
        }

        //public static void RemoveAttributeIfExists(DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        //{
        //    function.RemoveAttributeIfExists("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.LogCallHandler");
        //}

        internal Dictionary<string, string> PrepareLogAttribute(string name)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(this.AfterMessage))
            {
                args.Add("AfterMessage", String.Format("\"{0} [{1}]\"", this.AfterMessage, name));
            }
            if (!String.IsNullOrEmpty(this.BeforeMessage))
            {
                args.Add("BeforeMessage", String.Format("\"{0} [{1}]\"", this.BeforeMessage, name));
            }
            if (!String.IsNullOrEmpty(this.Categorie))
            {
                args.Add("Categories", String.Format("new string[] {\"{0}\"}", this.Categorie.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));
            }
            if (this.EventId > 0)
            {
                args.Add("EventId", this.EventId.ToString());
            }
            if (this.LogBeforeCall)
            {
                args.Add("LogBeforeCall", "true");
            }
            if (this.LogAfterCall)
            {
                args.Add("LogAfterCall", "true");
            }
            if (this.IncludeParameters)
            {
                args.Add("IncludeParameters", "true");
            }
            if (this.IncludeCallStack)
            {
                args.Add("IncludeCallStack", "true");
            }
            if (this.IncludeCallTime)
            {
                args.Add("IncludeCallTime", "true");
            }

            if (this.Priority > 0)
            {
                args.Add("Priority", this.Priority.ToString());
            }
 
            args.Add("Severity", "System.Diagnostics.TraceEventType."+ this.Severity.ToString());
            return args;
        }
    }
}
