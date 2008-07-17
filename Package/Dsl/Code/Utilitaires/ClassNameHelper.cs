using System;
using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Classe permettant de manipuler un nom de type
    /// </summary>
    [CLSCompliant(true)]
    public class ClassNameInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClassName;
        /// <summary>
        /// 
        /// </summary>
        public string[] NamespacesHierarchy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassNameInfo"/> class.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        public ClassNameInfo(string fullName)
        {
            ClassName = GetName(fullName);
            NamespacesHierarchy = GetNamespace(String.Empty, fullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassNameInfo"/> class.
        /// </summary>
        /// <param name="defaultNamespace">The default namespace.</param>
        /// <param name="fullName">The full name.</param>
        public ClassNameInfo(string defaultNamespace, string fullName)
        {
            ClassName = GetName(fullName);
            NamespacesHierarchy = GetNamespace(defaultNamespace, fullName);
        }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace
        {
            get { return Concat(NamespacesHierarchy); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    NamespacesHierarchy = value.Split('.');
                else
                    NamespacesHierarchy = new string[0];
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return GetFullName(ClassName); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        private static string GetName(string fullName)
        {
            int pos = fullName.LastIndexOf('.');
            if (pos >= 0)
                return fullName.Substring(pos + 1);

            return fullName;
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <param name="defaultNamespace">The default namespace.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        private static string[] GetNamespace(string defaultNamespace, string fullName)
        {
            //string layerName = null;

            //// Remplacement du namespace par le namespace initial
            //if( !String.IsNullOrEmpty( initialNamespace ) )
            //{
            //    int pos = initialNamespace.LastIndexOf( '.' );
            //    layerName = initialNamespace.Substring( pos + 1 );
            //    string ns = initialNamespace.Substring( 0, pos );

            //    // Si le fullname contient le namespace initial, on l'enléve
            //    if( fullName!=null && fullName.StartsWith( ns ) )
            //        fullName = fullName.Substring( ns.Length + 1 );
            //}
            //List<string> splits = new List<string>( fullName.Split( '.' ) );

            //splits.RemoveAt( splits.Count - 1 ); // On vire le nom du type
            ////if( layerName != null )
            ////    splits.Remove( layerName );
            //if( splits.Count > 0 )
            //    return splits.ToArray();


            // Si la classe contient un namespace, on le prend
            int pos = fullName.LastIndexOf('.');
            if (pos > 0)
            {
                return fullName.Substring(0, pos).Split('.');
            }

            // Sinon on prend celui fournit par défaut
            if (!String.IsNullOrEmpty(defaultNamespace))
                return defaultNamespace.Split('.');

            return new string[0];
        }

        /// <summary>
        /// Concats the specified parts.
        /// </summary>
        /// <param name="parts">The parts.</param>
        /// <returns></returns>
        private static string Concat(string[] parts)
        {
            return string.Join(".", parts);
        }

        /// <summary>
        /// Combines the with project namespace.
        /// </summary>
        /// <param name="appNamespace">The app namespace.</param>
        /// <returns></returns>
        public string CombineWithProjectNamespace(string appNamespace)
        {
            if (String.IsNullOrEmpty(appNamespace) && NamespacesHierarchy.Length == 0)
                return string.Empty;

            if (String.IsNullOrEmpty(appNamespace))
                return Concat(NamespacesHierarchy);

            if (NamespacesHierarchy.Length == 0)
                return appNamespace;

            return String.Format("{0}.{1}", appNamespace, Concat(NamespacesHierarchy));
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetFullName(string name)
        {
            List<string> tmp = new List<string>(NamespacesHierarchy);
            tmp.Add(name);
            return Concat(tmp.ToArray());
        }
    }
}