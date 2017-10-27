using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smartdocs.Models;
using Xamarin.Forms;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Smartdocs.SQLite;
using FormsBackgrounding.Messages;

namespace Smartdocs
{
    public partial class InboxPage : ContentPage
    {
        //public DocumentType dType { get; private set; }

        public InboxPage()
        {
            InitializeComponent();
            /*List<InboxViewModel> inboxModels = new List<InboxViewModel> {
				new InboxViewModel {
					Title = "Invoice",
					Status = "Approval",
					ImageIcon = "invoice.png",
					PageType = typeof(InvoicePage)
				},
				new InboxViewModel {
					Title = "Purchase Order",
					Status = "Approval",
					ImageIcon = "purchase_order.png",
					PageType = typeof(PurchaseOrder)
				},
				new InboxViewModel {
					Title = "Purchase Request",
					Status = "Approval",
					ImageIcon = "purchase_request.png",
					PageType = typeof(PurchaseRequest)
				},
				new InboxViewModel {
					Title = "Material Master",
					Status = "Request",
					ImageIcon = "request.png",
					PageType = typeof(MaterialMaster)
				}
			};

			PopulateList (inboxModels);*/
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void PopulateList(List<InboxViewModel> list)
        {
            var column = LeftColumn;
            LeftColumn.Children.Clear();
            RightColumn.Children.Clear();

            var inboxItemTapped = new TapGestureRecognizer();
            inboxItemTapped.Tapped += OnItemTapped;

            for (var i = 0; i < list.Count; i++)
            {
                var item = new InboxItemTemplate();

                if (i % 2 == 0)
                {
                    column = LeftColumn;
                }
                else
                {
                    column = RightColumn;
                }
                item.BindingContext = list[i];
                item.GestureRecognizers.Add(inboxItemTapped);
                column.Children.Add(item);
            }

        }

        private async void OnItemTapped(Object sender, EventArgs e)
        {
            var selectedItem = (InboxViewModel)((InboxItemTemplate)sender).BindingContext;
            try
            {

                var page = (Page)Activator.CreateInstance(selectedItem.PageType);
                App.G_DocType = selectedItem.DocType;
                await Navigation.PushAsync(page);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Test", ex.ToString());
            }

        }

        public void ShowToast(string toastMessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(toastMessage).SetDuration(TimeSpan.FromSeconds(10)));
        }

        protected async override void OnAppearing()
        {

            App.G_ARRAY_DOCTYPE = new List<string>();
            App.G_ARRAY_DOCTYPE_ForRequest = new List<string>();

            if (App.doctypeloaded == false)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Debug.WriteLine("You are online");
                    UserDialogs.Instance.ShowLoading("Loading...");

                    App.G_DOC_ITEMS = new List<DocType>();
                    App.G_DOC_ITEMS = await App.G_HTTP_CLIENT.GetDocTypeAsync();

                    if (App.G_DOC_ITEMS == null)
                    {
                        UserDialogs.Instance.HideLoading();
                        goToOfflineModew();
                    }
                    else
                    {
                        App.doctypeloaded = true;

                        App.G_WORK_ITEMS = new List<WorkItem>();
                        App.G_WORK_ITEMS = await App.G_HTTP_CLIENT.GetAllWorkItemsAsync(Constants.GETWORKITEM_API);

                        if (App.G_WORK_ITEMS == null)
                        {
                            goToOfflineModew();
                        }
                        else
                        {
                            App.workitemloaded = true;
                            App.G_UserDetails = await App.G_HTTP_CLIENT.GetAllUsersAsync();

                            //TODO : This is for counting workitems for each docType.
                            countWorkitemForEachDocType();
                            SetSQLiteDB();
                        }
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    goToOfflineModew();
                }
            }
            else
                //this is for refresh counting when approve or reject
                countWorkitemForEachDocType();

            var inboxModels = new List<InboxViewModel>();

            foreach (DocType item in App.G_DOC_ITEMS)
            {
                if (item.itemCount != 0)//display only if there are workitems
                {
                    var model = new InboxViewModel
                    {
                        Title = item.adminData.Label,
                        Status = item.adminData.SubLabel + "(" + item.itemCount + ")",
                        ImageIcon = "invoice.png",
                        PageType = typeof(InvoicePage),
                        DocType = item.docTypeName
                    };

                    if (item.adminData.WorkItemOnly == "")
                        App.G_ARRAY_DOCTYPE_ForRequest.Add(model.Title);//no use

                    App.G_ARRAY_DOCTYPE.Add(model.Title);//no use
                    inboxModels.Add(model);
                }
            }

            PopulateList(inboxModels);
        }

        private void countWorkitemForEachDocType()
        {
            foreach (DocType item_doctype in App.G_DOC_ITEMS)
            {
                item_doctype.itemCount = 0;
                foreach (WorkItem item_workitem in App.G_WORK_ITEMS)
                {
                    if (item_doctype.docTypeName.Equals(item_workitem.adminData.DocumentType))
                    {
                        item_doctype.itemCount += 1;
                    }
                    // TODO : Assigning UserDetails.
                    item_workitem.userDetailsData = App.G_UserDetails;
                }
            }
        }

        async void goToOfflineModew()
        {
            // Constants.ShowToast("You are offline");
            // Start polling data WHEN offline.
            var start_message = new StartLongRunningTaskMessage();
            MessagingCenter.Send(start_message, "StartLongRunningTaskMessage");

            //GetSQLiteDB
            App.G_WORK_ITEMS = new List<WorkItem>();
            var dbInit = new DataAccessLayer(null);
            await dbInit.PullDocType();
            App.G_WORK_ITEMS = await dbInit.PullWorkitemData("New");
            App.doctypeloaded = true;
            App.workitemloaded = true;

            countWorkitemForEachDocType();
        }

        async void SetSQLiteDB()
        {
            //SQLite initialize
            //string path = "Altostratus.db3";
            //SQLiteAsyncConnection conn = new SQLiteAsyncConnection(path);

            var dbInit = new DataAccessLayer(null);
            await dbInit.InitAsync();

            foreach (DocType item in App.G_DOC_ITEMS)
            {
                var dType = new DocumentType();
                dType.docTypeName = item.docTypeName;
                dType.DocTypeDesc = item.adminData.DocTypeDesc;
                dType.SignaturePad = item.adminData.SignaturePad;
                dType.WorkItemOnly = item.adminData.WorkItemOnly;
                dType.Label = item.adminData.Label;
                dType.SubLabel = item.adminData.SubLabel;
                dType.IconName = item.adminData.IconName;

                await dType.Save();

                foreach (DataField datafield_item in item.dataFields)
                {
                    var dField = new DocumentField();
                    dField.docTypeName = item.docTypeName;
                    dField.FieldName = datafield_item.FieldName;
                    dField.LineItemType = datafield_item.LineItemType;
                    dField.PossibleValues = datafield_item.PossibleValues;
                    dField.DataType = datafield_item.DataType;
                    dField.Length = datafield_item.Length;
                    dField.Mandatory = datafield_item.Mandatory;
                    dField.Order = datafield_item.Order;
                    dField.BarcodeField = datafield_item.BarCodeField;
                    dField.Label = datafield_item.Label;
                    dField.VisibleLength = datafield_item.VisibleLength;

                    await dField.Save();
                }
            }

            int i = 0;
            foreach (WorkItem item in App.G_WORK_ITEMS)
            {

                Debug.WriteLine("========== workitem index: " + i);
                i++;

                var wAdminData = new WorkitemAdminData();
                wAdminData.WorkitemID = item.workItemId;
                wAdminData.DocID = item.docId;
                wAdminData.DocumentType = item.adminData.DocumentType;
                wAdminData.SAPWorkItem = item.adminData.SAPWorkItem;
                wAdminData.Mode = item.adminData.Mode;
                wAdminData.WorkitemTitle = item.adminData.WorkitemTitle;
                wAdminData.WorkitemDate = item.adminData.WorkitemDate;
                wAdminData.Status = "New";
                await wAdminData.Save();

                var wHeaderData = new SQLHeaderData();
                wHeaderData.WorkitemID = item.workItemId;
                wHeaderData.DocID = item.docId;
                wHeaderData.Advance_amount = item.headerData.Advance_amount;
                wHeaderData.Company_name = item.headerData.Company_name;
                wHeaderData.Other_deductions = item.headerData.Other_deductions;
                wHeaderData.Remarks2 = item.headerData.Remarks2;
                wHeaderData.Remarks1 = item.headerData.Remarks1;
                wHeaderData.Nature_of_work = item.headerData.Nature_of_work;
                wHeaderData.Retention = item.headerData.Retention;
                wHeaderData.Fund_Centre_Name = item.headerData.Fund_Centre_Name;
                wHeaderData.Project_site = item.headerData.Project_site;
                wHeaderData.WBS_Element = item.headerData.WBS_Element;
                wHeaderData.Kind = "";

                await wHeaderData.Save();

                foreach (Log log_item in item.logs)
                {
                    var wLogData = new LogData();
                    wLogData.WorkitemID = item.workItemId;
                    wLogData.DocID = item.docId;
                    wLogData.User = log_item.User;
                    wLogData.Comments = log_item.Comments;
                    wLogData.Activity = log_item.Activity;
                    wLogData.Time = log_item.Time;
                    wLogData.Date = log_item.Date;
                    wLogData.Kind = "";
                    await wLogData.Save();
                }

                foreach (Attachment attach_item in item.attachments)
                {
                    var wAttachData = new AttachmentData();
                    wAttachData.WorkitemID = item.workItemId;
                    wAttachData.DocID = item.docId;
                    wAttachData.FolderName = attach_item.FolderName;

                    wAttachData.Name = attach_item.Name;
                    wAttachData.URL = attach_item.URL;
                    wAttachData.Type = attach_item.Type;
                    wAttachData.Kind = "";
                    await wAttachData.Save();
                }

                foreach (Activity activity_item in item.activities)
                {
                    var wActivityData = new ActivityData();
                    wActivityData.WorkitemID = item.workItemId;
                    wActivityData.DocID = item.docId;
                    wActivityData.ActivityName = activity_item.ActivityName;
                    wActivityData.CommentsRequired = activity_item.CommentsRequired;
                    wActivityData.ButtonText = activity_item.ButtonText;
                    wActivityData.Icon = activity_item.Icon;
                    await wActivityData.Save();
                }

                foreach (LineItem line_item in item.lineitemData)
                {
                    var wLineItemData = new LineItemData();
                    wLineItemData.WorkitemID = item.workItemId;
                    wLineItemData.DocID = item.docId;
                    wLineItemData.Amount = line_item.Amount;
                    wLineItemData.Material = line_item.Material;
                    wLineItemData.Kind = "";
                    await wLineItemData.Save();
                }

                if (item.userDetailsData != null)
                {
                    // TODO:Store in UserDetails data.
                    foreach (UserDetails user_item in item.userDetailsData)
                    {
                        var userDetailsData = new UserDetailsData();
                        userDetailsData.UserId = user_item.UserId;
                        userDetailsData.FirstName = user_item.FirstName;
                        userDetailsData.LastName = user_item.LastName;
                        userDetailsData.Email = user_item.Email;
                        userDetailsData.Title = user_item.Title;
                        userDetailsData.Location = user_item.Location;
                        userDetailsData.SAPUserId = user_item.SAPUserId;
                        userDetailsData.PhoneNumber = user_item.PhoneNumber;
                        await userDetailsData.Save();
                    }
                }
            }
        }
    }
}