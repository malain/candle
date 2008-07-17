using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using VSLangProj;
using IExtenderProvider=EnvDTE.IExtenderProvider;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Extender pour tous les fichiers d'un projet candle qui
    /// permet d'ajouter la propriété 'CanRegenerate'. Cette propriété permet
    /// d'empècher la regénération d'un fichier. Cette propriété est stockée 
    /// dans un fichier de mapping associée à la solution.
    /// </summary>
    [Guid("F3E69344-1228-46a8-B117-9DC91CC7D142")]
    public class CandleExtenderProvider : Object, IExtenderProvider
    {
        private static string prjCATIDWebFileBrowseObject = "{E231573C-C018-4768-A383-18B73F267E71}";
        private static string staticExtenderName = "CandleExtender";

        /// <summary>
        /// Gets the name of the static extender.
        /// </summary>
        /// <value>The name of the static extender.</value>
        public static string StaticExtenderName
        {
            get { return staticExtenderName; }
        }

        //////////////////////////////////////////////////////////////////
        // IExtenderProvider Implementation

        #region IExtenderProvider Members

        /// <summary>
        /// Determines whether this instance can extend the specified extender CATID.
        /// </summary>
        /// <param name="ExtenderCATID">The extender CATID.</param>
        /// <param name="ExtenderName">Name of the extender.</param>
        /// <param name="ExtendeeObject">The extendee object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can extend the specified extender CATID; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExtend(string ExtenderCATID,
                              string ExtenderName,
                              object ExtendeeObject)
        {
            // check if provider can create extender for the given
            // ExtenderCATID, ExtenderName, and Extendee instance
            PropertyDescriptor extendeeCATIDProp =
                TypeDescriptor.GetProperties(ExtendeeObject)["ExtenderCATID"];
            if (!ExtenderName.Equals(StaticExtenderName) || null == extendeeCATIDProp)
                return false;
            try
            {
                string fullPath =
                    TypeDescriptor.GetProperties(ExtendeeObject)["FullPath"].GetValue(ExtendeeObject) as string;
                if (Mapper.Instance.FindMapItem(fullPath) == null)
                    return false;

                if (ExtenderCATID.ToUpper().Equals(PrjBrowseObjectCATID.prjCATIDCSharpFileBrowseObject.ToUpper())
                    &&
                    extendeeCATIDProp.GetValue(ExtendeeObject).ToString().ToUpper().Equals(
                        PrjBrowseObjectCATID.prjCATIDCSharpFileBrowseObject.ToUpper()))
                    return Mapper.Instance.IsValid;

                if (ExtenderCATID.ToUpper().Equals(prjCATIDWebFileBrowseObject)
                    &&
                    extendeeCATIDProp.GetValue(ExtendeeObject).ToString().ToUpper().Equals(prjCATIDWebFileBrowseObject))
                    return Mapper.Instance.IsValid;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Gets the extender.
        /// </summary>
        /// <param name="ExtenderCATID">The extender CATID.</param>
        /// <param name="ExtenderName">Name of the extender.</param>
        /// <param name="ExtendeeObject">The extendee object.</param>
        /// <param name="ExtenderSite">The extender site.</param>
        /// <param name="Cookie">The cookie.</param>
        /// <returns></returns>
        public object GetExtender(string ExtenderCATID,
                                  string ExtenderName,
                                  object ExtendeeObject,
                                  IExtenderSite ExtenderSite,
                                  int Cookie)
        {
            CandleExtender staticExtender = null;

            if (CanExtend(ExtenderCATID, ExtenderName, ExtendeeObject))
            {
                //EnvDTE.Solution solution = ((EnvDTE.DTE)ExtenderSite.GetObject("")).Solution;
                //if (null == solution)
                //{
                //    throw new NullReferenceException("((EnvDTE.DTE) " + "ExtenderSite.GetObject(\"\")).Solution failed");
                //}
                PropertyDescriptor fileNameProp = TypeDescriptor.GetProperties(ExtendeeObject)["FullPath"];
                String fileName = fileNameProp.GetValue(ExtendeeObject) as String;
                staticExtender = new CandleExtender();
                staticExtender.Init(fileName, Cookie, ExtenderSite);
            }
            return staticExtender;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ICandleExtender
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance can regenerate.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can regenerate; otherwise, <c>false</c>.
        /// </value>
        bool CanRegenerate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class CandleExtender : ICandleExtender, IFilterProperties
    {
        private const string fileCanGeneratePropName = "FileCanGenerate";
        //////////////////////////////////////////////////////////////////
        // StaticExtender Fields
        private IExtenderSite _extenderSite;
        private string _fileName;
        private int _siteCookie;

        #region ICandleExtender Members

        /// <summary>
        /// Gets or sets a value indicating whether this instance can regenerate.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can regenerate; otherwise, <c>false</c>.
        /// </value>
        [Browsable(true)]
        [Category("Code generation")]
        public bool CanRegenerate
        {
            get { return Mapper.Instance.GetCanGeneratePropertyValue(_fileName); }
            set { Mapper.Instance.SetCanGeneratePropertyValue(_fileName, value); }
        }

        #endregion

        #region IFilterProperties Members

        /// <summary>
        /// Determines whether [is property hidden] [the specified property name].
        /// </summary>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public vsFilterProperties IsPropertyHidden(string PropertyName)
        {
            vsFilterProperties propertyHidden = vsFilterProperties.vsFilterPropertiesNone;

            // Si on ne trouve pas d'association, on n'affiche rien
            if (PropertyName == "CanRegenerate" && Mapper.Instance.FindMapItem(_fileName) == null)
            {
                propertyHidden = vsFilterProperties.vsFilterPropertiesAll;
            }
            return propertyHidden;
        }

        #endregion

        /// <summary>
        /// Inits the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="ExtenderCookie">The extender cookie.</param>
        /// <param name="ExtenderSite">The extender site.</param>
        public void Init(string fileName,
                         int ExtenderCookie,
                         IExtenderSite ExtenderSite)
        {
            _extenderSite = ExtenderSite;
            _siteCookie = ExtenderCookie;
            _fileName = fileName;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="CandleExtender"/> is reclaimed by garbage collection.
        /// </summary>
        ~CandleExtender()
        {
            // Wrap this call in a try-catch to avoid any failure code the
            // Site may return. For instance, since this object is GC'ed,
            // the Site may have already let go of its Cookie.
            try
            {
                if (_extenderSite != null && _siteCookie != 0)
                {
                    _extenderSite.NotifyDelete(_siteCookie);
                    _siteCookie = 0;
                }
            }
            catch
            {
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        // IStaticExtender Implementation
        //#region IStaticExtender Members

        //public string SolutionDescription
        //{
        //    get
        //    {
        //        EnvDTE.Globals solutionGlobals = solution.Globals;
        //        if (!solutionGlobals.get_VariableExists(slnDescPropName))
        //        {
        //            solutionGlobals[slnDescPropName] = string.Empty;
        //            solutionGlobals.set_VariablePersists(slnDescPropName, true);
        //        }
        //        return solutionGlobals[slnDescPropName].ToString();
        //    }
        //    set
        //    {
        //        EnvDTE.Globals solutionGlobals = solution.Globals;
        //        solutionGlobals[slnDescPropName] = value;
        //        solutionGlobals.set_VariablePersists(slnDescPropName, true);
        //    }
        //}

        //#endregion

        //////////////////////////////////////////////////////////////////
        // IFilterProperties Implementation
        // public EnvDTE.vsFilterProperties IsPropertyHidden(string PropertyName)
        // Microsoft.VisualStudio.PropertyBrowser.AutoExtMgr_IFilterProperties 
    }
}