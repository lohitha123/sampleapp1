using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using Smartdocs.SQLite;
using Smartdocs.Models;

namespace Smartdocs
{
    // The singleton Database class provides methods to abstract queries to the SQLite database.
    // It doesn't concern itself with data binding as that's the job of the DataModel class.
    //
    // Note: this implementation is derived from Xamarin's To Do PCL example but is rewritten to use
    // the asynchronous SQLite API. The primary changes from the sychronous API to the async API are:
    //   1. No need to use a lock around SQLite calls.
    //   2. Database initialization code went into InitAsync, which has to be called from another
    //      async function, e.g. the App's OnStart.
    //   3. Functions that end in a single async call are implemented to return Task<T>. These do not
    //      use await and are not therefore marked as async.
    //   4. Functions that involve multiple async calls, do processing after an await, or have no
    //      return value are marked as async and return Task<T>.    

    public class DataAccessLayer
    {
        static object locker = new object();
        static SQLiteAsyncConnection database;
        public static DataAccessLayer Current { get { return _current; } }
        public static DataAccessLayer _current = null;

        #region Constructors
        // The optional argument (and InitAsync) are used by the DBInitialize console program to bypass the
        // Xamarin.Forms DependencyService.         
        public DataAccessLayer(SQLiteAsyncConnection db = null)
        {
            if (db == null)
            {
                String path = DependencyService.Get<ISQLite>().GetDatabasePath();
                db = new SQLiteAsyncConnection(path);

                // Alternate use to use the synchronous SQLite API:
                // database = SQLiteConnection(path);         

                try
                {
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Due_Date_As_Per_PO varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN  Retention_Doc_No varchar");
                    //Coromandal Headers
                    //   db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN SBU_DES varchar");
                    //   db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN LOCATION_DES varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Department varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Category varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN SubCategory varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Total_Amount varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Valid_Date varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Justification_Summary  varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Recommended_Vendor varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Recommended_Vendor_Name varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Recommended_Vendor_Landed_Cost varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN SBU varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Location varchar");
                    //   db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN DEPT varchar"); 
                    //   db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN CATEGORY varchar");
                    //   db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN SCATEGORY varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Value varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Tax varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Cost_Center varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN CEP_Approval_No varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN RFWD_No varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN Proposed_CEP_Amount varchar");
                    db.QueryAsync<SQLHeaderData>("ALTER TABLE `SQLHeaderData` ADD COLUMN RFWD_Value varchar");

                    db.QueryAsync<AttachmentData>("ALTER TABLE `AttachmentData` ADD COLUMN FolderName varchar");
                }
                catch
                { }
            }

            database = db;
            _current = this;
        }

        public async Task InitAsync()
        {
            // Note that the data types used in CreateTableAsync need to be simple classes without any properties
            // of complex classes like System.Uri. If you try to use such classes, CreateTableasync will throw an
            // exception without any decent information as to why.

            try
            {
                //await database.CreateTableAsync<DocumentType>();
                //await database.CreateTableAsync<DocumentField>();
                //await database.CreateTableAsync<WorkitemAdminData>();
                //await database.CreateTableAsync<SQLHeaderData>();
                //await database.CreateTableAsync<LogData>();
                //await database.CreateTableAsync<AttachmentData>();
                //await database.CreateTableAsync<ActivityData>();
                //await database.CreateTableAsync<LineItemData>();
                //await database.CreateTableAsync<UserInfo>();
                //await database.CreateTableAsync<SubmitRequestData>();

                //await database.CreateTableAsync<UserPreferences>();
                //await database.CreateTableAsync<AuthProvider>();
                //await database.CreateTableAsync<Timestamp>();
                //await database.CreateTableAsync<Category>();
                await database.CreateTableAsync<UserDetailsData>();

                await database.QueryAsync<DocumentType>("DELETE FROM `DocumentType`");
                await database.QueryAsync<DocumentField>("DELETE FROM `DocumentField`");
                await database.QueryAsync<WorkitemAdminData>("DELETE FROM `WorkitemAdminData`");
                await database.QueryAsync<LogData>("DELETE FROM `LogData` where `Kind` != 'Request'");
                await database.QueryAsync<AttachmentData>("DELETE FROM `AttachmentData` where `Kind` != 'Request'");
                await database.QueryAsync<ActivityData>("DELETE FROM `ActivityData`");
                await database.QueryAsync<SQLHeaderData>("DELETE FROM `SQLHeaderData` where `Kind` != 'Request'");
                await database.QueryAsync<LineItemData>("DELETE FROM `LineItemData` where `Kind` != 'Request'");
                await database.QueryAsync<UserDetailsData>("DELETE FROM `UserDetailsData`");
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("sql database creation error" + ex.Message);
            }
        }

        public async Task InitUserInfoDB()
        {
            await database.QueryAsync<UserInfo>("DELETE FROM `UserInfo`");
        }

        #endregion

        #region UserInfo Table

        public Task<UserInfo> GetUserInfoAsync()
        {
            return database.Table<UserInfo>().FirstOrDefaultAsync();
        }

        public async Task SetUserInfoAsync(UserInfo userInfo)
        {
            UserInfo current = await GetUserInfoAsync();

            if (current == null)
            {
                try
                {
                    await database.InsertAsync(userInfo);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {

                string query = "INSERT INTO `UserInfo`(`userID`,`password`,`portalURL`,`companyID`,`token`) VALUES(\"" +
                            userInfo.userID + "\"" + "," + "\"" +
                            userInfo.password + "\"" + "," + "\"" +
                            userInfo.portalURL + "\"" + "," + "\"" +
                            userInfo.companyID + "\"" + "," + "\"" +
                            userInfo.token + "\"" + ")";
                await database.QueryAsync<UserInfo>(query);
                Debug.WriteLine("====== write user info to sqlite d");

            }
        }
        public async Task<List<UserInfo>> PullUserInfo(string userId, string password)
        {
            string query = "SELECT * FROM `UserInfo` WHERE `userID` = '" + userId + "' AND `password` = '" + password + "'";
            List<UserInfo> userInfo = await database.QueryAsync<UserInfo>(query);

            return userInfo;
        }

        #endregion

        #region DocumentType Table

        public Task<DocumentType> GetDocumentTypeAsync()
        {
            return database.Table<DocumentType>().FirstOrDefaultAsync();
        }

        public async Task SetDocumentTypeAsync(DocumentType dtype)
        {
            DocumentType current = await GetDocumentTypeAsync();

            if (current == null)
            {
                await database.InsertAsync(dtype);
            }
            else
            {

                string query = "INSERT INTO `DocumentType`(`docTypeName`,`DocTypeDesc`,`SignaturePad`,`WorkItemOnly`,`Label`,`SubLabel`,`IconName`) VALUES(\"" +
                         dtype.docTypeName + "\"" + "," + "\"" +
                         dtype.DocTypeDesc + "\"" + "," + "\"" +
                         dtype.SignaturePad + "\"" + "," + "\"" +
                         dtype.WorkItemOnly + "\"" + "," + "\"" +
                         dtype.Label + "\"" + "," + "\"" +
                         dtype.SubLabel + "\"" + "," + "\"" +
                         dtype.IconName + "\"" + ")";
                await database.QueryAsync<DocumentType>(query);

            }
        }

        public async Task PullDocType()
        {
            App.G_DOC_ITEMS = new List<DocType>();

            string query = "select * from DocumentType";
            List<DocumentType> results = await database.QueryAsync<DocumentType>(query);

            foreach (DocumentType doctype_sql_item in results)
            {
                string query1 = "select * from DocumentField where docTypeName='" + doctype_sql_item.docTypeName + "'";
                List<DocumentField> result1 = await database.QueryAsync<DocumentField>(query1);

                var docType = new DocType();

                docType.docTypeName = doctype_sql_item.docTypeName;
                docType.adminData = new DocTypeAdminData();
                docType.adminData.DocTypeDesc = doctype_sql_item.DocTypeDesc;
                docType.adminData.SignaturePad = doctype_sql_item.SignaturePad;
                docType.adminData.SubLabel = doctype_sql_item.SubLabel;
                docType.adminData.WorkItemOnly = doctype_sql_item.WorkItemOnly;
                docType.adminData.IconName = doctype_sql_item.IconName;
                docType.adminData.Label = doctype_sql_item.Label;

                docType.dataFields = new List<DataField>();
                for (int i = 0; i < result1.Count; i++)
                {
                    var dataField = new DataField();
                    dataField.FieldName = result1[i].FieldName;
                    dataField.LineItemType = result1[i].LineItemType;
                    dataField.PossibleValues = result1[i].PossibleValues;
                    dataField.DataType = result1[i].DataType;
                    dataField.Length = result1[i].Length;
                    dataField.Mandatory = result1[i].Mandatory;
                    dataField.Order = result1[i].Order;
                    dataField.BarCodeField = result1[i].BarcodeField;
                    dataField.Label = result1[i].Label;
                    dataField.VisibleLength = result1[i].VisibleLength;

                    docType.dataFields.Add(dataField);
                }

                App.G_DOC_ITEMS.Add(docType);
            }
        }

        #endregion

        #region DocumentField Table

        public Task<DocumentField> GetDocumentFieldAsync()
        {
            return database.Table<DocumentField>().FirstOrDefaultAsync();
        }

        public async Task SetDocumentFieldAsync(DocumentField dfield)
        {
            DocumentField current = await GetDocumentFieldAsync();

            if (current == null)
            {
                await database.InsertAsync(dfield);
            }
            else
            {

                string query = "INSERT INTO `DocumentField`(`docTypeName`,`FieldName`,`LineItemType`,`PossibleValues`,`DataType`,`Length`,`Mandatory`,`Order`,`BarcodeField`,`Label`,`VisibleLength`) VALUES(\"" +
                          dfield.docTypeName + "\"" + "," + "\"" +
                          dfield.FieldName + "\"" + "," + "\"" +
                          dfield.LineItemType + "\"" + "," + "\"" +
                          dfield.PossibleValues + "\"" + "," + "\"" +
                          dfield.DataType + "\"" + "," + "\"" +
                          dfield.Length + "\"" + "," + "\"" +
                          dfield.Mandatory + "\"" + "," + "\"" +
                          dfield.Order + "\"" + "," + "\"" +
                          dfield.BarcodeField + "\"" + "," + "\"" +
                          dfield.Label + "\"" + "," + "\"" +
                          dfield.VisibleLength + "\"" + ")";
                await database.QueryAsync<DocumentField>(query);
            }
        }

        #endregion

        #region WorkitemAdminData Table
        public Task<WorkitemAdminData> GetWorkitemAdminData()
        {
            return database.Table<WorkitemAdminData>().FirstOrDefaultAsync();
        }

        public async Task SetWorkitemAdminData(WorkitemAdminData adminData)
        {
            WorkitemAdminData current = await GetWorkitemAdminData();
            if (current == null)
            {
                await database.InsertAsync(adminData);
            }
            else
            {

                string query = "INSERT INTO `WorkitemAdminData`(`WorkitemID`,`DocID`,`DocumentType`,`SAPWorkItem`,`Mode`,`WorkitemTitle`,`WorkitemDate`,`Status`) VALUES ('" +
                         adminData.WorkitemID + "'" + "," + "'" +
                         adminData.DocID + "'" + "," + "'" +
                         adminData.DocumentType + "'" + "," + "'" +
                         adminData.SAPWorkItem + "'" + "," + "'" +
                         adminData.Mode + "'" + "," + "'" +
                         adminData.WorkitemTitle + "'" + "," + "'" +
                         adminData.WorkitemDate + "'" + "," + "'" +
                                  adminData.Status + "'" + ")";
                await database.QueryAsync<WorkitemAdminData>(query);
            }
        }

        public async Task SetWorkitemStatus(string workitemID, string status)
        {
            string query = "update `WorkitemAdminData` set `Status`='" + status + "' where `WorkitemID`='" + workitemID + "'";
            await database.QueryAsync<WorkitemAdminData>(query);
        }

        public async Task<List<WorkItem>> PullWorkitemData(string status)
        {
            var workitem_list = new List<WorkItem>();
            string query = "select * from WorkitemAdminData where `Status` = '" + status + "'";
            List<WorkitemAdminData> list_workitem_admindata = await database.QueryAsync<WorkitemAdminData>(query);

            for (int i = 0; i < list_workitem_admindata.Count; i++)
            {
                string keyField = list_workitem_admindata[i].WorkitemID;

                var workitem = new WorkItem();
                workitem.adminData = new AdminData();
                workitem.headerData = new HeaderData();
                workitem.logs = new List<Log>();
                workitem.activities = new List<Activity>();
                workitem.attachments = new List<Attachment>();
                workitem.lineitemData = new List<LineItem>();

                workitem.workItemId = list_workitem_admindata[i].WorkitemID;//keyField
                workitem.docId = list_workitem_admindata[i].DocID;
                workitem.adminData.DocumentType = list_workitem_admindata[i].DocumentType;
                workitem.adminData.SAPWorkItem = list_workitem_admindata[i].SAPWorkItem;
                workitem.adminData.Mode = list_workitem_admindata[i].Mode;
                workitem.adminData.WorkitemTitle = list_workitem_admindata[i].WorkitemTitle;
                workitem.adminData.WorkitemDate = list_workitem_admindata[i].WorkitemDate;
                workitem.adminData.Status = list_workitem_admindata[i].Status;

                string query1 = "select * from LineItemData where WorkitemID='" + keyField + "'";
                List<LineItemData> list_lineitemdata = await database.QueryAsync<LineItemData>(query1);
                for (int j = 0; j < list_lineitemdata.Count; j++)
                {
                    var lineitem = new LineItem();
                    lineitem.Amount = list_lineitemdata[j].Amount;
                    lineitem.Material = list_lineitemdata[j].Material;
                    workitem.lineitemData.Add(lineitem);
                }

                string query_logdata = "select * from LogData where WorkitemID='" + keyField + "'";
                List<LogData> list_logdata = await database.QueryAsync<LogData>(query_logdata);
                for (int j = 0; j < list_logdata.Count; j++)
                {
                    var log = new Log();
                    log.User = list_logdata[j].User;
                    log.Comments = list_logdata[j].Comments;
                    log.Activity = list_logdata[j].Activity;
                    log.Time = list_logdata[j].Time;
                    log.Date = list_logdata[j].Date;
                    workitem.logs.Add(log);
                }

                string query_attach = "select * from AttachmentData where WorkitemID='" + keyField + "'";
                List<AttachmentData> list_attachdata = await database.QueryAsync<AttachmentData>(query_attach);
                for (int j = 0; j < list_attachdata.Count; j++)
                {
                    var attachment = new Attachment();
                    attachment.Name = list_attachdata[j].Name;
                    attachment.FolderName = list_attachdata[j].FolderName;
                    attachment.Type = list_attachdata[j].Type;
                    attachment.URL = list_attachdata[j].URL;
                    workitem.attachments.Add(attachment);
                }

                string query_activity = "select * from ActivityData where WorkitemID='" + keyField + "'";
                List<ActivityData> list_activitydata = await database.QueryAsync<ActivityData>(query_activity);
                for (int j = 0; j < list_activitydata.Count; j++)
                {
                    var activity = new Activity();
                    activity.CommentsRequired = list_activitydata[j].CommentsRequired;
                    activity.ButtonText = list_activitydata[j].ButtonText;
                    activity.ActivityName = list_activitydata[j].ActivityName;
                    activity.Icon = list_activitydata[j].Icon;
                    workitem.activities.Add(activity);
                }

                string query_header = "select * from SQLHeaderData where WorkitemID='" + keyField + "'";
                List<SQLHeaderData> list_sqlheaderdata = await database.QueryAsync<SQLHeaderData>(query_header);
                for (int j = 0; j < list_sqlheaderdata.Count; j++)
                {
                    workitem.headerData.Advance_amount = list_sqlheaderdata[j].Advance_amount;
                    workitem.headerData.Company_name = list_sqlheaderdata[j].Company_name;
                    workitem.headerData.Other_deductions = list_sqlheaderdata[j].Other_deductions;
                    workitem.headerData.Remarks2 = list_sqlheaderdata[j].Remarks2;

                }

                string query_userDetails = "select * from UserDetailsData";
                List<UserDetailsData> list_userdetailsdata = await database.QueryAsync<UserDetailsData>(query_userDetails);
                for (int j = 0; j < list_userdetailsdata.Count; j++)
                {
                    var userDetails = new UserDetails();
                    userDetails.UserId = list_userdetailsdata[j].UserId;
                    userDetails.FirstName = list_userdetailsdata[j].FirstName;
                    userDetails.LastName = list_userdetailsdata[j].LastName;
                    userDetails.Email = list_userdetailsdata[j].Email;
                    userDetails.Title = list_userdetailsdata[j].Title;
                    userDetails.Location = list_userdetailsdata[j].Location;
                    userDetails.SAPUserId = list_userdetailsdata[j].SAPUserId;
                    userDetails.PhoneNumber = list_userdetailsdata[j].PhoneNumber;
                    workitem.userDetailsData.Add(userDetails);
                }

                workitem_list.Add(workitem);
            }

            return workitem_list;
        }

        public async Task GetBindUserDetailsUsingSQLite()
        {
            string query_userDetails = "select * from UserDetailsData";
            List<UserDetailsData> list_userdetailsdata = await database.QueryAsync<UserDetailsData>(query_userDetails);
            for (int j = 0; j < list_userdetailsdata.Count; j++)
            {
                var userDetails = new UserDetails();
                userDetails.UserId = list_userdetailsdata[j].UserId;
                userDetails.FirstName = list_userdetailsdata[j].FirstName;
                userDetails.LastName = list_userdetailsdata[j].LastName;
                userDetails.Email = list_userdetailsdata[j].Email;
                userDetails.Title = list_userdetailsdata[j].Title;
                userDetails.Location = list_userdetailsdata[j].Location;
                userDetails.SAPUserId = list_userdetailsdata[j].SAPUserId;
                userDetails.PhoneNumber = list_userdetailsdata[j].PhoneNumber;
                App.G_UserDetails.Add(userDetails);
            }
        }

        #endregion

        #region WorkitemLogData Table
        public Task<LogData> GetWorkitemLogData()
        {
            return database.Table<LogData>().FirstOrDefaultAsync();
        }

        public async Task SetWorkitemLogData(LogData logData)
        {
            var current = await GetWorkitemLogData();
            if (current == null)
            {
                await database.InsertAsync(logData);
            }
            else
            {

                string query = "INSERT INTO `LogData`(`WorkitemID`,`DocID`,`User`,`Comments`,`Activity`,`Time`,`Date`,`Kind`) VALUES ('" +
                    logData.WorkitemID + "'" + "," + "'" +
                           logData.DocID + "'" + "," + "'" +
                           logData.User + "'" + "," + "'" +
                           logData.Comments + "'" + "," + "'" +
                           logData.Activity + "'" + "," + "'" +
                           logData.Time + "'" + "," + "'" +
                           logData.Date + "'" + "," + "'" +
                           logData.Kind + "'" + ")";
                await database.QueryAsync<LogData>(query);
            }
        }

        public async Task RemoveWorkitemLogData(string workitemID)
        {
            await database.QueryAsync<LogData>("DELETE FROM `LogData` where `WorkitemID` = '" + workitemID + "'");
        }
        #endregion

        #region WorkitemAttachmentData Table
        public Task<AttachmentData> GetWorkitemAttachData()
        {
            return database.Table<AttachmentData>().FirstOrDefaultAsync();
        }

        public async Task SetWorkitemAttachData(AttachmentData attachData)
        {
            var current = await GetWorkitemAttachData();
            if (current == null)
            {
                await database.InsertAsync(attachData);
            }
            else
            {

                string query = "INSERT INTO `AttachmentData`(`WorkitemID`,`DocID`,`FolderName`,`Name`,`Type`,`URL`,`Kind`) VALUES ('" +
                              attachData.WorkitemID + "'" + "," + "'" +
                              attachData.DocID + "'" + "," + "'" +
                                attachData.FolderName + "'" + "," + "'" +
                              attachData.Name + "'" + "," + "'" +
                              attachData.Type + "'" + "," + "'" +
                              attachData.URL + "'" + "," + "'" +
                              attachData.Kind + "'" + ")";
                await database.QueryAsync<AttachmentData>(query);
            }
        }
        #endregion

        #region WorkitemActivityData Table
        public Task<ActivityData> GetWorkitemActivityData()
        {
            return database.Table<ActivityData>().FirstOrDefaultAsync();
        }

        public async Task SetWorkitemActivityData(ActivityData activityData)
        {
            var current = await GetWorkitemActivityData();
            if (current == null)
            {
                await database.InsertAsync(activityData);
            }
            else
            {

                string query = "INSERT INTO `ActivityData`(`WorkitemID`,`DocID`,`CommentsRequired`,`ButtonText`,`ActivityName`,`Icon`) VALUES ('" +
                                activityData.WorkitemID + "'" + "," + "'" +
                                activityData.DocID + "'" + "," + "'" +
                                activityData.CommentsRequired + "'" + "," + "'" +
                                activityData.ButtonText + "'" + "," + "'" +
                                activityData.ActivityName + "'" + "," + "'" +
                                activityData.Icon + "'" + ")";
                await database.QueryAsync<ActivityData>(query);
            }
        }
        #endregion

        #region WorkitemHeaderData Table
        public Task<SQLHeaderData> GetWorkitemHeaderData()
        {
            return database.Table<SQLHeaderData>().FirstOrDefaultAsync();
        }

        public async Task SetWorkitemHeaderData(SQLHeaderData headerData)
        {
            var current = await GetWorkitemHeaderData();
            if (current == null)
            {
                await database.InsertAsync(headerData);
            }
            else
            {

                string query = "INSERT INTO `SQLHeaderData`(`WorkitemID`,`DocID`,`Advance_amount`,`Company_name`,`Other_deductions`,`Remarks2`," +
                                "'Remarks1','Nature_of_Work','Retention',`Fund_Centre_Name`,`Project_site`,`WBS_Element`,`Kind`) VALUES ('" +
                                headerData.WorkitemID + "'" + "," + "'" +
                                headerData.DocID + "'" + "," + "'" +
                                headerData.Advance_amount + "'" + "," + "'" +
                                headerData.Company_name + "'" + "," + "'" +
                                headerData.Other_deductions + "'" + "," + "'" +
                                headerData.Remarks2 + "'" + "," + "'" +
                                headerData.Remarks1 + "'" + "," + "'" +
                                headerData.Nature_of_work + "'" + "," + "'" +
                                headerData.Retention + "'" + "," + "'" +
                                headerData.Fund_Centre_Name + "'" + "," + "'" +
                                headerData.Project_site + "'" + "," + "'" +
                                headerData.WBS_Element + "'" + "," + "'" +
                                headerData.Kind + "'" + ")";
                await database.QueryAsync<SQLHeaderData>(query);
            }
        }
        #endregion

        #region WorkitemLineItemData Table
        public Task<LineItemData> GetWorkitemLineItemData()
        {
            return database.Table<LineItemData>().FirstOrDefaultAsync();
        }

        public async Task SetWorkitemLineItemData(LineItemData lineitemData)
        {
            var current = await GetWorkitemLineItemData();
            if (current == null)
            {
                await database.InsertAsync(lineitemData);
            }
            else
            {

                string query = "INSERT INTO `LineItemData`(`WorkitemID`,`DocID`,`Amount`,`Material`,`Kind`) VALUES ('" +
                                lineitemData.WorkitemID + "'" + "," + "'" +
                                lineitemData.DocID + "'" + "," + "'" +
                                lineitemData.Amount + "'" + "," + "'" +
                                lineitemData.Material + "'" + "," + "'" +
                                            lineitemData.Kind + "'" + ")";
                await database.QueryAsync<LineItemData>(query);
            }
        }
        #endregion

        #region SubmitRequestData Table
        public Task<SubmitRequestData> GetSubmitRequestData()
        {
            return database.Table<SubmitRequestData>().FirstOrDefaultAsync();
        }
        public async Task SetSubmitRequestData(SubmitRequestData data)
        {
            var current = await GetSubmitRequestData();
            if (current == null)
                await database.InsertAsync(data);
            else
            {
                string query = "insert into `SubmitRequestData`(`newRequestId`,`DocumentType`,`Location`) values ('" +
                        data.newRequestId + "'" + "," + "'" +
                        data.DocumentType + "'" + "," + "'" +
                        data.Location + "'" + ")";
                await database.QueryAsync<SubmitRequestData>(query);
            }
        }

        public async Task<List<SubmitRequest>> PullSubmitRequestData()
        {
            var submitRequest_list = new List<SubmitRequest>();

            string query = "select * from SubmitRequestData";
            List<SubmitRequestData> list_submitrequest_data = await database.QueryAsync<SubmitRequestData>(query);

            for (int i = 0; i < list_submitrequest_data.Count; i++)
            {
                string key_field = list_submitrequest_data[i].newRequestId;
                var submitRequest = new SubmitRequest();
                submitRequest.adminData = new SubmitRequestAdminData();
                submitRequest.headerData = new HeaderData();
                submitRequest.logs = new Log();
                submitRequest.attachments = new List<Attachment>();
                submitRequest.lineitemData = new List<LineItem>();

                submitRequest.newRequestId = key_field;
                submitRequest.adminData.DocumentType = list_submitrequest_data[i].DocumentType;
                submitRequest.adminData.Location = list_submitrequest_data[i].Location;

                string query_header = "select * from SQLHeaderData where WorkitemID='" + key_field + "'";
                List<SQLHeaderData> list_sqlheaderdata = await database.QueryAsync<SQLHeaderData>(query_header);
                for (int j = 0; j < list_sqlheaderdata.Count; j++)
                {
                    submitRequest.headerData.Advance_amount = list_sqlheaderdata[j].Advance_amount;
                    submitRequest.headerData.Company_name = list_sqlheaderdata[j].Company_name;
                    submitRequest.headerData.Other_deductions = list_sqlheaderdata[j].Other_deductions;
                    submitRequest.headerData.Remarks2 = list_sqlheaderdata[j].Remarks2;
                }

                string query_logdata = "select * from LogData where WorkitemID='" + key_field + "'";
                List<LogData> list_logdata = await database.QueryAsync<LogData>(query_logdata);
                for (int j = 0; j < list_logdata.Count; j++)
                {
                    submitRequest.logs.User = list_logdata[j].User;
                    submitRequest.logs.Comments = list_logdata[j].Comments;
                    submitRequest.logs.Activity = list_logdata[j].Activity;
                    submitRequest.logs.Time = list_logdata[j].Time;
                    submitRequest.logs.Date = list_logdata[j].Date;
                }

                string query_attach = "select * from AttachmentData where WorkitemID='" + key_field + "'";
                List<AttachmentData> list_attachdata = await database.QueryAsync<AttachmentData>(query_attach);
                for (int j = 0; j < list_attachdata.Count; j++)
                {
                    var attachment = new Attachment();
                    attachment.Name = list_attachdata[j].Name;
                    attachment.Type = list_attachdata[j].Type;
                    attachment.URL = list_attachdata[j].URL;
                    attachment.FolderName = list_attachdata[j].FolderName;
                    submitRequest.attachments.Add(attachment);
                }

                string query1 = "select * from LineItemData where WorkitemID='" + key_field + "'";
                List<LineItemData> list_lineitemdata = await database.QueryAsync<LineItemData>(query1);
                for (int j = 0; j < list_lineitemdata.Count; j++)
                {
                    var lineitem = new LineItem();
                    lineitem.Amount = list_lineitemdata[j].Amount;
                    lineitem.Material = list_lineitemdata[j].Material;
                    submitRequest.lineitemData.Add(lineitem);
                }

                submitRequest_list.Add(submitRequest);

            }

            return submitRequest_list;
        }

        public async Task RemoveSubmitRequest(string requestId)
        {
            string query = "delete from SubmitRequestData where `newRequestId`='" + requestId + "'";
            await database.QueryAsync<SubmitRequestData>(query);
        }

        #endregion


        #region UserPreferences Table
        // UserPreferences is a single-row table.
        public Task<UserPreferences> GetUserPreferencesAsync()
        {
            return database.Table<UserPreferences>().FirstOrDefaultAsync();
        }

        public async Task SetUserPreferencesAsync(UserPreferences prefs)
        {
            UserPreferences current = await GetUserPreferencesAsync();

            if (current == null)
            {
                await database.InsertAsync(prefs);
            }
            else
            {
                String query = "update UserPreferences set UserID='" + prefs.UserID + "' ";
                query += ", AccessToken='" + prefs.AccessToken + "' ";
                query += ", ProviderName='" + prefs.ProviderName + "' ";
                query += ", ProviderUri='" + prefs.ProviderUri + "' ";
                query += ", ConversationLimit='" + prefs.ConversationLimit + "' ";
                query += ", ActiveCategories='" + prefs.ActiveCategories + "'";
                await database.QueryAsync<UserPreferences>(query);
            }
        }
        #endregion

        #region AuthProviders Table
        public Task<List<AuthProvider>> GetAuthProvidersAsync()
        {
            return database.Table<AuthProvider>().ToListAsync();
        }

        public async Task UpdateOrAddAuthProviderAsync(AuthProvider ap)
        {
            List<AuthProvider> results = await database.QueryAsync<AuthProvider>("select * from AuthProvider where Name='" + ap.Name + "'");
            AuthProvider current = results.FirstOrDefault<AuthProvider>();

            if (current != null)
            {
                await database.UpdateAsync(ap);
            }
            else
            {
                await database.InsertAsync(ap);
            }
        }

        public Task<int> DeleteAuthProviderAsync(String name)
        {
            return database.DeleteAsync(new AuthProvider() { Name = name });
        }
        #endregion

        #region Timestamp Table
        // Timestamp is a single-row table
        public Task<Timestamp> GetTimestampAsync()
        {
            return database.Table<Timestamp>().FirstOrDefaultAsync();
        }

        public async Task SetTimestampAsync(Timestamp t)
        {
            Timestamp current = await GetTimestampAsync();

            if (current == null)
            {
                await database.InsertAsync(t);
            }
            else
            {
                await database.QueryAsync<Timestamp>("update Timestamp set Stamp='" + t.Stamp + "'");
            }
        }
        #endregion

        #region Category Table
        public Task<List<Category>> GetCategoriesAsync(CategorySet set = CategorySet.All)
        {
            String query = "select * from Category";

            switch (set)
            {
                case CategorySet.All:
                default:
                    break;

                case CategorySet.Active:
                    query += " where IsActive = 1";
                    break;
            }

            return database.QueryAsync<Category>(query);
        }

        public async Task<IEnumerable<String>> GetCategoryNamesAsync(CategorySet set = CategorySet.All)
        {
            IEnumerable<Category> categories = await GetCategoriesAsync(set);
            return categories.Select(c => c.Name);
        }

        public async Task UpdateOrAddCategoryAsync(Category s, CategoryKeepActive keepIsActive)
        {
            List<Category> results = await database.QueryAsync<Category>("select * from Category where Name='" + s.Name + "'");
            Category current = results.FirstOrDefault<Category>();

            if (current != null)
            {
                // If we want to retain the current IsActive flag, copy it from the current record
                // before doing an update.
                if (keepIsActive == CategoryKeepActive.Keep)
                {
                    s.IsActive = current.IsActive;
                }

                await database.UpdateAsync(s);
            }
            else
            {
                await database.InsertAsync(s);
            }
        }

        public Task<int> DeleteCategoryAsync(String name)
        {
            return database.DeleteAsync(new Category() { Name = name });
        }
        #endregion

        #region Item Table        
        public Task<List<Item>> GetItemsAsync()
        {
            return database.Table<Item>().ToListAsync();
        }

        // GetItemListForCategoryAsync queries for item data appropriate for UI display, which is
        // only the title, description, provider, and uri (the primary key). 
        public async Task<IEnumerable<Item>> GetItemListForCategoryAsync(String category, Int32 itemLimit)
        {
            // Note: Using LINQ with SQLite.Net retrieves the full item data regardless of what's indicated
            // in the select clause, so we use the Query method instead to retrieve only partial data.                
            String query = "select Title, Description, Provider, Url from Item where Category='"
                + category + "' order by LastUpdated DESC"
                + " limit " + itemLimit;

            IEnumerable<Item> list = (await database.QueryAsync<Item>(query)).ToList();
            return list;
        }

        public Task<Item> GetItemAsync(String uri)
        {
            var query = database.Table<Item>().Where(x => x.Url == uri);
            return query.FirstOrDefaultAsync();
        }

        public async Task<String> UpdateOrAddItemAsync(Item item)
        {
            Item current = await GetItemAsync(item.Url);

            if (current != null)
            {
                await database.UpdateAsync(item);
                return item.Url;
            }
            else
            {
                await database.InsertAsync(item);
                return item.Url;
            }
        }

        public Task<int> DeleteItemAsync(String url)
        {
            return database.DeleteAsync(new Item() { Url = url });
        }

        // Enforces keeping only a limited number of items per category in the table.
        public async Task ApplyConversationLimitAsync(Int32 limit)
        {
            var categories = await GetCategoryNamesAsync();

            foreach (String c in categories)
            {
                AsyncTableQuery<Item> catItems;

                catItems = database.Table<Item>().Where(item => item.Category == c);

                Int32 itemsToDelete = (await catItems.CountAsync() - limit);

                if (itemsToDelete > 0)
                {
                    // We need to delete some, so sort by descending age and then knock off whatever is
                    // at the position of limit until we get the count down.
                    List<Item> sorted = await catItems.OrderByDescending<DateTime>(item => item.LastUpdated).ToListAsync();

                    for (var i = 0; i < itemsToDelete; i++)
                    {
                        await database.DeleteAsync(sorted.ElementAt(limit).Url);
                    }
                }
            }
        }
        #endregion

        #region UserDetailsData Table
        public Task<UserDetailsData> GetUserDetailsData()
        {
            return database.Table<UserDetailsData>().FirstOrDefaultAsync();
        }

        public async Task SetUserDetailsData(UserDetailsData userDetailsData)
        {
            var current = await GetUserDetailsData();
            if (current == null)
            {
                await database.InsertAsync(userDetailsData);
            }
            else
            {

                string query = "INSERT INTO `UserDetailsData`(`UserId`,`FirstName`,`LastName`,`Email`,`Title`,`Location`," +
                                "'SAPUserId','PhoneNumber') VALUES ('" +
                                userDetailsData.UserId + "'" + "," + "'" +
                                userDetailsData.FirstName + "'" + "," + "'" +
                                userDetailsData.LastName + "'" + "," + "'" +
                                userDetailsData.Email + "'" + "," + "'" +
                                userDetailsData.Title + "'" + "," + "'" +
                                userDetailsData.Location + "'" + "," + "'" +
                                userDetailsData.SAPUserId + "'" + "," + "'" +
                                userDetailsData.PhoneNumber + "'" + ")";
                await database.QueryAsync<UserDetailsData>(query);
            }
        }
        #endregion
    }
}
