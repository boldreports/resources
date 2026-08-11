using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ExternalRptServer.Data
{
    public class SQLDataHelper
    {
        private static SQLDataHelper _sqlHelper;

        public const string ConnectionString = @"Data Source=localhost;Initial Catalog=ExternalServer;Integrated Security=SSPI;";

        public SQLDataHelper()
        {

        }

        public static SQLDataHelper DataInstance
        {
            get
            {
                if (_sqlHelper == null)
                {
                    _sqlHelper = new SQLDataHelper();
                }
                return _sqlHelper;
            }
        }


        public DataTable GetData(string query)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oCmd = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(oCmd))
                    {
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        return dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return null;
        }

        public bool SetData(string query)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oCmd = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    return oCmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            return false;
        }

        public bool SetReportData(string category, string rptName, byte[] rptData)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'Folder' and [Path] = '" + category + "'";
            var data = this.GetData(query);
            var reports = new List<string>();
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "' and Name = '" + rptName+ "'";
                var reportData = this.GetData(rptQuery);
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    var rowId = data.Rows[0]["Id"].ToString();
                    var updateRpt = "UPDATE [ExternalServer].[dbo].[Category_Data] Set [Data] ='"+ System.Text.Encoding.UTF8.GetString(rptData) +"' where Id = '" + rowId + "'";
                    return this.SetData(updateRpt);
                }
                else
                {
                    var insertRpt = "INSERT INTO [ExternalServer].[dbo].[Category_Data]([Id],[Data], [ItemType],[Name]) VALUES('" + itemId + "','" + System.Text.Encoding.UTF8.GetString(rptData) + "','Report','" + rptName + "')";
                    return this.SetData(insertRpt);
                }

            }
            return false;
        }

        public List<string> GetCategories()
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'Folder'";
            var data = this.GetData(query);
            var folders = new List<string>();
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    folders.Add(row["Name"].ToString());
                }
            }
            return folders;
        }

        public List<string> GetReports(string category)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'Folder' and [Path] = '" + category + "'";
            var data = this.GetData(query);
            var reports = new List<string>();
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "'";
                var reportData = this.GetData(rptQuery);
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    foreach (DataRow row in reportData.Rows)
                    {
                        reports.Add(row["Name"].ToString());
                    }
                }
            }
            return reports;
        }

        public List<string> GetDataSources()
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'DataSource'";
                        var data = this.GetData(query);
            var dataSources = new List<string>();
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var dataQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "'";
                var reportData = this.GetData(dataQuery);
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    foreach (DataRow row in reportData.Rows)
                    {
                        dataSources.Add(row["Name"].ToString());
                    }
                }
            }
            return dataSources;
        }

        public List<string> GetDataSets()
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'DataSet'";
            var data = this.GetData(query);
            var dataSources = new List<string>();
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var dataQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "'";
                var reportData = this.GetData(dataQuery);
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    foreach (DataRow row in reportData.Rows)
                    {
                        dataSources.Add(row["Name"].ToString());
                    }
                }
            }
            return dataSources;
        }

        public byte[] GetReport(string category, string rptName)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'Folder' and [Name] = '" + category + "'";
            var data = this.GetData(query);
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "' and Name ='" + rptName + "'";
                var rptData = this.GetData(rptQuery);
                if (rptData != null && rptData.Rows.Count > 0)
                {
                    return System.Text.Encoding.UTF8.GetBytes(rptData.Rows[0]["Data"].ToString());
                }
            }
            return null;
        }

        public byte[] GetDataSource(string dataSourceName)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'DataSource'";
            var data = this.GetData(query);
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "' and Name ='" + dataSourceName + "'";
                var rptData = this.GetData(rptQuery);
                if (rptData != null && rptData.Rows.Count > 0)
                {
                    return System.Text.Encoding.UTF8.GetBytes(rptData.Rows[0]["Data"].ToString());
                }
            }
            return null;
        }

        public byte[] GetDataSet(string dataSetName)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'DataSet'";
            var data = this.GetData(query);
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "' and Name ='" + dataSetName + "'";
                var rptData = this.GetData(rptQuery);
                if (rptData != null && rptData.Rows.Count > 0)
                {
                    return System.Text.Encoding.UTF8.GetBytes(rptData.Rows[0]["Data"].ToString());
                }
            }
            return null;
        }

        public bool SetDataSource(string name, byte[] rptData)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'DataSource'";
            var data = this.GetData(query);
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "' and Name = '" + name + "'";
                var reportData = this.GetData(rptQuery);
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    var rowId = data.Rows[0]["Id"].ToString();
                    var updateRpt = "UPDATE [ExternalServer].[dbo].[Category_Data] Set [Data] ='" + System.Text.Encoding.UTF8.GetString(rptData) + "' where Id = '" + rowId + "'";
                    return this.SetData(updateRpt);
                }
                else
                {
                    var insertRpt = "INSERT INTO [ExternalServer].[dbo].[Category_Data]([Id],[Data], [ItemType],[Name]) VALUES('" + itemId + "','" + System.Text.Encoding.UTF8.GetString(rptData) + "','DataSource','" + name + "')";
                    return this.SetData(insertRpt);
                }

            }
            return false;
        }

        public bool SetDataSet(string name, byte[] rptData)
        {
            var query = "select * from [ExternalServer].[dbo].[Category] where [ItemType] = 'DataSet'";
            var data = this.GetData(query);
            if (data != null && data.Rows.Count > 0)
            {
                var itemId = data.Rows[0]["Id"].ToString();
                var rptQuery = "select * from [ExternalServer].[dbo].[Category_Data] where Id = '" + itemId + "' and Name = '" + name + "'";
                var reportData = this.GetData(rptQuery);
                if (reportData != null && reportData.Rows.Count > 0)
                {
                    var rowId = data.Rows[0]["Id"].ToString();
                    var updateRpt = "UPDATE [ExternalServer].[dbo].[Category_Data] Set [Data] ='" + System.Text.Encoding.UTF8.GetString(rptData) + "' where Id = '" + rowId + "'";
                    return this.SetData(updateRpt);
                }
                else
                {
                    var insertRpt = "INSERT INTO [ExternalServer].[dbo].[Category_Data]([Id],[Data], [ItemType],[Name]) VALUES('" + itemId + "','" + System.Text.Encoding.UTF8.GetString(rptData) + "','DataSet','" + name + "')";
                    return this.SetData(insertRpt);
                }

            }
            return false;
        }
    }
}