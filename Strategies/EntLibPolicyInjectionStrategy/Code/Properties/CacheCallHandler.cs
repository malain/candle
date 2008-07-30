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

    [TypeConverter(typeof(ExpandableElementConverter<CacheCallHandler>))]
    public class CacheCallHandler : ISerializableProperty, IPIABHandler
    {
        private TimeSpan expirationTime;

        [Browsable(false)]
        public bool Enabled
        {
            get { return expirationTime != null && expirationTime.TotalMinutes > 0; }
        }

        public TimeSpan ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }
	
        #region ISerializableProperty Members
        public void ConvertFromString(string value)
        {
            if (!String.IsNullOrEmpty(value) && value != "[No Cache]")
            {
                try
                {
                    expirationTime = TimeSpan.Parse(value);
                }
                catch
                {
                    expirationTime = new TimeSpan(0, 0, 0);
                }
            }
        }

        public string ConvertToString()
        {
            if (expirationTime == null || expirationTime.TotalMinutes == 0)
                return "[No Cache]";

            return expirationTime.ToString();
        }
        #endregion

        //public static void RemoveAttributeIfExists(DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        //{
        //    function.RemoveAttributeIfExists("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.CachingCallHandler");
        //}

        public void SetAttribute(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        {
            function.AddAttribute("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.CachingCallHandler",
                context.Strategy.StrategyId, false,
                String.Empty, expirationTime.Hours.ToString(),
                String.Empty, expirationTime.Minutes.ToString(),
                String.Empty, expirationTime.Seconds.ToString());
        }

    }
}
