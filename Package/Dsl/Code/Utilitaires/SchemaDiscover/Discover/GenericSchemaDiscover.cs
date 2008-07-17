using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GenericSchemaDiscover : ISchemaDiscover
    {
        /// <summary>
        /// 
        /// </summary>
        protected IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSchemaDiscover"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public GenericSchemaDiscover(IDbConnection connection)
        {
            this.connection = connection;
        }

        #region ISchemaDiscover Members

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <param name="proc">The proc.</param>
        /// <returns></returns>
        public virtual List<DbColumn> GetColumns(DbStoredProcedure proc)
        {
            // Lecture des paramètres de la procédure
            string sqlFmt =
                @"SELECT r.SPECIFIC_SCHEMA, r.SPECIFIC_NAME, p.ORDINAL_POSITION, p.PARAMETER_NAME
                FROM INFORMATION_SCHEMA.ROUTINES AS r FULL OUTER JOIN
                      INFORMATION_SCHEMA.PARAMETERS AS p ON r.SPECIFIC_NAME = p.SPECIFIC_NAME AND r.SPECIFIC_SCHEMA = p.SPECIFIC_SCHEMA
                WHERE (r.ROUTINE_TYPE = N'PROCEDURE') AND (ISNULL(OBJECTPROPERTY(OBJECT_ID(r.SPECIFIC_NAME), 'IsMSShipped'), 0) = 0)
                    AND (r.SPECIFIC_NAME = N'{1}') AND (r.SPECIFIC_SCHEMA = N'{0}')
                ORDER BY r.SPECIFIC_SCHEMA, r.SPECIFIC_NAME, p.ORDINAL_POSITION";

            IDbCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = string.Format("[{0}].[{1}]", proc.Owner, proc.Name);

            // Préparation de la commande simulé qui va servir à récupérer
            // le schéma du résultat.
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = String.Format(sqlFmt, proc.Owner, proc.Name);
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["ORDINAL_POSITION"] != DBNull.Value)
                    {
                        string parameterName = ((string) reader["PARAMETER_NAME"]).Substring(1);
                        DbParameter dbParm = new DbParameter();
                        dbParm.Name = parameterName;

                        SqlDbType sqlDbType = GetFieldValue<SqlDbType>(reader["ProviderType"], SqlDbType.NVarChar);
                        dbParm.ClrType = GetFieldValue<Type>(reader["DataType"], null);
                        int size = GetFieldValue<int>(reader["CHARACTER_MAXIMUM_LENGTH"], 0);
                        int precision = GetFieldValue<int>(reader["NUMERIC_PRECISION"], -1);
                        int scale = GetFieldValue<int>(reader["NUMERIC_SCALE"], -1);
                        dbParm.ServerType = GetSqlTypeDeclaration(sqlDbType, size, precision, scale, false, false);
                        switch (((string) reader["PARAMETER_MODE"]))
                        {
                            case "IN":
                                dbParm.ParameterDirection = ParameterDirection.Input;
                                break;

                            case "INOUT":
                                dbParm.ParameterDirection = ParameterDirection.InputOutput;
                                break;

                            case "OUT":
                                dbParm.ParameterDirection = ParameterDirection.Output;
                                break;

                            default:
                                dbParm.ParameterDirection = ParameterDirection.Input;
                                break;
                        }
                        proc.Parameters.Add(dbParm);

                        SqlParameter parm = new SqlParameter(parameterName, null);
                        command.Parameters.Add(parm);
                    }
                }
                reader.Close();
            }

            using (IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                return GetColumnsFromReader(proc, reader);
            }
        }

        /// <summary>
        /// Récupére les colonnes d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        /// <remarks>
        /// Ne doit pas mettre à jour directement les colonnes dans la table
        /// </remarks>
        public virtual List<DbColumn> GetColumns(DbTable table)
        {
            IDbCommand cmd = connection.CreateCommand();
            // cmd.Connection = connection as SqlConnection;
            string tableName = table.Name;
            if (tableName.IndexOf(' ') >= 0)
                tableName = '[' + tableName + ']';
            cmd.CommandText = String.Concat("SELECT * FROM ", table.Owner, ".", tableName);
            using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
            {
                return GetColumnsFromReader(table, reader);
            }
        }

        /// <summary>
        /// Liste des index d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public abstract List<DbIndex> GetIndexes(DbTable table);

        /// <summary>
        /// Liste des relations à partir et vers une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public abstract List<DbRelationShip> GetRelations(DbTable table);

        #endregion

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected T GetFieldValue<T>(object value, T defaultValue)
        {
            if (value != DBNull.Value && value != null)
                return (T) value;
            return defaultValue;
        }

        /// <summary>
        /// Gets the columns from reader.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private List<DbColumn> GetColumnsFromReader(DbContainer parent, IDataReader reader)
        {
            List<DbColumn> columns = new List<DbColumn>();
            //Retrieve column schema into a DataTable.
            DataTable schemaTable = reader.GetSchemaTable();
            if (schemaTable == null)
                return columns;

            //For each field in the table...
            foreach (DataRow field in schemaTable.Rows)
            {
                DbColumn col = new DbColumn();
                col.Name = (string) field["ColumnName"];
                col.ClrType = GetFieldValue<Type>(field["DataType"], null);
                col.IsNullable = GetFieldValue<bool>(field["AllowDBNull"], false);
                col.InPrimaryKey = GetFieldValue<bool>(field["IsKey"], false);
                SqlDbType sqlDbType = GetFieldValue<SqlDbType>(field["ProviderType"], SqlDbType.NVarChar);

                //try
                //{                        
                //    //col.ServerType = (string)field["DataTypeName"];
                //}
                //catch
                //{
                //    // N'existe pas pour Oracle
                //    col.ServerType = String.Empty;
                //}
                try
                {
                    col.IsAutoIncrement = GetFieldValue<bool>(field["IsAutoIncrement"], false);
                }
                catch
                {
                }
                col.InUniqueKey = GetFieldValue<bool>(field["IsUnique"], false);
                col.Length = GetFieldValue<int>(field["ColumnSize"], 0);
                col.Precision = GetFieldValue<Int16>(field["NumericPrecision"], 0);
                col.Scale = GetFieldValue<Int16>(field["NumericScale"], 0);
                col.Parent = parent;
                columns.Add(col);
                col.ServerType =
                    GetSqlTypeDeclaration(sqlDbType, col.Length, col.Precision, col.Scale, col.IsNullable,
                                          col.IsAutoIncrement);
            }
            return columns;
        }

        /// <summary>
        /// Gets the SQL type declaration.
        /// </summary>
        /// <param name="sqlDbType">Type of the SQL db.</param>
        /// <param name="length">The length.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        /// <param name="isAutoIncrement">if set to <c>true</c> [is auto increment].</param>
        /// <returns></returns>
        protected virtual string GetSqlTypeDeclaration(SqlDbType sqlDbType, int length, int precision, int scale,
                                                       bool isNullable, bool isAutoIncrement)
        {
            StringBuilder sb = new StringBuilder();
            if (sqlDbType == SqlDbType.Timestamp)
            {
                sb.Append("ROWVERSION");
            }
            else
            {
                sb.Append(sqlDbType.ToString());
            }

            if (sqlDbType == SqlDbType.Binary
                || sqlDbType == SqlDbType.Char
                || sqlDbType == SqlDbType.NChar
                || sqlDbType == SqlDbType.NVarChar
                || sqlDbType == SqlDbType.VarBinary
                || sqlDbType == SqlDbType.VarChar)
            {
                sb.AppendFormat("({0})", length);
            }
            else if (sqlDbType == SqlDbType.Decimal)
            {
                sb.AppendFormat("({0},{1})", precision, scale);
            }

            //if (!isNullable)
            //{
            //    sb.Append(" NOT NULL");
            //}

            //if (isAutoIncrement)
            //{
            //    if (sqlDbType == SqlDbType.UniqueIdentifier)
            //    {
            //        sb.Append(" ROWGUIDCOL");
            //    }
            //    else
            //    {
            //        sb.Append(" IDENTITY");
            //    }
            //}
            return sb.ToString();
        }
    }
}