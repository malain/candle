using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// L'utilisateur a annul� une insertion 
    /// </summary>
    /// <remarks>
    /// Cette exception est lanc� dans un merge configure pour annuler l'insertion. Cette exception est intercept� dans
    /// le diagramme (ShouldReportException et UnhandledException)
    /// </remarks>
    public class CanceledByUser : Exception
    {
    }
}
