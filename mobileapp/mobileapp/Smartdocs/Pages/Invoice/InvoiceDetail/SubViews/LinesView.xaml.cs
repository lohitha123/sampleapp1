using System.Collections.Generic;
using Xamarin.Forms;

using Smartdocs.Models;
using System;
using Smartdocs.Pages.Invoice.InvoiceDetail.SubViews;
using Rg.Plugins.Popup.Extensions;
using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Net;
using Smartdocs.SQLite;

namespace Smartdocs
{
	public partial class LinesView : ContentView
	{

        string approveCommentReq, rejectCommentReq, collaborateCommentReq, collaborateBackCommentReq;
        public List<LineItem> main_data;
        List<LineItem> sorted_mainitem;
        private PopupView popUp;
        private CollobarateBackPopup popUpColBack;

        public LinesView ()
		{
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

                throw;
            }

			var activeItem = App.G_CURRENT_ACTIVE_ITEM;
			var line_data = new List<LineItem>();

			foreach (DocType doctype_item in App.G_DOC_ITEMS)
			{
				if (App.G_DocType.Equals(doctype_item.docTypeName))//get current docType
				{
					foreach (LineItem line_item in activeItem.lineitemData)
					{
						var lineitem = new LineItem
						{
							Material = line_item.Material,
							Amount = line_item.Amount
						};
						line_data.Add(lineitem);
					}
				}
			}

			if (line_data.Count == 0)
			{
				LineItemHeader.IsVisible = false;
				LineItemEmptyLabel.IsVisible = true;
			}

			PopulateList(line_data);



         
        
    }

		private void PopulateList(List<LineItem> list)
		{
			var column = Row;
			column.Children.Clear();

			for (var i = 0; i < list.Count; i++) {
				var item = new LinesViewItemTemplate (false);

				item.BindingContext = list[i];
				column.Children.Add( item );
			}

		}


        async void OnButtonCollobarateClicked(Object sender, EventArgs e)
        {
            popUp = new PopupView();
            await Navigation.PushPopupAsync(popUp, true);
        }

        async void OnButtonCollobarateBackClicked(Object sender, EventArgs e)
        {
            popUpColBack = new CollobarateBackPopup();
            await Navigation.PushPopupAsync(popUpColBack, true);
        }

        async void OnApproveButtonClicked(Object sender, EventArgs e)
        {
            if (approveCommentReq.Equals("X"))
            {
                if (String.IsNullOrEmpty(App.approveComment))
                {
                    var pc = new PromptConfig();
                    pc.Title = "Please enter comment";
                    pc.Placeholder = "Please enter comment";
                    var result = await UserDialogs.Instance.PromptAsync(pc);
                    if (result.Ok)
                    {
                        if (string.IsNullOrEmpty(result.Text))
                        {
                            await UserDialogs.Instance.AlertAsync("Comment can't be blank", "OK", null);
                            OnApproveButtonClicked(sender, e);
                        }
                        else
                        {
                            App.approveComment = result.Text;
                            submitWorkItem(sender, "Approve", App.approveComment);
                            App.approveComment = "";
                        }
                    }
                }
                else
                {
                    submitWorkItem(sender, "Approve", App.approveComment);
                    App.approveComment = "";
                }
            }
            else
            {
                submitWorkItem(sender, "Approve", "");
                App.approveComment = "";
            }
        }

        async void OnRejectButtonClicked(Object sender, EventArgs e)
        {
            if (rejectCommentReq.Equals("X"))
            {
                if (String.IsNullOrEmpty(App.approveComment))
                {
                    var pc = new PromptConfig();
                    pc.Title = "Please enter comment";
                    pc.Placeholder = "Please enter comment";
                    var result = await UserDialogs.Instance.PromptAsync(pc);
                    if (result.Ok)
                    {
                        if (string.IsNullOrEmpty(result.Text))
                        {
                            await UserDialogs.Instance.AlertAsync("Comment can't be blank", "OK", null);
                            OnRejectButtonClicked(sender, e);
                        }
                        else
                        {
                            App.approveComment = result.Text;
                            submitWorkItem(sender, "Reject", App.approveComment);
                            App.approveComment = "";
                        }
                    }
                }
                else
                {
                    submitWorkItem(sender, "Reject", App.approveComment);
                    App.approveComment = "";
                }
            }
            else
            {
                submitWorkItem(sender, "Reject", "");
                App.approveComment = "";
            }
        }
        private async void submitWorkItem(Object sender, string action, string usercomment)
        {
            var activeItem = App.G_CURRENT_ACTIVE_ITEM;

            var admin_data = new SubmitWorkItemAdminData
            {
                docId = activeItem.docId,
                documentType = activeItem.adminData.DocumentType
            };

            var header_data = new HeaderData();
            header_data = activeItem.headerData;

            foreach (LineItem lineitem in main_data)
            {
                header_data.setValue(lineitem.FieldName, lineitem.Amount);
            }

            var activitydata = new Activity
            {
                ActivityName = action,
                ButtonText = action,
                Icon = action,
                CommentsRequired = "true"
            };

            IDictionary<string, object> properties = Application.Current.Properties;
            var logdata = new Log
            {
                User = properties["userId"].ToString(),
                Comments = usercomment,
                Activity = action,
                Date = Constants.getDate(),
                Time = Constants.getTime()
            };

            var attachmentdata = new List<Attachment>();
            attachmentdata = activeItem.attachments;

            if (App.imgByteData != null)
            {
                UserDialogs.Instance.ShowLoading("Sending Image...");
                string uploadFileResult = await App.G_HTTP_CLIENT.uploadImage(App.imgByteData);
                UserDialogs.Instance.HideLoading();

                var add_attach_data = new Attachment
                {
                    Name = App.fileName,
                    Type = App.fileExt,
                    URL = uploadFileResult
                };
                attachmentdata.Add(add_attach_data);
            }

            var submitWorkitemData = new SubmitWorkItem
            {
                workitemId = activeItem.workItemId,
                adminData = admin_data,
                headerData = header_data,
                logs = logdata,
                activities = activitydata,
                attachments = attachmentdata,
                lineitemData = activeItem.lineitemData
            };

            if (CrossConnectivity.Current.IsConnected)
            {
                ((Button)sender).IsEnabled = false;

                UserDialogs.Instance.ShowLoading("Sending...");
                var result = await App.G_HTTP_CLIENT.SubmitWorkItemAsync(submitWorkitemData, Constants.SubmitWorkitem_API);
                UserDialogs.Instance.HideLoading();

                //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                //{
                ((Button)sender).IsEnabled = true;

                if (result == null)
                {
                    await UserDialogs.Instance.AlertAsync("", "Failed to Submit workitem. Please try again", "OK", null);
                }
                else
                {
                    if (result.StatusCode == HttpStatusCode.Created)
                    {
                        System.Diagnostics.Debug.WriteLine("success!");

                        App.G_WORK_ITEMS.Remove(App.G_CURRENT_ACTIVE_ITEM);//remove current workitem in all workitems
                        App.G_COMPLETE_WORK_ITEMS.Add(App.G_CURRENT_ACTIVE_ITEM);//add current workitem to completedworkitems
                        await Navigation.PopAsync();

                        await UserDialogs.Instance.AlertAsync("", "Workitem Action submitted Sucessfully", "OK", null);
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("", "Failed to submit workitem Action", "OK", null);
                    }
                }
                //});
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Your action will be done once you are online", "You are offline!", "OK", null);
                App.G_WORK_ITEMS.Remove(App.G_CURRENT_ACTIVE_ITEM);
                await Navigation.PopAsync();

                var dbInit = new DataAccessLayer(null);
                await dbInit.SetWorkitemStatus(activeItem.workItemId, Constants.PENDING);

                //2016.10.26
                await dbInit.RemoveWorkitemLogData(activeItem.workItemId);
                var cur_logdata = new LogData
                {
                    WorkitemID = activeItem.workItemId,
                    DocID = activeItem.docId,
                    User = logdata.User,
                    Comments = logdata.Comments,
                    Activity = logdata.Activity,
                    Time = logdata.Time,
                    Date = logdata.Date
                };
                await cur_logdata.Save();
            }
        }


    }
}

