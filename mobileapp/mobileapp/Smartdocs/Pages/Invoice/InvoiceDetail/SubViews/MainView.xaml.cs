﻿using System;
using System.Collections.Generic;
using System.Net;
using Xamarin.Forms;
using Smartdocs.Models;
using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Diagnostics;
using System.Globalization;
using Smartdocs.SQLite;
using Smartdocs.Pages.Invoice.InvoiceDetail.SubViews;
using Rg.Plugins.Popup.Extensions;

namespace Smartdocs
{
    public partial class MainView : ContentView
    {
        string approveCommentReq, rejectCommentReq, collaborateCommentReq, collaborateBackCommentReq;
        public List<LineItem> main_data;
        List<LineItem> sorted_mainitem;
        private PopupView popUp;
        private CollobarateBackPopup popUpColBack;
        public static string collaborateMessage;
        public static string selectUserId = "0";

        public MainView()
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

            ////configure active button
            //foreach (Activity item in activeItem.activities)
            //{
            //    if (item.ButtonText.Equals("Approve"))
            //    {
            //        ButtonApprove.IsVisible = true;
            //        approveCommentReq = item.CommentsRequired;
            //    }
            //    else if (item.ButtonText.Equals("Reject"))
            //    {
            //        ButtonReject.IsVisible = true;
            //        rejectCommentReq = item.CommentsRequired;
            //    }
            //    else if (item.ButtonText.Equals("Collaborate"))
            //    {
            //        ButtonCollaborate.IsVisible = true;
            //        collaborateCommentReq = item.CommentsRequired;
            //    }
            //    else if (item.ButtonText.Equals("CollaborateBack"))
            //    {
            //        ButtonCollaborateBack.IsVisible = true;
            //        collaborateBackCommentReq = item.CommentsRequired;
            //    }
            //}

            //ButtonApprove.Clicked += OnApproveButtonClicked;
            //ButtonReject.Clicked += OnRejectButtonClicked;
            //ButtonCollaborate.Clicked += OnButtonCollobarateClicked;
            //ButtonCollaborateBack.Clicked += OnButtonCollobarateBackClicked;


            // TODO:Confgigure main table data.
            main_data = new List<LineItem>();
            sorted_mainitem = new List<LineItem>();

            string tracking = "";

            foreach (DocType doctype_item in App.G_DOC_ITEMS)
            {
                if (App.G_DocType.Equals(doctype_item.docTypeName))
                {
                    foreach (DataField datafield_item in doctype_item.dataFields)
                    {
                        if (datafield_item.LineItemType.Equals(""))
                        {
                            var data_value = activeItem.headerData.getValue(datafield_item.FieldName);//get value according to field name

                            DateTime dt = new DateTime();
                            if (datafield_item.DataType.Equals("Date"))
                            {
                                if (!data_value.Equals("") && !data_value.Equals("00.00.0000"))
                                {
                                    tracking += data_value.ToString();
                                    tracking += "/";
                                    //dt = System.Convert.ToDateTime(Constants.changeDateFormat(data_value));//it is crashing in s's device
                                    dt = DateTime.ParseExact(Constants.changeDateFormat(data_value), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                                }
                            }

                            var lineitem = new LineItem
                            {
                                Order = datafield_item.Order,
                                FieldType = datafield_item.DataType,
                                BarcodeField = datafield_item.BarCodeField,
                                VisibleLength = datafield_item.VisibleLength,
                                FieldName = datafield_item.FieldName,
                                Material = datafield_item.Label,
                                Amount = data_value,
                                DateData = dt
                            };
                            main_data.Add(lineitem);
                        }
                    }
                }
            }
            //ShowToast(tracking);
            Sort();
            PopulateList(sorted_mainitem);

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

        private void PopulateList(List<LineItem> list)
        {
            var column = Row;
            column.Children.Clear();

            for (var i = 0; i < list.Count; i++)
            {
                App.req_inbox_VisibleLength = list[i].VisibleLength;

                //set datatype - keyboard type
                App.request_field_type = list[i].FieldType;

                //set font size
                App.fontsize = 16;
                if (list[i].Amount != null)
                {
                    int value_length = list[i].Amount.Length;
                    if (value_length > 15)
                        App.fontsize = 11;
                }

                ////if there is default date (empty date)
                bool emptyValue = false;
                //if (list[i].FieldType.Equals("Date"))
                //{
                //    if (list[i].DateData.Equals(new DateTime()))
                //    {
                //        emptyValue = true;
                //    }
                //}

                var item = new LinesViewItemTemplate(emptyValue);
                item.BindingContext = list[i];
                column.Children.Add(item);
            }
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

        void PromptComment()
        {
            /*Acr.UserDialogs.ConfirmConfig cc = new Acr.UserDialogs.ConfirmConfig();
			cc.Message = "Are you sure you want to respawn?";
			cc.OkText = "Yes";
			cc.CancelText = "No";
			cc.OnConfirm += delegate (bool result)
			{
				if (result == true)
				{
					
				}
			};
			Device.BeginInvokeOnMainThread(delegate
			{
				Acr.UserDialogs.UserDialogs.Instance.Confirm(cc);
			});*/
        }

        void Sort()
        {
            while (main_data.Count > 0)
            {
                int index = 0;
                int candidate = Convert.ToInt32(main_data[0].Order);

                for (int j = 0; j < main_data.Count; j++)
                {
                    var temp = Convert.ToInt32(main_data[j].Order);
                    if (candidate > temp)
                    {
                        candidate = temp;
                        index = j;
                    }
                }
                sorted_mainitem.Add(main_data[index]);
                main_data.RemoveAt(index);

            }
        }

        void ShowToast(string toastMessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(toastMessage).SetDuration(TimeSpan.FromSeconds(6)));
        }

    }
}

