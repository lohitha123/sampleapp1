using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Altostratus;

namespace Smartdocs.SQLite
{
    #region DataModel
    // DataModel is the roll-up class that is instantiated once for the app and obtained through
    // App.DataModel. Its public properties contain what is needed by the view models.
    //
    // Note that all data synchronization methods are in sync.cs, hence the partial class

    public partial class DataModel : BindableBase
    {
        //Task<SyncResult> syncTask = null;
        CancellationTokenSource syncTokenSource;
        CancellationToken syncToken;

        public GroupList GroupedItems { get; private set; }
        public Configuration Configuration { get; private set; }
        public DataAccessLayer db { get; private set; }


        public DataModel(DataAccessLayer dbInit = null)
        {
            // The optional dbInit argument is allows the DBInitialize console application to bypass
            // the Xamarin.Forms DependencyService code by creating the DB directly and pass that in here.
            // The mobile client, on the other hand, lets the Database constructor do that work (see database.cs).
            if (dbInit == null)
            {
                //Constructor is responsible for instantiating the data model.
                db = new DataAccessLayer();
            }
            else
            {
                db = dbInit;
            }
        }

        public async Task InitAsync()
        {
            Configuration = new Configuration();
            await Configuration.InitAsync();

            // The WebAPI needs a token provider, which is the UserPreferences object we now have.
            // WebAPI.Initialize(Configuration.CurrentUser);

            // Build the ListView data source from the database. This always uses the existing
            // database as sync with then backend happens later and the data source is updated then.             
            GroupedItems = new GroupList();
            await PopulateGroupedItemsFromDB();

            // Synchronization calls are made by users of the data model.            
        }


        public async Task PopulateGroupedItemsFromDB()
        {
            // Generates the grouped list of active categories as needed by the Xamarin.Forms ListView, 
            // limited by the user's conversation limit setting because there could be more items in the
            // database above the limit that we haven't cleaned up yet.            
            GroupedItems.Clear();

            var categories = Configuration.GetCategories(CategorySet.Active);
            IEnumerable<Item> list;

            foreach (String c in categories)
            {
                list = await db.GetItemListForCategoryAsync(c, Configuration.CurrentUser.ConversationLimit);
                Group group = new Group(0, c, list);
                GroupedItems.Add(group);
            }
        }
    }
    #endregion

    #region Items
    // Item is what we store in our database; FeedItem is aligned with what
    // comes from web requests and is just an intermediary.

    public class Item
    {
        [PrimaryKey]
        public String Url { get; set; }
        public DateTime LastUpdated { get; set; }
        [MaxLength(512)]
        public String Title { get; set; }
        [MaxLength(128)]
        public String Description { get; set; }

        public String Body { get; set; }
        [MaxLength(100)]
        public String Provider { get; set; }
        [MaxLength(100)]
        public String Category { get; set; }
    }
    #endregion

    #region Timestamp
    //Timestamp is used to mark when we last synced.
    public class Timestamp
    {
        [PrimaryKey]
        public String Stamp { get; set; }
    }

    #endregion

    #region GroupsAndCategories
    // A group is a collection of items with a name; the ListView in the UI
    // works with a collection of Groups (a GroupList).
    public class Group : ObservableCollection<Item>
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }

        public Group()
        {
        }

        //Override to handle converting an IEnumerable<Item> to a Group
        public Group(Int32 ID, String Name, IEnumerable<Item> list) : base(list)
        {
            this.ID = ID;
            this.Name = Name;
        }
    }

    public class GroupList : ObservableCollection<Group> { }

    public enum CategorySet
    {
        All = 0,
        Active
    }

    public enum CategoryKeepActive
    {
        Keep = 0,
        Force
    }

    public class Category : BindableBase
    {
        String name;
        Boolean isActive;

        public Category()
        {
        }

        [PrimaryKey]
        public String Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public Boolean IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value); }
        }
    }

    public class CategoryList : ObservableCollection<Category> { }
    #endregion


    #region UserInfo
    public class UserInfo
    {
        [PrimaryKey]
        public string userID { get; set; }

        public string password { get; set; }
        public string portalURL { get; set; }
        public string companyID { get; set; }
        public string token { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetUserInfoAsync(this);
        }
    }
    #endregion

    #region DocumentType
    public class DocumentType
    {
        [PrimaryKey]
        public string docTypeName { get; set; }

        public string DocTypeDesc { get; set; }
        public string SignaturePad { get; set; }
        public string WorkItemOnly { get; set; }
        public string Label { get; set; }
        public string SubLabel { get; set; }
        public string IconName { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetDocumentTypeAsync(this);
        }

        public async Task Get()
        {
            await DataAccessLayer.Current.PullDocType();
        }
    }
    #endregion

    #region DocumentField
    public class DocumentField
    {
        //[PrimaryKey]
        public string docTypeName { get; set; }

        public string FieldName { get; set; }
        public string LineItemType { get; set; }
        public string PossibleValues { get; set; }
        public string DataType { get; set; }
        public string Length { get; set; }
        public string Mandatory { get; set; }
        public string Order { get; set; }
        public string BarcodeField { get; set; }
        public string Label { get; set; }
        public string VisibleLength { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetDocumentFieldAsync(this);
        }
    }
    #endregion

    #region WorkitemAdminData
    public class WorkitemAdminData
    {
        [PrimaryKey]
        public string WorkitemID { get; set; }
        public string DocID { get; set; }

        public string DocumentType { get; set; }
        public string SAPWorkItem { get; set; }
        public string Mode { get; set; }
        public string WorkitemTitle { get; set; }
        public string WorkitemDate { get; set; }
        public string Status { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetWorkitemAdminData(this);
        }
    }
    #endregion

    #region HeaderData
    public class SQLHeaderData
    {
        [PrimaryKey]
        public string WorkitemID { get; set; }
        public string DocID { get; set; }

        public string Advance_amount { get; set; }
        public string Company_name { get; set; }
        public string Other_deductions { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks1 { get; set; }
        public string Nature_of_work { get; set; }
        public string Retention { get; set; }
        public string Fund_Centre_Name { get; set; }
        public string Project_site { get; set; }
        public string WBS_Element { get; set; }
        public string Cost_centre_No { get; set; }
        public string Date { get; set; }
        public string Invoice_Value { get; set; }
        public string Payment_terms1 { get; set; }
        public string GL_Name { get; set; }
        public string GL_Cmtmt_item { get; set; }
        public string Vendor_Code { get; set; }
        public string Payment_terms3 { get; set; }
        public string Net_amount_to_be_paid { get; set; }
        public string Payment_terms2 { get; set; }
        public string Invoice_No { get; set; }
        public string Company_Code { get; set; }
        public string Budgeted_Amount { get; set; }
        public string Plant { get; set; }
        public string TDS_amount { get; set; }
        public string Budget_Utilized { get; set; }
        public string Vendor_Name { get; set; }
        public string Remarks3 { get; set; }
        public string Invoice_Date { get; set; }
        public string Agmt_Work_order_No { get; set; }
        public string Material_Service_status { get; set; }
        public string Cost_centre_Name { get; set; }
        public string Fund_Centre_No { get; set; }
        public string Network_Activity_No { get; set; }
        public string Inspection_Status { get; set; }
        public string Proforma_Invoice_No { get; set; }
        public string Project_Site { get; set; }
        public string Proforma_Invoice_Value { get; set; }
        public string Reference_No { get; set; }
        public string Net_Amount_to_be_paid { get; set; }
        public string Advance_Amount { get; set; }
        public string Cost_Centre_Name { get; set; }
        public string Inspected_by { get; set; }
        public string Network_activity_No { get; set; }
        public string Doc_Type { get; set; }
        public string Purchase_Group { get; set; }
        public string PO_Value { get; set; }
        public string Nature_Of_Contract { get; set; }
        public string Warranty { get; set; }
        public string Frieght_scope { get; set; }
        public string Project_Common_Infra { get; set; }
        public string Insurance_scope { get; set; }
        public string Purchase_Document_Type { get; set; }
        public string Vendor { get; set; }
        public string PO_Date { get; set; }
        public string Purchasing_Type { get; set; }
        public string Purchasing_Group { get; set; }
        public string Project { get; set; }
        public string Purchase_Order { get; set; }
        public string Work_Description { get; set; }
        public string GRN_Type { get; set; }
        public string PO_Number { get; set; }
        public string Mtrl_Work_Status2 { get; set; }
        public string Invoice_DC_JMS_No { get; set; }
        public string Mtrl_Work_Status1 { get; set; }
        public string Invoice_DC_JMS_Date { get; set; }
        public string Advance_paid { get; set; }
        public string WTG_Model { get; set; }
        public string Reference_Id { get; set; }
        public string Scope_of_Work_BUIL { get; set; }
        public string Site_Name { get; set; }
        public string WTG_Capacity { get; set; }
        public string WTG_Quantity { get; set; }
        public string WTG_Make { get; set; }
        public string Scope_of_Work_SPV { get; set; }
        public string SPV_Name { get; set; }
        public string State { get; set; }
        public string Scope_of_Work_MEIL { get; set; }
        public string Capacity { get; set; }
        public string Phase { get; set; }

        public string Currency { get; set; }
        public string Details { get; set; }
        public string Material_code { get; set; }

        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string SmartDocID { get; set; }
        public string CreateDate { get; set; }
        public string Comments { get; set; }

        public string Kind { get; set; }

        public string Due_Date_As_Per_PO { get; set; }

        public string Retention_Doc_No { get; set; }

        //Coromandal Headers
        //  public string SBU_DES { get; set; }
        //  public string LOCATION_DES { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Total_Amount { get; set; }
        public string Valid_Date { get; set; }
        public string Justification_Summary { get; set; }
        public string Recommended_Vendor { get; set; }
        public string Recommended_Vendor_Name { get; set; }
        public string Recommended_Vendor_Landed_Cost { get; set; }
        public string SBU { get; set; }
        public string Location { get; set; }
        //  public string DEPT { get; set; }
        //  public string CATEGORY { get; set; }
        //   public string SCATEGORY { get; set; }
        public string Value { get; set; }
        public string Tax { get; set; }
        public string Cost_Center { get; set; }
        public string CEP_Approval_No { get; set; }
        public string RFWD_No { get; set; }
        public string Proposed_CEP_Amount { get; set; }
        public string RFWD_Value { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetWorkitemHeaderData(this);
        }
    }
    #endregion

    #region LogData
    public class LogData
    {
        public string WorkitemID { get; set; }
        public string DocID { get; set; }

        public string User { get; set; }
        public string Comments { get; set; }
        public string Activity { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        public string Kind { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetWorkitemLogData(this);
        }
    }
    #endregion

    #region AttachmentData
    public class AttachmentData
    {
        public string WorkitemID { get; set; }
        public string DocID { get; set; }
        public string FolderName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }

        public string Kind { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetWorkitemAttachData(this);
        }
    }
    #endregion

    #region ActivityData
    public class ActivityData
    {
        public string WorkitemID { get; set; }
        public string DocID { get; set; }

        public string CommentsRequired { get; set; }
        public string ButtonText { get; set; }
        public string ActivityName { get; set; }
        public string Icon { get; set; }


        public async Task Save()
        {
            await DataAccessLayer.Current.SetWorkitemActivityData(this);
        }
    }
    #endregion

    #region LineItemData
    public class LineItemData
    {
        public string WorkitemID { get; set; }
        public string DocID { get; set; }

        public string Amount { get; set; }
        public string Material { get; set; }

        public string Kind { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetWorkitemLineItemData(this);
        }
    }
    #endregion

    #region SubmitRequestData
    public class SubmitRequestData
    {
        public string newRequestId { get; set; }
        public string DocumentType { get; set; }
        public string Location { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetSubmitRequestData(this);
        }
    }
    #endregion

    #region UserPreferences
    public class UserPreferences : BindableBase//, ITokenProvider
    {
        public static Int32 ConversationMin = 20;
        public static Int32 ConversationMax = 100;

        // Database requires non-null for the primary key UserID (though it's ignored unless UserToken is non-null);
        public static String DefaultUserID = "unknown";

        Int32 conversationLimit = ConversationMax;

        [PrimaryKey]
        public String UserID { get; set; }

        // The existence of an access token means the user has authenticated with the backend.
        // V2: access token should be stored in secure storage and not the database.
        public String AccessToken { get; set; }
        public String ProviderName { get; set; }
        public String ProviderUri { get; set; }
        public Int32 ConversationLimit
        {
            get { return conversationLimit; }
            set
            {
                SetProperty(ref conversationLimit, value);
            }
        }

        // This field is just used with IsEqual; real values are in the IsActive fields in Configuration.Categories.
        public String ActiveCategories { get; set; }

        public UserPreferences()
        {
            UserID = DefaultUserID;
            AccessToken = null;
            ProviderName = null;
            ProviderUri = null;
            ConversationLimit = UserPreferences.ConversationMax;
            ActiveCategories = null;
        }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetUserPreferencesAsync(this);
        }

        //Clone takes a snapshot for later comparison with IsEqual
        public UserPreferences Clone()
        {
            // Note that we don't need to call InitAsync on this UserPreference instance
            // because we're initializing with known values.
            return new UserPreferences()
            {
                UserID = this.UserID,
                AccessToken = this.AccessToken,
                ProviderName = this.ProviderName,
                ProviderUri = this.ProviderUri,
                ConversationLimit = this.ConversationLimit,
                ActiveCategories = this.ActiveCategories
            };
        }

        public Boolean IsEqual(UserPreferences other)
        {
            if (other == null) { return false; }

            return (this.UserID == other.UserID
                && this.AccessToken == other.AccessToken
                && this.ProviderName == other.ProviderName
                && this.ProviderUri == other.ProviderUri
                && this.ConversationLimit == other.ConversationLimit
                && this.ActiveCategories == other.ActiveCategories);
        }
    }
    #endregion

    #region AuthProvider
    //Describes an authentication provider supported by the backend
    public class AuthProvider
    {
        [PrimaryKey]
        public String Name { get; set; }
        public String Url { get; set; }
    }
    #endregion

    #region Configuration
    // Argument type for CheckChanges method
    [Flags]
    public enum ChangeCheckType : int
    {
        ConversationLimit,
        CategorySelection,
    }

    // Configuration contains the data for the configuration page, which includes the
    // list of all categories and the current user preferences.
    public class Configuration : BindableBase
    {
        CategoryList categories;
        List<AuthProvider> providers;
        Boolean hasChanged;

        public Configuration()
        {
        }

        public async Task InitAsync()
        {
            categories = new CategoryList();
            await PopulateCategoriesFromDB();

            providers = new List<AuthProvider>();
            await PopulateAuthProvidersFromDB();

            // The access token in the user preferences determines authentication status, as that token
            // is used automatically in the HTTP request message hander. Nothing else is needed to
            // authenticate on startup.

            CurrentUser = await DataAccessLayer.Current.GetUserPreferencesAsync();

            if (CurrentUser == null)
            {
                CurrentUser = new UserPreferences();
                CurrentUser.ActiveCategories = GetCategoryList(CategorySet.Active);
            }

            // We could call ApplyBackendConfiguration here, but we will let the consumer of
            // this data model do that when it wants. (See HomeViewModel.cs.)            
        }

        public UserPreferences CurrentUser { get; private set; }
        public List<AuthProvider> Providers { get { return providers; } }
        public CategoryList Categories { get { return categories; } }

        public Boolean HasChanged
        {
            get { return hasChanged; }
            set { SetProperty(ref hasChanged, value); }
        }

        public async Task PopulateCategoriesFromDB()
        {
            Categories.Clear();
            var dbCategories = await DataAccessLayer.Current.GetCategoriesAsync(CategorySet.All);

            foreach (Category c in dbCategories)
            {
                Categories.Add(c);
            }
        }

        public async Task PopulateAuthProvidersFromDB()
        {
            Providers.Clear();
            IEnumerable<AuthProvider> providers = null;
            providers = await DataAccessLayer.Current.GetAuthProvidersAsync();
            Providers.AddRange(providers);
        }


        public List<String> GetCategories(CategorySet set)
        {
            IEnumerable<String> temp;

            if (set == CategorySet.Active)
            {
                temp = from c in Categories where c.IsActive == true select c.Name;
            }
            else
            {
                temp = from c in Categories select c.Name;
            }

            return temp.ToList();
        }

        public String GetCategoryList(CategorySet set = CategorySet.All)
        {
            String list = "";

            foreach (String c in GetCategories(set))
            {
                list += c + ",";
            }

            //Remove the trailing , if there is one
            char[] trims = { ',' };
            return list.TrimEnd(trims);
        }

        public Task ApplyConversationLimit()
        {
            return DataAccessLayer.Current.ApplyConversationLimitAsync(CurrentUser.ConversationLimit);
        }

        public void ConfigurationApplied()
        {
            HasChanged = false;
        }

        public async Task<Boolean> CheckChanges(UserPreferences previous, ChangeCheckType type)
        {
            if ((type & ChangeCheckType.CategorySelection) == ChangeCheckType.CategorySelection)
            {
                // Update category status in the database
                foreach (Category c in categories)
                {
                    await DataAccessLayer.Current.UpdateOrAddCategoryAsync(c, CategoryKeepActive.Force);
                }

                // Update the UserPreferences
                CurrentUser.ActiveCategories = GetCategoryList(CategorySet.Active);
                await CurrentUser.Save();
            }

            HasChanged = !CurrentUser.IsEqual(previous);
            return HasChanged;
        }

        /*public async Task<Boolean> ApplyBackendConfiguration()
        {
            // Ignore if we're not authenticated.
            if (CurrentUser.AccessToken == null || CurrentUser.AccessToken == "")
            {
                return false;
            }

            Boolean result = false;

            // Retrieve preferences from the backend and apply them here.
            UserPreferenceDTO prefs = await WebAPI.GetUserPrefsAsync();

            if (prefs != null)            
            {
                result = true;
                CurrentUser.ConversationLimit = prefs.ConversationLimit;

                // Go through all categories and change IsActive flags depending on how they're
                // set in the preferences we just retrieved.
                foreach (Category c in categories)
                {
                    c.IsActive = prefs.Categories.Contains(c.Name);
                    await DataAccessLayer.Current.UpdateOrAddCategoryAsync(c, CategoryKeepActive.Force);
                }

                CurrentUser.ActiveCategories = GetCategoryList(CategorySet.Active);
            }

            await CurrentUser.Save();
            return result;
        }*/
    }
    #endregion

    #region UserDetails
    public class UserDetailsData
    {
        [PrimaryKey]
        public string UserId { get; set; }
        [MaxLength(50)]
        public String FirstName { get; set; }
        [MaxLength(50)]
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Title { get; set; }
        public string Location { get; set; }
        public string SAPUserId { get; set; }
        public string PhoneNumber { get; set; }

        public async Task Save()
        {
            await DataAccessLayer.Current.SetUserDetailsData(this);
        }
    }
    #endregion
}
