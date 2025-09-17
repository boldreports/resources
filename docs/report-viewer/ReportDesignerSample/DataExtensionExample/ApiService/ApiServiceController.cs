using Syncfusion.EJ.ReportViewer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ReportDesignerSample.Service
{
    public class ApiServiceController : ApiController
    {
        public Tuple<string, string, string> GetData()
        {
            var dataSource1 = GetDataSource();
            var dataSource2 = GetDataSource2();
            var dataSet = GetDataSet();
            return new Tuple<string, string, string>(dataSource1, dataSource2, dataSet);
        }

        public string GetDataSource()
        {
            Syncfusion.RDL.DOM.DataSource dataSource = new Syncfusion.RDL.DOM.DataSource();
            dataSource.Name = "DataSource1";
            dataSource.Transaction = false;
            dataSource.SecurityType = Syncfusion.RDL.DOM.SecurityType.DataBase;
            Syncfusion.RDL.DOM.ConnectionProperties connectionProperties = new Syncfusion.RDL.DOM.ConnectionProperties();
            connectionProperties.ConnectString = "Data Source=localhost;Initial Catalog=AdventureWorks;";
            connectionProperties.EmbedCredentials = false;
            connectionProperties.DataProvider = "SQL";
            connectionProperties.IsDesignState = false;
            connectionProperties.IntegratedSecurity = true;
            connectionProperties.UserName = "";
            connectionProperties.PassWord = "";
            connectionProperties.Prompt = "Specify the Username and password for DataSource DataSource1";
            connectionProperties.CustomProperties = null;
            dataSource.ConnectionProperties = connectionProperties;
            var SerializeObject = SerializeDOM(dataSource);
            return SerializeObject;
        }

        public string GetDataSource2()
        {
            Syncfusion.RDL.DOM.DataSource dataSource = new Syncfusion.RDL.DOM.DataSource();
            dataSource.Name = "DataSource2";
            dataSource.Transaction = false;
            dataSource.SecurityType = Syncfusion.RDL.DOM.SecurityType.DataBase;
            Syncfusion.RDL.DOM.ConnectionProperties connectionProperties = new Syncfusion.RDL.DOM.ConnectionProperties();
            connectionProperties.ConnectString = "Data Source=localhost;Initial Catalog=AdventureWorks;";
            connectionProperties.EmbedCredentials = false;
            connectionProperties.DataProvider = "SQL";
            connectionProperties.IsDesignState = false;
            connectionProperties.IntegratedSecurity = true;
            connectionProperties.UserName = "";
            connectionProperties.PassWord = "";
            connectionProperties.Prompt = "Specify the Username and password for DataSource DataSource1";
            connectionProperties.CustomProperties = null;
            dataSource.ConnectionProperties = connectionProperties;
            var SerializeObject = SerializeDOM(dataSource);
            return SerializeObject;
        }


        public string GetDataSet()
        {
            Syncfusion.RDL.DOM.DataSet dataSet = new Syncfusion.RDL.DOM.DataSet();
            dataSet.Name = "DataSet1";
            Syncfusion.RDL.DOM.Fields fields = GetFields();
            dataSet.Fields = fields;
            Syncfusion.RDL.DOM.Query query = GetQueryData();
            dataSet.Query = query;
            query.DataSourceName = "DataSource1";
            dataSet.CaseSensitivity = 0;
            dataSet.Collation = null;
            dataSet.AccentSensitivity = 0;
            dataSet.KanatypeSensitivity = 0;
            dataSet.WidthSensitvity = 0;
            dataSet.Filters = null;
            dataSet.InterpretSubtotalsAsDetails = 0;
            dataSet.DataSetInfo = null;
            dataSet.DataSetObject = null;
            dataSet.SharedDataSet = null;
            var SerializeObject = SerializeDOM(dataSet);
            return SerializeObject;
        }

        public static Syncfusion.RDL.DOM.Fields GetFields()
        {
            Syncfusion.RDL.DOM.Fields fields = new Syncfusion.RDL.DOM.Fields();
            Syncfusion.RDL.DOM.Field fieldCollection = null;
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "EmployeeID",
                DataField = "EmployeeID",
                Value = null,
                TypeName = "System.Int32"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "NationalIDNumber",
                DataField = "NationalIDNumber",
                Value = null,
                TypeName = "System.String"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "ContactID",
                DataField = "ContactID",
                Value = null,
                TypeName = "System.Int32"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "LoginID",
                DataField = "LoginID",
                Value = null,
                TypeName = "System.String"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "ManagerID",
                DataField = "ManagerID",
                Value = null,
                TypeName = "System.Int32"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "Title",
                DataField = "Title",
                Value = null,
                TypeName = "System.String"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "BirthDate",
                DataField = "BirthDate",
                Value = null,
                TypeName = "System.DateTime"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "MaritalStatus",
                DataField = "MaritalStatus",
                Value = null,
                TypeName = "System.String"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "Gender",
                DataField = "Gender",
                Value = null,
                TypeName = "System.String"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "HireDate",
                DataField = "HireDate",
                Value = null,
                TypeName = "System.DateTime"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "SalariedFlag",
                DataField = "SalariedFlag",
                Value = null,
                TypeName = "System.Boolean"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "VacationHours",
                DataField = "VacationHours",
                Value = null,
                TypeName = "System.Int16"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "SickLeaveHours",
                DataField = "SickLeaveHours",
                Value = null,
                TypeName = "System.Int16"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "CurrentFlag",
                DataField = "CurrentFlag",
                Value = null,
                TypeName = "System.Boolean"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "rowguid",
                DataField = "rowguid",
                Value = null,
                TypeName = "System.Guid"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "ModifiedDate",
                DataField = "ModifiedDate",
                Value = null,
                TypeName = "System.DateTime"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "EmployeeAddress_EmployeeID",
                DataField = "EmployeeAddress_EmployeeID",
                Value = null,
                TypeName = "System.Int32"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "AddressID",
                DataField = "AddressID",
                Value = null,
                TypeName = "System.Int32"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "EmployeeAddress_rowguid",
                DataField = "EmployeeAddress_rowguid",
                Value = null,
                TypeName = "System.Guid"
            };
            fields.Add(fieldCollection);
            fieldCollection = new Syncfusion.RDL.DOM.Field()
            {
                Name = "EmployeeAddress_ModifiedDate",
                DataField = "EmployeeAddress_ModifiedDate",
                Value = null,
                TypeName = "System.DateTime"
            };
            fields.Add(fieldCollection);
            return fields;
        }

        public static Syncfusion.RDL.DOM.Query GetQueryData()
        {
            Syncfusion.RDL.DOM.Query query = new Syncfusion.RDL.DOM.Query();
            query.CommandText = "SELECT [HumanResources].[Employee].[EmployeeID],[HumanResources].[Employee].[NationalIDNumber],[HumanResources].[Employee].[ContactID],[HumanResources].[Employee].[LoginID],[HumanResources].[Employee].[ManagerID],[HumanResources].[Employee].[Title],[HumanResources].[Employee].[BirthDate],[HumanResources].[Employee].[MaritalStatus],[HumanResources].[Employee].[Gender],[HumanResources].[Employee].[HireDate],[HumanResources].[Employee].[SalariedFlag],[HumanResources].[Employee].[VacationHours],[HumanResources].[Employee].[SickLeaveHours],[HumanResources].[Employee].[CurrentFlag],[HumanResources].[Employee].[rowguid],[HumanResources].[Employee].[ModifiedDate],[HumanResources].[EmployeeAddress].[EmployeeID] AS[EmployeeAddress_EmployeeID],[HumanResources].[EmployeeAddress].[AddressID],[HumanResources].[EmployeeAddress].[rowguid] AS[EmployeeAddress_rowguid],[HumanResources].[EmployeeAddress].[ModifiedDate] AS[EmployeeAddress_ModifiedDate] FROM[HumanResources].[Employee] INNER JOIN[HumanResources].EmployeeAddress ON[HumanResources].[Employee].[EmployeeID]=[HumanResources].[EmployeeAddress].[EmployeeID]";
            query.CommandType = Syncfusion.RDL.DOM.CommandType.Text;
            Syncfusion.RDL.DOM.QueryDesignerState queryDesignerState = new Syncfusion.RDL.DOM.QueryDesignerState();
            queryDesignerState.Expressions = null;
            queryDesignerState.Filters = null;
            queryDesignerState.StoredProcedure = null;
            List<Syncfusion.RDL.DOM.Table> tables = new List<Syncfusion.RDL.DOM.Table>();
            Syncfusion.RDL.DOM.Table firsttable = new Syncfusion.RDL.DOM.Table();
            firsttable.Name = "Employee";
            firsttable.Schema = "HumanResources";
            List<Syncfusion.RDL.DOM.Column> columns = GetFirstTableColumnData();
            firsttable.Columns = columns;
            List<Syncfusion.RDL.DOM.SchemaInfo> schemaInfos = GetFirstTableSchemaInfos();
            firsttable.SchemaLevels = schemaInfos;
            tables.Add(firsttable);
            Syncfusion.RDL.DOM.Table secondtable = new Syncfusion.RDL.DOM.Table();
            secondtable.Name = "EmployeeAddress";
            secondtable.Schema = "HumanResources";
            List<Syncfusion.RDL.DOM.Column> secondtablecolumns = GetSecondTableColumnData();
            secondtable.Columns = secondtablecolumns;
            List<Syncfusion.RDL.DOM.SchemaInfo> secondtableschemaInfos = GetSecondTableSchemaInfos();
            secondtable.SchemaLevels = secondtableschemaInfos;
            tables.Add(secondtable);
            queryDesignerState.Tables = tables;
            List<Syncfusion.RDL.DOM.Join> joins = new List<Syncfusion.RDL.DOM.Join>();
            Syncfusion.RDL.DOM.Join join = new Syncfusion.RDL.DOM.Join();
            join.LeftTable = "Employee";
            join.RightTable = "EmployeeAddress";
            join.JoinType = "Inner";
            join.PrimaryKeyOwner = "Employee";
            List<Syncfusion.RDL.DOM.JoinField> joinFields = GetJoinFields();
            join.JoinFields = joinFields;
            joins.Add(join);
            queryDesignerState.Joins = joins;
            List<Syncfusion.RDL.DOM.QueryFilter> filters = new List<Syncfusion.RDL.DOM.QueryFilter>();
            Syncfusion.RDL.DOM.QueryFilter filter = new Syncfusion.RDL.DOM.QueryFilter();
            filter.Name = "EmployeeID";
            filter.DataType = "System.Int32";
            filter.Operator = "=";
            filter.IsQueryParameter = false;
            filter.TableName = "Employee";
            filter.Schema = "HumanResources";
            Syncfusion.RDL.DOM.QueryFilterValue queryFilterValue = new Syncfusion.RDL.DOM.QueryFilterValue();
            queryFilterValue.Value1 = '1';
            filter.Value = queryFilterValue;
            Syncfusion.RDL.DOM.QueryParameterName queryParameterName = new Syncfusion.RDL.DOM.QueryParameterName();
            queryParameterName.Parameter1 = "None";
            filter.ParameterName = queryParameterName;
            filters.Add(filter);
            queryDesignerState.Filters = filters;
            query.QueryDesignerState = queryDesignerState;
            return query;
        }

        public static List<Syncfusion.RDL.DOM.JoinField> GetJoinFields()
        {
            List<Syncfusion.RDL.DOM.JoinField> joinFields = new List<Syncfusion.RDL.DOM.JoinField>();
            Syncfusion.RDL.DOM.JoinField joinField = null;
            joinField = new Syncfusion.RDL.DOM.JoinField()
            {
                LeftField = "EmployeeID",
                RightField = "EmployeeID",
                OperatorType = "="
            };
            joinFields.Add(joinField);
            return joinFields;
        }

        public static List<Syncfusion.RDL.DOM.SchemaInfo> GetFirstTableSchemaInfos()
        {
            List<Syncfusion.RDL.DOM.SchemaInfo> SchemaInfos = new List<Syncfusion.RDL.DOM.SchemaInfo>();
            Syncfusion.RDL.DOM.SchemaInfo schemaInfo = null;
            schemaInfo = new Syncfusion.RDL.DOM.SchemaInfo()
            {
                Name = "HumanResources",
                SchemaType = Syncfusion.RDL.DOM.SchemaType.Schema
            };
            SchemaInfos.Add(schemaInfo);
            schemaInfo = new Syncfusion.RDL.DOM.SchemaInfo()
            {
                Name = "Tables",
                SchemaType = Syncfusion.RDL.DOM.SchemaType.Category
            };
            SchemaInfos.Add(schemaInfo);
            schemaInfo = new Syncfusion.RDL.DOM.SchemaInfo()
            {
                Name = "Employee",
                SchemaType = Syncfusion.RDL.DOM.SchemaType.Table
            };
            SchemaInfos.Add(schemaInfo);
            return SchemaInfos;
        }

        public static List<Syncfusion.RDL.DOM.Column> GetFirstTableColumnData()
        {
            List<Syncfusion.RDL.DOM.Column> Columns = new List<Syncfusion.RDL.DOM.Column>();
            Syncfusion.RDL.DOM.Column column = null;
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "EmployeeID",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "NationalIDNumber",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "ContactID",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "LoginID",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "ManagerID",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "Title",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "BirthDate",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "MaritalStatus",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "Gender",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "HireDate",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "SalariedFlag",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "VacationHours",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "SickLeaveHours",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "CurrentFlag",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "rowguid",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "ModifiedDate",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            return Columns;
        }

        public static List<Syncfusion.RDL.DOM.Column> GetSecondTableColumnData()
        {
            List<Syncfusion.RDL.DOM.Column> Columns = new List<Syncfusion.RDL.DOM.Column>();
            Syncfusion.RDL.DOM.Column column = null;
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "EmployeeID",
                AliasName = "EmployeeAddress_EmployeeID",
                IsDuplicate = "True",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "AddressID",
                IsDuplicate = "False",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "rowguid",
                AliasName = "EmployeeAddress_rowguid",
                IsDuplicate = "True",
                IsSelected = "True"
            };
            Columns.Add(column);
            column = new Syncfusion.RDL.DOM.Column()
            {
                Name = "ModifiedDate",
                AliasName = "EmployeeAddress_ModifiedDate",
                IsDuplicate = "True",
                IsSelected = "True"
            };
            Columns.Add(column);
            return Columns;
        }

        public static List<Syncfusion.RDL.DOM.SchemaInfo> GetSecondTableSchemaInfos()
        {
            List<Syncfusion.RDL.DOM.SchemaInfo> SchemaInfos = new List<Syncfusion.RDL.DOM.SchemaInfo>();
            Syncfusion.RDL.DOM.SchemaInfo schemaInfo = null;
            schemaInfo = new Syncfusion.RDL.DOM.SchemaInfo()
            {
                Name = "HumanResources",
                SchemaType = Syncfusion.RDL.DOM.SchemaType.Schema
            };
            SchemaInfos.Add(schemaInfo);
            schemaInfo = new Syncfusion.RDL.DOM.SchemaInfo()
            {
                Name = "Tables",
                SchemaType = Syncfusion.RDL.DOM.SchemaType.Category
            };
            SchemaInfos.Add(schemaInfo);
            schemaInfo = new Syncfusion.RDL.DOM.SchemaInfo()
            {
                Name = "EmployeeAddress",
                SchemaType = Syncfusion.RDL.DOM.SchemaType.Table
            };
            SchemaInfos.Add(schemaInfo);
            return SchemaInfos;
        }
        public string SerializeDOM(object jsonData)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer(new RDLTypeResolver());
            string jsonObject = javaScriptSerializer.Serialize(jsonData);
            return jsonObject;
        }
    }
}