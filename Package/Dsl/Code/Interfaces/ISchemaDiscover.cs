using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISchemaDiscover
    {
        /// <summary>
        /// Récupére les colonnes d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        /// <remarks>
        /// Ne doit pas mettre à jour directement les colonnes dans la table
        /// </remarks>
        List<DbColumn> GetColumns( DbTable table );

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="proc">The proc.</param>
        /// <returns></returns>
        List<DbColumn> GetColumns(DbStoredProcedure proc);

        /// <summary>
        /// Liste des relations à partir et vers une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        List<DbRelationShip> GetRelations( DbTable table );

        /// <summary>
        /// Liste des index d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        List<DbIndex> GetIndexes( DbTable table );

    }
}
