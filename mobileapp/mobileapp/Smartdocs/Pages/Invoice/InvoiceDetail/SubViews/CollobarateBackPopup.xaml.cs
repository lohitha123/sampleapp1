using Acr.UserDialogs;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Smartdocs.Models;
using Smartdocs.SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Smartdocs.Pages.Invoice.InvoiceDetail.SubViews
{
    public partial class CollobarateBackPopup : PopupPage
    {
        // TODO:Class level varriable declaration.
        public event EventHandler DialogClosed;
        public event EventHandler DialogShow;
        public event EventHandler DialogClosing;
        public event EventHandler DialogShowing;
        public static readonly BindableProperty HeaderTitleProperty = BindableProperty.Create("HeaderTitle", typeof(string), typeof(PopupView), string.Empty, BindingMode.TwoWay);
        public List<LineItem> main_data;
        List<LineItem> sorted_mainitem;
        string selectedUser;
        string selectUserId = "0";

        public string HeaderTitle
        {
            get { return (string)GetValue(HeaderTitleProperty); }
            set { SetValue(HeaderTitleProperty, value); }
        }
        public CollobarateBackPopup()
        {
            InitializeComponent();


            

            var activeItem = App.G_CURRENT_ACTIVE_ITEM;

            //TODO:Inialize listview. 

            PopUpBgLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(HideDialog)
            });

            PopUpDialogClose.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(HideDialog)
            });

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
            ButtonSend.Clicked += OnCollobarateButtonClicked;
        }

        async void OnCollobarateButtonClicked(Object sender, EventArgs e)
        {
            var pc = new PromptConfig();
            pc.Title = "Please enter comment";
            pc.Placeholder = "Please enter comment";

            if (selectedUser == "Select User")
            {
                UserDialogs.Instance.AlertAsync("Please Select User!", "Error!", "OK", null);
                return;
            }
            else if (string.IsNullOrEmpty(entryUserComment.Text))
            {
                UserDialogs.Instance.AlertAsync("Please Enter Comment!", "Error!", "OK", null);
                return;
            }

            App.approveComment = entryUserComment.Text;
            submitWorkItem(sender, "CollaborateBack", App.approveComment);
            App.approveComment = "";
            HideDialog();
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
                    FolderName = "folder1",
                    URL = uploadFileResult
                };
                attachmentdata.Add(add_attach_data);
            }

            // TODO : Store Collobarate Selected User in list.
            var selColUsers = new Users();
            selColUsers.User = new List<string>();
            selColUsers.User.Add(selectUserId);

            var submitWorkitemData = new SubmitWorkItem
            {
                workitemId = activeItem.workItemId,
                adminData = admin_data,
                headerData = header_data,
                logs = logdata,
                activities = activitydata,
                attachments = attachmentdata,
                lineitemData = activeItem.lineitemData,
                collaborated_Users = selColUsers
            };

            if (CrossConnectivity.Current.IsConnected)
            {
                ((Button)sender).IsEnabled = false;

                UserDialogs.Instance.ShowLoading("Sending...");
                var result = await App.G_HTTP_CLIENT.SubmitWorkItemAsync(submitWorkitemData, Constants.SubmitWorkitem_API);
                UserDialogs.Instance.HideLoading();

                ((Button)sender).IsEnabled = true;

                if (result == null)
                {
                    await UserDialogs.Instance.AlertAsync("", "Failed to Submit workitem. Please try again", "OK", null);
                }
                else
                {
                    if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created)
                    {
                        System.Diagnostics.Debug.WriteLine("success!");

                        App.G_WORK_ITEMS.Remove(App.G_CURRENT_ACTIVE_ITEM);//remove current workitem in all workitems
                        App.G_COMPLETE_WORK_ITEMS.Add(App.G_CURRENT_ACTIVE_ITEM);//add current workitem to completedworkitems
                                                                                 //await Navigation.PushAsync(new InvoicePage());
                        PopupView.isBack = true;
                        //await Navigation.PushModalAsync(new InvoicePage());
                        await Navigation.PopAsync();

                        await UserDialogs.Instance.AlertAsync("", "Workitem Action submitted Sucessfully", "OK", null);
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("", "Failed to submit workitem Action", "OK", null);
                    }
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Your action will be done once you are online", "You are offline!", "OK", null);
                App.G_WORK_ITEMS.Remove(App.G_CURRENT_ACTIVE_ITEM);
                await Navigation.PopAsync();

                var dbInit = new DataAccessLayer(null);
                await dbInit.SetWorkitemStatus(activeItem.workItemId, Constants.PENDING);

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

        public void ShowDialog()
        {
            ShowDialogAnimation(PopUpDialogLayout, PopUpBgLayout);
        }

        public void HideDialog()
        {
            HideDialogAnimation(PopUpDialogLayout, PopUpBgLayout);
        }

        //public View DialogContent
        //{
        //    get { return ContentView.Content; }
        //    set { ContentView.Content = value; }
        //}

        protected virtual void OnDialogClosed(EventArgs e)
        {
            DialogClosed?.Invoke(this, e);
        }

        protected virtual void OnDialogShow(EventArgs e)
        {
            DialogShow?.Invoke(this, e);
        }

        protected virtual void OnDialogClosing(EventArgs e)
        {
            DialogClosing?.Invoke(this, e);
        }

        protected virtual void OnDialogShowing(EventArgs e)
        {
            DialogShowing?.Invoke(this, e);
        }

        private void ShowDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            dialog.TranslationY = bg.Height;
            bg.IsVisible = true;
            dialog.IsVisible = true;

            ////ANIMATIONS 
            var showBgAnimation = OpacityAnimation(bg, 0, 0.5);
            var showDialogAnimation = TransLateYAnimation(dialog, bg.Height, 0);

            ////EXECUTE ANIMATIONS
            this.Animate("showBg", showBgAnimation, 16, 200, Easing.Linear, (d, f) => { });
            this.Animate("showMenu", showDialogAnimation, 16, 200, Easing.Linear, (d, f) =>
            {
                OnDialogShow(new EventArgs());
            });

            OnDialogShowing(new EventArgs());
        }

        private async void HideDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            //ANIMATIONS     
            var hideBgAnimation = OpacityAnimation(bg, 0.5, 0);
            var showDialogAnimation = TransLateYAnimation(dialog, 0, bg.Height);

            ////EXECUTE ANIMATIONS
            this.Animate("hideBg", hideBgAnimation, 16, 200, Easing.Linear, (d, f) => { });
            this.Animate("hideMenu", showDialogAnimation, 16, 200, Easing.Linear, (d, f) =>
            {
                bg.IsVisible = false;
                dialog.IsVisible = false;
                dialog.TranslationY = PopUpBgLayout.Height;

                OnDialogClosing(new EventArgs());
            });
            // await Navigation.PopAllPopupAsync(true);
            await Navigation.PopAllPopupAsync();

            OnDialogClosing(new EventArgs());
        }

        private static Animation TransLateYAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationY = d; }, from, to);
        }

        private static Animation TransLateXAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationX = d; }, from, to);
        }

        private static Animation OpacityAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.Opacity = d; }, from, to);
        }
    }
}
