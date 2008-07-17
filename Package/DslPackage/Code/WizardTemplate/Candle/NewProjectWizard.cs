using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using EnvDTE80;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Wizard;
using System.Xml;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.TemplateWizards
{
    /// <summary>
    /// Assistant qui sera executé lors de l'ajout d'un nouveau projet.
    /// Activer la propriété de génération du projet "Inscrire pour COM Interop"
    /// Mettre les fichiers vsdir et vsz dans un répertoire DSLFactory dans "...\IDE\VC#\CSharpProjects" de visual studio
    /// </summary>
    [ComVisible( true )]
    [Guid( "33456E91-22C7-4c16-AE16-FEBB0579C1CA" )]
    public sealed class NewProjectWizard : IDTWizard
    {
        void IDTWizard.Execute( object application, int hwndOwner, ref object[] contextParams, ref object[] customParams, ref wizardResult result )
        {
            // Initialisation des paramètres
            //
            DTE2 dte = (DTE2)application;
            string wizardType = (string)contextParams[0];
            string projectFolder = (string)contextParams[2];
            string localDirectory = (string)contextParams[3];
            bool fExclusive = (bool)contextParams[4];
            string solutionName = (string)contextParams[5];


            // Fermeture de la solution si NewprojectType
            if( wizardType.Equals( "{0F90E1D0-4999-11D1-B6D1-00A0C90F2744}", StringComparison.InvariantCultureIgnoreCase ) && fExclusive )
            {
                if( dte.Solution.IsOpen && ( dte.ItemOperations.PromptToSave == vsPromptResult.vsPromptResultCancelled ) )
                {
                    result = wizardResult.wizardResultCancel;
                    return;
                }
            }

            try
            {
                if (String.IsNullOrEmpty(solutionName))
                {
                    solutionName = (string)contextParams[1];
                }

                // On descend d'un niveau car on travaille au niveau de la solution
                int pos = projectFolder.LastIndexOf(Path.DirectorySeparatorChar);
                projectFolder = projectFolder.Substring(0, pos);

                // Création de la solution
                Directory.CreateDirectory( projectFolder );
                dte.Solution.Create(projectFolder, solutionName);

                // Chargement du package 
                IVsShell shell =  (IVsShell)Microsoft.VisualStudio.Shell.Package.GetGlobalService( typeof( SVsShell ) );
                Guid packageGuid = new Guid( Constants.CandlePackageId );
                IVsPackage package=null;
                shell.LoadPackage( ref packageGuid, out package );

                if (package == null)
                    throw new Exception("Unable to load package.");

                // Recherche du template passé en paramètre sous la forme
                //  modelTemplate = xxxxxxx
                //
                string template = ExtractParam( customParams, "modelTemplate", null );
                string strategyTemplate = ExtractParam( customParams, "strategy", "default" );
                string showDialog = ExtractParam( customParams, "showDialog", "true" );

                ServiceLocator.Instance.ShellHelper.AddDSLModelToSolution(template, strategyTemplate, solutionName, showDialog!=null && showDialog.ToLower()=="true");
                result = wizardResult.wizardResultSuccess;
            }
            catch( Exception ex )
            {
                if(!(ex is CanceledByUser ))
                    MessageBox.Show(ex.Message);

                result = wizardResult.wizardResultCancel;
            }
        }

        /// <summary>
        /// Lecture de la valeur d'un paramètre dans le fichier de description du wizard
        /// </summary>
        /// <param name="customParams">Liste des paramètres sous la forme nom=valeur</param>
        /// <param name="parmName">Nom du paramètre à récupérer</param>
        /// <param name="defaultValue">Valeur par défaut</param>
        /// <returns></returns>
        private static string ExtractParam( object[] customParams, string parmName, string defaultValue )
        {
            foreach( string param in customParams )
            {
                string[] parts = param.Split( '=' );
                if( parts.Length == 2 && Utils.StringCompareEquals( parts[0].Trim(), parmName ) )
                {
                    return Path.GetFileNameWithoutExtension( parts[1].Trim() );
                }
            }
            return defaultValue;
        }    
    }
}
