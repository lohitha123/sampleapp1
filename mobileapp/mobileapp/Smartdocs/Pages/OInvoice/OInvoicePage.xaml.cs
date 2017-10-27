using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Smartdocs.Models;
using Smartdocs.SQLite;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class OInvoicePage : ContentPage
	{
		public OInvoicePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private void PopulateList(List<InvoiceModel> list)
		{
			var column = InvoiceRow;
			column.Children.Clear();

			var invoiceItemTapGestureRecognizer = new TapGestureRecognizer();
			invoiceItemTapGestureRecognizer.Tapped += OnInvoiceTapped;

			for (var i = 0; i < list.Count; i++)
			{
				var item = new InvoiceItemTemplate();
				item.BindingContext = list[i];
				item.GestureRecognizers.Add(invoiceItemTapGestureRecognizer);
				column.Children.Add(item);
			}

		}

		private async void OnInvoiceTapped(Object sender, EventArgs e)
		{

			var selectedItem = (InvoiceModel)((InvoiceItemTemplate)sender).BindingContext;
			WorkItem viewItem = new WorkItem();
			foreach (WorkItem item in App.G_COMPLETE_WORK_ITEMS)
			{
				if (selectedItem.InvoiceID.Equals(Constants.removeZeroFromNumber(item.docId)))
				{
					viewItem = item;
					break;
				}
			}
			App.G_CURRENT_COM_ACTIVE_ITEM = viewItem;
			App.G_DocId = selectedItem.InvoiceID;
			App.displayMode = false;

			await Navigation.PushAsync(new OInvoiceDetailPage());
		}

		protected async override void OnAppearing()
		{

			//pagelabel.Text = App.G_DocType;

			if (App.completedworkitemloaded == false)
			{
				if (CrossConnectivity.Current.IsConnected)
				{
					UserDialogs.Instance.ShowLoading("Loading...");
					App.completedworkitemloaded = true;
					//App.G_COMPLETE_WORK_ITEMS = new List<WorkItem>();//item can be added from inbox page by approving or rejecting, so it is slashed
					App.G_COMPLETE_WORK_ITEMS = await App.G_HTTP_CLIENT.GetAllWorkItemsAsync(Constants.GETCOMPLETRWORKITEM_API);
					UserDialogs.Instance.HideLoading();

					if (App.G_COMPLETE_WORK_ITEMS == null) {
						goToOfflineMode();
					}else
						SetSQLiteDB();
				}
				else {
					goToOfflineMode();
				}

			}

			List<InvoiceModel> invoiceModels = new List<InvoiceModel>();

			foreach (WorkItem item in App.G_COMPLETE_WORK_ITEMS)
			{

				var currentDocType = item.adminData.DocumentType;
				if (App.G_DocType.Equals(currentDocType))
				{

					string date = "";
					if (!String.IsNullOrEmpty(item.adminData.WorkitemDate))
					{
						date = Constants.getDateFromFormat(item.adminData.WorkitemDate);
					}

					string budget = "";
					if (!String.IsNullOrEmpty(item.headerData.Budgeted_Amount))
					{
						budget = "$" + item.headerData.Budgeted_Amount;
					}

					InvoiceModel model = new InvoiceModel
					{
						WorkitemTitle = item.adminData.WorkitemTitle,
						InvoiceID = Constants.removeZeroFromNumber(item.docId),//workitemid
						//From = item.headerData.Company_name,
						//Price = budget,
						//Date = date
					};
					invoiceModels.Add(model);
				}
			}

			PopulateList(invoiceModels);
		}

		async void goToOfflineMode()
		{
			App.G_COMPLETE_WORK_ITEMS = new List<WorkItem>();
			var dbInit = new DataAccessLayer(null);
			App.G_COMPLETE_WORK_ITEMS = await dbInit.PullWorkitemData("Completed");
			App.completedworkitemloaded = true;
		}

		async void SetSQLiteDB()
		{

			var dbInit = new DataAccessLayer(null);

			foreach (WorkItem item in App.G_COMPLETE_WORK_ITEMS)
			{
				var wAdminData = new WorkitemAdminData();
				wAdminData.WorkitemID = item.workItemId;
				wAdminData.DocID = item.docId;
				wAdminData.DocumentType = item.adminData.DocumentType;
				wAdminData.SAPWorkItem = item.adminData.SAPWorkItem;
				wAdminData.Mode = item.adminData.Mode;
				wAdminData.WorkitemTitle = item.adminData.WorkitemTitle;
				wAdminData.WorkitemDate = item.adminData.WorkitemDate;
				wAdminData.Status = "Completed";
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
					await wLogData.Save();
				}

				foreach (Attachment attach_item in item.attachments)
				{
					var wAttachData = new AttachmentData();
					wAttachData.WorkitemID = item.workItemId;
					wAttachData.DocID = item.docId;
					wAttachData.Name = attach_item.Name;
					wAttachData.URL = attach_item.URL;
					wAttachData.Type = attach_item.Type;
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
					await wLineItemData.Save();
				}
			}
		}

		async void GetSQLiteDB()
		{
			var dbInit = new DataAccessLayer(null);
			await dbInit.PullWorkitemData("Completed");

		}
	}
}

