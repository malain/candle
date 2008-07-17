namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelConstants
    {
        /// <summary>
        /// Version de visual studio cible
        /// </summary>
        public const string VisualStudioVersion = "8.2.0.0";

        /// <summary>
        /// Key dans la base de registre 
        /// </summary>
        public const string VisualStudioRegKey = "8.0";

        /// <summary>
        /// Extension avec le point
        /// </summary>
        public const string FileNameExtension = ".cml";
        
        /// <summary>
        /// Filtre d'extension (*.xxx)
        /// </summary>
        public const string FilterExtension = "*" + FileNameExtension;
        /// <summary>
        /// 
        /// </summary>
        public const string FileDialogFilterExtension = "Models (" + FilterExtension + ")|" + FilterExtension;

        /// <summary>
        /// Nom de l'application
        /// </summary>
        public const string ApplicationName = "Candle";

        /// <summary>
        /// 
        /// </summary>
        public const string TemplateFolderName = "DSLFactory";

        /// <summary>
        /// 
        /// </summary>
        public const string SoftwareName = "Candle";

        /// <summary>
        /// 
        /// </summary>
        public const string ApplicationUriNamespace = "http://www.DSLFactory.org/Candle";
    }
}
