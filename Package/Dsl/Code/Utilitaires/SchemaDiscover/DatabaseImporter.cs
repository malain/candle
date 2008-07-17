using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseImporter : IDatabaseImporter
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ImportedColumnInfo = "ImportDB_column";
        /// <summary>
        /// 
        /// </summary>
        public const string ImportedProcedureInfo = "ImportDB_databaseStoredProcedure";
        /// <summary>
        /// 
        /// </summary>
        public const string ImportedRelationInfo = "ImportDB_relation";
        /// <summary>
        /// 
        /// </summary>
        public const string ImportedTableInfo = "ImportDB_databaseTable";

        private DataLayer _layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseImporter"/> class.
        /// </summary>
        internal DatabaseImporter()
        {
        }

        #region IDatabaseImporter Members

        /// <summary>
        /// Import des tables
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="parentPackage">The parent package.</param>
        /// <param name="dbObjects">The db objects.</param>
        /// <param name="dbType">Type of the db.</param>
        public void Import(IDbConnection connection, Package parentPackage, List<DbContainer> dbObjects,
                           DatabaseType dbType)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            _layer = parentPackage.Layer;

            ISchemaDiscover schemaDiscover = GetSchemaDiscover(connection);
            string connectionTypeName = connection != null ? connection.GetType().Name : "??unknow??";
            if (schemaDiscover == null)
            {
                if (logger != null)
                    logger.Write("Import DbTable", "schema discover not found for connection " + connectionTypeName,
                                 LogType.Error);
                return;
            }

            string providerName = connection.GetType().Namespace;

            try
            {
                parentPackage.Layer.AddXmlConfigurationContent("ConnectionStrings",
                                                               String.Format(
                                                                   @"<configuration><connectionStrings><add name=""{0}"" connectionString=""{1}"" providerName=""{2}""/></connectionStrings></configuration>",
                                                                   connectionTypeName,
                                                                   connection.ConnectionString.Replace('"', ' '),
                                                                   providerName));
            }
            catch (Exception cfgEx)
            {
                if (logger != null)
                    logger.WriteError("Import DbTable", "Registering connectionString", cfgEx);
            }

            // Création des entités
            foreach (DbContainer dbContainer in dbObjects)
            {
                try
                {
                    // On recherche si il n'y a pas dèjà une classe qui pointe sur la même table
                    Entity doublon = FindEntity(dbContainer.Name);
                    if (doublon != null)
                    {
                        IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                        if (ide != null)
                        {
                            if (
                                ide.ShowMessageBox(
                                    String.Concat("The entity ", doublon.Name,
                                                  " is binding to the same table. Do you want to continue ?"), "Warning",
                                    MessageBoxButtons.YesNo) == DialogResult.No)
                                continue;
                        }
                    }

                    dbContainer.Connection = connection;
                    using (
                        Transaction transaction =
                            parentPackage.Store.TransactionManager.BeginTransaction("Add entity from DB"))
                    {
                        transaction.Context.ContextInfo.Add(
                            dbType == DatabaseType.Table ? ImportedTableInfo : ImportedProcedureInfo, dbContainer);

                        Entity entity = new Entity(parentPackage.Store);
                        EntityNameConfirmation dlg = new EntityNameConfirmation(parentPackage.Layer, dbContainer.Name);
                        if (dlg.ShowDialog() == DialogResult.Cancel)
                            continue;

                        entity.DatabaseType = dbType;
                        entity.RootName = dlg.RootName;
                        entity.Name = dlg.EntityName;
                        entity.TableName = dbContainer.Name;
                        entity.TableOwner = dbContainer.Owner;
                        entity.DatabaseType = dbType;
                        parentPackage.Types.Add(entity);
                        dbContainer.Entity = entity;

                        List<DbColumn> columns = null;

                        DbTable dbTable = dbContainer as DbTable;
                        if (dbTable != null)
                        {
                            List<DbIndex> indexes = schemaDiscover.GetIndexes(dbTable);
                            dbTable.Indexes = indexes;
                            columns = schemaDiscover.GetColumns(dbTable);
                        }
                        else if (dbContainer is DbStoredProcedure)
                            columns = schemaDiscover.GetColumns(dbContainer as DbStoredProcedure);

                        dbContainer.Columns = columns;

                        foreach (DbColumn column in columns)
                        {
                            using (
                                Transaction transaction2 =
                                    parentPackage.Store.TransactionManager.BeginTransaction("Add column in entity"))
                            {
                                transaction2.Context.ContextInfo.Add(ImportedColumnInfo, column);
                                Property property = new Property(entity.Store);
                                property.Name =
                                    StrategyManager.GetInstance(property.Store).NamingStrategy.ToPascalCasing(
                                        column.Name);
                                property.RootName = property.Name;
                                property.ColumnName = column.Name;
                                property.Type = column.ClrType.FullName;
                                property.Nullable = column.IsNullable;
                                property.ServerType = column.ServerType;
                                property.IsPrimaryKey = column.InPrimaryKey;
                                property.IsAutoIncrement = column.IsAutoIncrement;
                                entity.Properties.Add(property);
                                column.Property = property;
                                transaction2.Commit();
                            }
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("Import table",
                                          String.Format("Error when importing table {0}", dbContainer.Name), ex);
                }
            }

            if (dbType == DatabaseType.StoredProcedure)
                return;

            // Puis on crée les liens
            foreach (DbTable table in dbObjects)
            {
                List<DbRelationShip> relations = schemaDiscover.GetRelations(table);
                foreach (DbRelationShip relation in relations)
                {
                    Entity sourceEntity = FindEntity(relation.SourceTableName);
                    Entity targetEntity = FindEntity(relation.TargetTableName);

                    if (targetEntity == null || sourceEntity == null)
                        continue;

                    if (Association.GetLinks(sourceEntity, targetEntity).Count > 0)
                        continue;

                    using (
                        Transaction transaction =
                            parentPackage.Store.TransactionManager.BeginTransaction("Create relations"))
                    {
                        transaction.Context.ContextInfo.Add(ImportedRelationInfo, relation);

                        Association association = new Association(sourceEntity, targetEntity);
                        association.Sort = AssociationSort.Normal;

                        // Calcul du nom 
                        // Si il n'existe pas d'autres relations avec le même modèle, on
                        // prend le nom du modèle cible
                        if (CountSameRelations(relations, relation) == 1)
                            association.SourceRoleName = targetEntity.Name;
                        else
                            association.SourceRoleName = relation.Name;

                        //On ajoute les propriétés concernées dans la liste des propriétes
                        //liées à l'association
                        for (int idx = 0; idx < relation.SourceColumnNames.Count; idx++)
                        {
                            string sourceColumnName = relation.SourceColumnNames[idx];
                            ForeignKey fk = new ForeignKey(sourceEntity.Store);
                            fk.Column =
                                sourceEntity.Properties.Find(
                                    delegate(Property prop) { return prop.ColumnName == sourceColumnName; });

                            string targetColumnName = relation.TargetColumnNames[idx];
                            fk.PrimaryKey =
                                targetEntity.Properties.Find(
                                    delegate(Property prop) { return prop.ColumnName == targetColumnName; });

                            if (fk.PrimaryKey != null && fk.Column != null)
                                association.ForeignKeys.Add(fk);
                        }

                        transaction.Commit();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Factory pour retourner le SchemaDiscover correspondant à la connection
        /// </summary>
        /// <param name="dataConnection">The data connection.</param>
        /// <returns></returns>
        private static ISchemaDiscover GetSchemaDiscover(IDbConnection dataConnection)
        {
            if (dataConnection is SqlConnection)
                return new SQLServerSchemaDiscover(dataConnection);
            if (dataConnection is OracleConnection)
                return new OracleSchemaDiscover(dataConnection);
            if (dataConnection is OleDbConnection)
                return new OleDbSchemaDiscover(dataConnection);
            return null;
        }

        /// <summary>
        /// Recherche d'une entité par rapport au nom de la table
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        private Entity FindEntity(string tableName)
        {
            foreach (Package package in _layer.Packages)
            {
                Entity targetEntity =
                    package.Types.Find(
                        delegate(DataType type) { return type is Entity && ((Entity) type).TableName == tableName; }) as
                    Entity;

                if (targetEntity != null)
                    return targetEntity;
            }
            return null;
        }

        /// <summary>
        /// Counts the same relations.
        /// </summary>
        /// <param name="relations">The relations.</param>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        private static int CountSameRelations(List<DbRelationShip> relations, DbRelationShip relation)
        {
            int nb = 0;
            foreach (DbRelationShip rel in relations)
            {
                if (rel.TargetTableName == relation.TargetTableName && rel.SourceTableName == relation.SourceTableName)
                    nb++;
            }
            return nb;
        }
    }
}