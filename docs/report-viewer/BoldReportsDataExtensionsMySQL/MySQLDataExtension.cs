using BoldReports.Data;
using BoldReports.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace BoldReports.DataExtensions.MySQL
{
    public class MySQLDataExtension : BoldReports.Data.IDataExtension
    {
        private Dictionary<string, object> _customProperties;

        public BoldReports.Data.ConnectionProperties ConnectionProperties
        {
            get;
            set;
        }

        public BoldReports.Data.Command Command
        {
            get;
            set;
        }

        public RDL.DOM.DataSource DataSource
        {
            get;
            set;
        }

        public RDL.DOM.DataSet DataSet
        {
            get;
            set;
        }

        public ExtensionHelper Helper
        {
            get;
            set;
        }

        public Dictionary<string, object> CustomProperties
        {
            get
            {
                return this._customProperties;
            }
            set
            {
                this._customProperties = value;
            }
        }

        public bool IsDesignerMode { get; set; }

        public MySQLDataExtension()
        {
        }

        public string GetCommandText(QueryBuilderDesignInfo designInfo, out string error)
        {
            throw new NotImplementedException();
        }

        public object GetData(out string error)
        {
            var connectionString = this.GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    error = string.Empty;
                    connection.Open();
                    DataTable dataTable = this.GetTable(connection, this.Command.Text);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public SchemaData GetDataSourceSchema(SchemaDataInfo schemaData, out string error)
        {
            throw new NotImplementedException();
        }

        public object GetQuerySchema(out string error)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(GetConnectionString()))
            {
                try
                {
                    error = string.Empty;
                    DataTable dtResult = new DataTable();
                    sqlConn.Open();
                    using (MySqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = this.Command.Text;
                        this.UpdateParameters(command);
                        MySqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult);
                        dtResult.Load(reader);
                        return dtResult;
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    throw new DataException(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
            return null;
        }

        public bool TestConnection(out string error)
        {
            MySqlConnection connection = new MySqlConnection(this.GetConnectionString());
            try
            {
                error = string.Empty;
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private string GetConnectionString()
        {
            if (this.ConnectionProperties.IntegratedSecurity)
            {
                return this.ConnectionProperties.ConnectionString + ";Integrated Security=true;";
            }
            else if (!string.IsNullOrEmpty(this.ConnectionProperties.UserName) && !string.IsNullOrEmpty(this.ConnectionProperties.Password))
            {
                return this.ConnectionProperties.ConnectionString + "; User ID=" + this.ConnectionProperties.UserName + "; Password=" + this.ConnectionProperties.Password;
            }
            return this.ConnectionProperties.ConnectionString;
        }

        private DataTable GetTable(MySqlConnection connection, string commandText)
        {
            using (MySqlCommand command = new MySqlCommand(commandText, (MySqlConnection)connection))
            {
                this.UpdateParameters(command);
                using (MySqlDataReader npgSqlDataReader = command.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(npgSqlDataReader);
                    return table;
                }
            }
        }

        void UpdateParameters(MySqlCommand command)
        {
            if (this.Command.Parameters != null && this.Command.Parameters.Count > 0)
            {
                foreach (var queryParameter in this.Command.Parameters)
                {
                    command.Parameters.AddWithValue(queryParameter.Name, queryParameter.Value);
                }
            }
        }

        public Dictionary<string, SchemaData> GetTableColumns(List<SchemaDataInfo> schemaData, out string error)
        {
            throw new NotImplementedException();
        }

        public List<Joiner> ValidateAutoJoin(List<string> tableNames, out string error)
        {
            throw new NotImplementedException();
        }

        public RemoveTableData RemoveQueryTable(QueryJoinerInfo joinerInfo, out string error)
        {
            throw new NotImplementedException();
        }

        public List<Joiner> ValidateTableRelations(List<Joiner> joins, out string error)
        {
            throw new NotImplementedException();
        }
    }
}
