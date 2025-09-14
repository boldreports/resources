using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReportServer
{

    [DataContract(Name = "Data1", Namespace = "http://schemas.datacontract.org/2004/07/Syncfusion.Server.Base.DataClasses")]
    public class ItemDetail
    {
        [DataMember(Name = "CanRead",Order = 1)]
        public bool CanRead { get; set; }
        [DataMember(Name = "CanWrite", Order = 2)]
        public bool CanWrite { get; set; }
        [DataMember(Name = "CanDelete", Order = 3)]
        public bool CanDelete { get; set; }
        [DataMember(Name = "CanSchedule", Order = 4)]
        public bool CanSchedule { get; set; }
        [DataMember(Name = "CanOpen", Order = 5)]
        public bool CanOpen { get; set; }
        [DataMember(Name = "CanMove", Order = 6)]
        public bool CanMove { get; set; }
        [DataMember(Name = "CanCopy", Order = 7)]
        public bool CanCopy { get; set; }
        [DataMember(Name = "CanClone", Order = 23)]
        public bool CanClone { get; set; }
        [DataMember(Name = "Id", Order = 24)]
        public Guid Id { get; set; }
        [DataMember(Name = "ItemType", Order = 25)]
        public ItemType ItemType { get; set; }
        [DataMember(Name = "Name", Order = 26)]
        public string Name { get; set; }
        [DataMember(Name = "Description", Order = 27)]
        public string Description { get; set; }
        [DataMember(Name = "TrimmedDescription", Order = 28)]
        public string TrimmedDescription
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Description) == false)
                {
                    return Description.Length > 40 ? Description.Substring(0, 40) + "..." : Description;
                }
                return String.Empty;
            }
            set
            {

            }
        }
        [DataMember(Name = "CreatedById", Order = 9)]
        public int CreatedById { get; set; }
        [DataMember(Name = "CreatedByDisplayName", Order = 10)]
        public string CreatedByDisplayName { get; set; }
        [DataMember(Name = "ModifiedById", Order = 11)]
        public int ModifiedById { get; set; }
        [DataMember(Name = "ModifiedByFullName", Order = 12)]
        public string ModifiedByFullName { get; set; }
        [DataMember(Name = "CategoryId", Order = 13)]
        public Guid? CategoryId { get; set; }
        [DataMember(Name = "CategoryName", Order = 14)]
        public string CategoryName { get; set; }
        [DataMember(Name = "CreatedDate", Order = 15)]
        public string CreatedDate { get; set; }
        [DataMember(Name = "ModifiedDate", Order = 16)]
        public string ModifiedDate { get; set; }
        [DataMember(Name = "IsCreatedByActive", Order = 17)]
        public bool IsCreatedByActive { get; set; }
        [DataMember(Name = "IsModifiedByActive", Order = 18)]
        public bool IsModifiedByActive { get; set; }
        [DataMember(Name = "ItemModifiedDate", Order = 19)]
        public DateTime ItemModifiedDate { set; get; }
        [DataMember(Name = "ItemCreatedDate", Order = 20)]
        public DateTime ItemCreatedDate { set; get; }
        [DataMember(Name = "CloneOf", Order = 21)]
        public Guid? CloneOf { get; set; }
        [DataMember(Name = "Extension", Order = 22)]
        public string Extension { get; set; }
    }
    public class ItemRequest
    {
        public Guid ItemId { get; set; }

        public DataSourceMappingInfo DatasourceDetails { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? CategoryId { get; set; }

        public byte[] ItemContent { get; set; }

        public DataSourceDefinition DataSourceDefinition { get; set; }

        public string UploadedReportName { get; set; }

        public List<DataSourceMappingInfo> DataSourceMappingInfo { get; set; }

        public List<DatasetMappingInfo> DatasetMappingInfo { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VersionComment { get; set; }

        public ItemType ItemType { get; set; }

        public string ServerPath { get; set; }

        public bool FavoriteValue { get; set; }

        public bool IsPublic { get; set; }

        public List<string> ReportReferences { get; set; }
    }

    public class ItemResponse
    {
        public bool Status { get; set; }

        public byte[] FileContent { get; set; }

        public string StatusCode { get; set; }

        public string Message { get; set; }

        public string StatusMessage { get; set; }

        public string ItemName { get; set; }

        public ItemType ItemType { get; set; }

        public Guid PublishedItemId { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsPublic { get; set; }

        public object ResponseContent { get; set; }

        public string CategoryName { get; set; }
    }

    public class DataSourceMappingInfo
    {
        public string Name { get; set; }

        public string DataSourceId { get; set; }
    }

    public class DatasetMappingInfo
    {
        public string Name { get; set; }

        public string DatasetId { get; set; }
    }

    [DataContract(Name = "Data", Namespace = "http://schemas.datacontract.org/2004/07/Syncfusion.Server.Base.DataClasses")]
    public class EntityData<T>
    {
        [DataMember(Name = "result", Order = 1)]
        public IEnumerable<T> result { get; set; }
        [DataMember(Name = "count", Order = 2)]
        public int count { get; set; }

    }
    public class ViewerReportApi
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
        public byte[] DatasourceDefinition { get; set; }
        public bool HasDatasources { get; set; }
        public byte[] ReportDefinition { get; set; }
    }

    [DataContract(Name = "Data3", Namespace = "http://schemas.datacontract.org/2004/07/Syncfusion.Server.Base.DataClasses")]
    public class ApiResponse
    {
        [DataMember(Name = "ApiStatus", Order = 1)]
        public bool ApiStatus { get; set; }
        [DataMember(Name = "Data", Order = 2)]
        public EntityData<ItemDetail> Data { get; set; }
    }



    [Serializable]
    public class DataSourceDefinition
    {
        private string extensionField;
        private string connectStringField;
        private bool useOriginalConnectStringField;
        private bool originalConnectStringExpressionBasedField;
        private CredentialRetrievalEnum credentialRetrievalField;
        private bool windowsCredentialsFieldSpecified;
        private bool windowsCredentialsField;
        private bool impersonateUserField;
        private bool impersonateUserFieldSpecified;
        private string promptField;
        private string userNameField;
        private string passwordField;
        private bool enabledField;
        private bool enabledFieldSpecified;
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }
        public string ConnectString
        {
            get
            {
                return connectStringField;
            }
            set
            {
                connectStringField = value;
            }
        }
        public bool UseOriginalConnectString
        {
            get
            {
                return useOriginalConnectStringField;
            }
            set
            {
                useOriginalConnectStringField = value;
            }
        }
        public bool OriginalConnectStringExpressionBased
        {
            get
            {
                return originalConnectStringExpressionBasedField;
            }
            set
            {
                originalConnectStringExpressionBasedField = value;
            }
        }
        public CredentialRetrievalEnum CredentialRetrieval
        {
            get
            {
                return credentialRetrievalField;
            }
            set
            {
                credentialRetrievalField = value;
            }
        }
        public bool WindowsCredentials
        {
            get
            {
                return windowsCredentialsField;
            }
            set
            {
                windowsCredentialsField = value;
            }
        }
        [XmlIgnore()]
        public bool WindowsCredentialsSpecified
        {
            get
            {
                return windowsCredentialsFieldSpecified;
            }
            set
            {
                windowsCredentialsFieldSpecified = value;
            }
        }
        public bool ImpersonateUser
        {
            get
            {
                return impersonateUserField;
            }
            set
            {
                impersonateUserField = value;
            }
        }
        [XmlIgnore()]
        public bool ImpersonateUserSpecified
        {
            get
            {
                return impersonateUserFieldSpecified;
            }
            set
            {
                impersonateUserFieldSpecified = value;
            }
        }
        public string Prompt
        {
            get
            {
                return promptField;
            }
            set
            {
                promptField = value;
            }
        }
        public string UserName
        {
            get
            {
                return userNameField;
            }
            set
            {
                userNameField = value;
            }
        }
        public string Password
        {
            get
            {
                return passwordField;
            }
            set
            {
                passwordField = value;
            }
        }
        public bool Enabled
        {
            get
            {
                return enabledField;
            }
            set
            {
                enabledField = value;
            }
        }
        [XmlIgnore()]
        public bool EnabledSpecified
        {
            get
            {
                return enabledFieldSpecified;
            }
            set
            {
                enabledFieldSpecified = value;
            }
        }
    }

    public enum CredentialRetrievalEnum
    {
        Prompt,
        Store,
        Integrated,
        None,
    }

    [DataContract(Name = "Data5", Namespace = "http://schemas.datacontract.org/2004/07/Syncfusion.Server.Base.DataClasses")]
    public enum ItemType
    {
        [EnumMember]
        [Description("Category")]
        Category = 1,
        [EnumMember]
        [Description("Dashboard")]
        Dashboard,
        [EnumMember]
        [Description("Report")]
        Report,
        [EnumMember]
        [Description("Datasource")]
        Datasource,
        [EnumMember]
        [Description("Dataset")]
        Dataset,
        [EnumMember]
        [Description("File")]
        File,
        [EnumMember]
        [Description("Schedule")]
        Schedule
    }

    public class Token
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires_in { get; set; }

        public string userName { get; set; }

        public string serverUrl { get; set; }

        public string password { get; set; }

    }
}
