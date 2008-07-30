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
using DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel;

namespace DSLFactory.Candle.SystemModel.Strategies
{

    [TypeConverter(typeof(ExpandableElementConverter<PerformanceCounterCallHandler>))]
    public class PerformanceCounterCallHandler : SerializablePropertyBase, IPIABHandler
    {
        private string categoryName;
        private string instanceName;
        private bool incrementAverageCallDuration;
        private bool incrementCallsPerSecond;
        private bool incrementExceptionsPerSecond;
        private bool incrementNumberOfCalls;
        private bool incrementTotalExceptions;
        private bool useTotalCounter;
        private bool enabled;

        public PerformanceCounterCallHandler()
        {
            this.categoryName = String.Empty;
            this.instanceName = "{assembly}";
            this.incrementAverageCallDuration = true;
            this.incrementCallsPerSecond = true;
            this.incrementExceptionsPerSecond = false;
            this.IncrementNumberOfCalls = true;
            this.incrementTotalExceptions = false;
            this.useTotalCounter = true;
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Performance counter category to update.
        /// </summary>
        /// <value>category name</value>
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        /// <summary>
        /// Counter instance name. This may include replacement
        /// tokens. See the <see cref="MethodInvocationFormatter"/> class for a list of the tokens.
        /// </summary>
        /// <value>instance name.</value>
        public string InstanceName
        {
            get { return instanceName; }
            set { instanceName = value; }
        }

        /// <summary>
        /// Should the "average seconds / call" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementAverageCallDuration
        {
            get { return incrementAverageCallDuration; }
            set { incrementAverageCallDuration = value; }
        }

        /// <summary>
        /// Should the "calls / second" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementCallsPerSecond
        {
            get { return incrementCallsPerSecond; }
            set { incrementCallsPerSecond = value; }
        }

        /// <summary>
        /// Should the "# exceptions / second" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementExceptionsPerSecond
        {
            get { return incrementExceptionsPerSecond; }
            set { incrementExceptionsPerSecond = value; }
        }

        /// <summary>
        /// Should the number of calls counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementNumberOfCalls
        {
            get { return incrementNumberOfCalls; }
            set { incrementNumberOfCalls = value; }
        }

        /// <summary>
        /// Should the "# of exceptions" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementTotalExceptions
        {
            get { return incrementTotalExceptions; }
            set { incrementTotalExceptions = value; }
        }

        /// <summary>
        /// Should a "Total" instance be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool UseTotalCounter
        {
            get { return useTotalCounter; }
            set { useTotalCounter = value; }
        }
	
        //public static void RemoveAttributeIfExists(DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        //{
        //    function.RemoveAttributeIfExists("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.PerformanceCounterCallHandler");
        //}

        public void SetAttribute(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add(CandleCodeElement.EmptyAttributeNameSig + "0", "\"" + categoryName + "\"");
            args.Add(CandleCodeElement.EmptyAttributeNameSig + "1", "\"" + instanceName + "\"");
            if (incrementAverageCallDuration)
                args.Add("IncrementAverageCallDuration", "true");
            if (incrementCallsPerSecond)
                args.Add("IncrementCallsPerSecond", "true");
            if (incrementExceptionsPerSecond)
                args.Add("IncrementExceptionsPerSecond", "true");
            if (incrementNumberOfCalls)
                args.Add("IncrementNumberOfCalls", "true");
            if (incrementTotalExceptions)
                args.Add("IncrementTotalExceptions", "true");
            if (useTotalCounter)
                args.Add("UseTotalCounter", "true");

            function.AddAttribute("Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.PerformanceCounterCallHandler", 
                context.Strategy.StrategyId,
                false,
                args);

        }

    }
}
