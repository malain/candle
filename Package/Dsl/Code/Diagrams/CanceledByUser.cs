using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// L'utilisateur a annulé une insertion 
    /// </summary>
    /// <remarks>
    /// Cette exception est lancé dans un merge configure pour annuler l'insertion. Cette exception est intercepté dans
    /// le diagramme (ShouldReportException et UnhandledException)
    /// </remarks>
    public class CanceledByUser : Exception
    {
    }
}
