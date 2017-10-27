using Xamarin.Forms;
using Smartdocs.Custom;
using Smartdocs.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Smartdocs.Models;
using Smartdocs.Pages.Invoice.InvoiceDetail.SubViews;
using Rg.Plugins.Popup.Extensions;
using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Net;
using Smartdocs.SQLite;

namespace Smartdocs
{
    public class InvoiceDetailPage : ContentPage
    {
        private View _tabs;

        private RelativeLayout _relativeLayout;

        private TabbedPageViewModel _viewModel;

        // TODO : Class Level Declartion.
        string approveCommentReq, rejectCommentReq, collaborateCommentReq, collaborateBackCommentReq;
        public List<LineItem> main_data;
        List<LineItem> sorted_mainitem;
        private PopupView popUp;
        private CollobarateBackPopup popUpColBack;
        string docID = string.Empty;

        public InvoiceDetailPage()
        {
            _viewModel = new TabbedPageViewModel();
            BindingContext = _viewModel;
            BackgroundColor = Color.White;

            Title = "Title";

            _relativeLayout = new RelativeLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var pagesCarousel = CreatePagesCarousel();
            _tabs = CreateTabs();
            var tabsHeight = 36;

            _relativeLayout.Children.Add(_tabs,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.Constant(tabsHeight)
            );

            _relativeLayout.Children.Add(pagesCarousel,
                Constraint.RelativeToParent((parent) => { return parent.X; }),
                Constraint.RelativeToParent((parent) => { return tabsHeight; }),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToView(_tabs, (parent, sibling) => { return parent.Height - (sibling.Height); })
            );

            StackLayout stackLayout = new StackLayout
            {
                BackgroundColor = Color.White
            };

            StackLayout menu = new StackLayout
            {
                HeightRequest = Device.OnPlatform(56, 46, 46),
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center
            };

            var backButtonTap = new TapGestureRecognizer();
            backButtonTap.Tapped += OnBackButtonClicked;


            Image backButton = new Image
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 24,
                HeightRequest = 24,
                Source = "back.png",
            };

            backButton.GestureRecognizers.Add(backButtonTap);

            StackLayout menuBarCenter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            Label MenuBarTitle = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 18,
                //Text = App.G_DocType + " " + App.G_DocId,
                Text = "Smartdocs ID " + App.G_DocId,
                TextColor = Color.Black
            };


            Button ButtonApprove = new Button
			{
				Image = "approvebtn.png",
				BackgroundColor = Color.FromHex("#ffffff"),
				FontSize = 16,

				//BorderWidth=0.1,
				IsVisible = false,
                TextColor = Color.FromHex("#ffffff")
            };

          

            Button ButtonReject = new Button
			{
				Image = "rejectbtn.png",
				BackgroundColor = Color.FromHex("#ffffff"),
				FontSize = 16,
				//BorderWidth = 0.1,
				IsVisible = false,
                TextColor = Color.FromHex("#ffffff")
            };

            Button ButtonCollaborate = new Button
			{
				Image = "collaboratebtn.png",
				BackgroundColor = Color.FromHex("#ffffff"),
				FontSize = 16,
				//BorderWidth = 0.1,
				IsVisible = false,
                TextColor = Color.FromHex("#ffffff")
            };

            Button ButtonCollaborateBack = new Button
            {
                Image = "collaborate_backbutton.png",
                BackgroundColor = Color.FromHex("#ffffff"),
                FontSize = 16,
               
                IsVisible = false,
                //BorderWidth = 0.1,
                TextColor = Color.FromHex("#ffffff")
            };


            Grid gridContent = new Grid
            {
               
            };
            gridContent.RowSpacing = 0;
            gridContent.ColumnSpacing = 0;
           // gridContent.Padding = new Thickness(0, 5, 0, 0);
            gridContent.HorizontalOptions = LayoutOptions.CenterAndExpand;
            gridContent.VerticalOptions = LayoutOptions.StartAndExpand;
            gridContent.BackgroundColor = Color.White;
            gridContent.RowDefinitions = new RowDefinitionCollection();
            gridContent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10, GridUnitType.Star) });
            gridContent.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                          new ColumnDefinition(){ Width=new GridLength(1,GridUnitType.Star)},
                           new ColumnDefinition(){ Width=new GridLength(1,GridUnitType.Star)},
                            new ColumnDefinition(){ Width=new GridLength(1,GridUnitType.Star)},
                             new ColumnDefinition(){ Width=new GridLength(1,GridUnitType.Star)},
                              new ColumnDefinition(){ Width=new GridLength(1,GridUnitType.Star)},
                    };

            gridContent.Children.Add(ButtonApprove,1, 0);
            gridContent.Children.Add(ButtonReject, 2, 0);
            gridContent.Children.Add(ButtonCollaborate, 3, 0);
            gridContent.Children.Add(ButtonCollaborateBack, 2, 0);

            menuBarCenter.Children.Add(MenuBarTitle);

            var footerstklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            
            menu.Children.Add(backButton);
            menu.Children.Add(menuBarCenter);

            menu.Padding = new Thickness(10, 38, 10, 10);

            stackLayout.Children.Add(menu);
            _relativeLayout.BackgroundColor = Color.White;
            stackLayout.Children.Add(_relativeLayout);



            var grid = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                
            };

            grid.BackgroundColor = Color.White;
            grid.RowSpacing = 0;

            grid.RowDefinitions = new RowDefinitionCollection
                    {
                                new RowDefinition(){Height=new GridLength(15,GridUnitType.Star)},
                                new RowDefinition(){Height=new GridLength(75,GridUnitType.Star)},
                                 new RowDefinition(){Height=new GridLength(10,GridUnitType.Star)}
                    };
            grid.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                          new ColumnDefinition(){ Width=new GridLength(1,GridUnitType.Star)},

                    };

            grid.Children.Add(menu, 0, 0);
            grid.Children.Add(_relativeLayout, 0, 1);
            grid.Children.Add(gridContent, 0, 2);

            Content = grid;
            NavigationPage.SetHasNavigationBar(this, false);




            // TODO:Confgigure main table data.
            var activeItem = App.G_CURRENT_ACTIVE_ITEM;
            docID = App.G_CURRENT_ACTIVE_ITEM.docId;
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


            //configure active button
            foreach (Activity item in activeItem.activities)
            {
                if (item.ButtonText.Equals("Approve"))
                {
                    ButtonApprove.IsVisible = true;
                    approveCommentReq = item.CommentsRequired;
                }
                else if (item.ButtonText.Equals("Reject"))
                {
                    ButtonReject.IsVisible = true;
                    rejectCommentReq = item.CommentsRequired;
                }
                else if (item.ButtonText.Equals("Collaborate"))
                {
                    ButtonCollaborate.IsVisible = true;
                    collaborateCommentReq = item.CommentsRequired;
                }
                else if (item.ButtonText.Equals("CollaborateBack"))
                {
                    ButtonCollaborateBack.IsVisible = true;
                    collaborateBackCommentReq = item.CommentsRequired;
                }
            }

            ButtonApprove.Clicked += OnApproveButtonClicked;
            ButtonReject.Clicked += OnRejectButtonClicked;
            ButtonCollaborate.Clicked += OnButtonCollobarateClicked;
            ButtonCollaborateBack.Clicked += OnButtonCollobarateBackClicked;



        }

        async void OnBackButtonClicked(Object sender, EventArgs e)
        {
            App.imgByteData = null;
            await Navigation.PopAsync();
        }

        CarouselLayout CreatePagesCarousel()
        {
            var carousel = new CarouselLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(typeof(DynamicTemplateLayout))
            };
            carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Pages");
            carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

            return carousel;
        }

        private View CreateTabsContainer()
        {
            return new StackLayout
            {
                Children = { CreateTabs() }
            };
        }

        private View CreateTabs()
        {
            var pagerIndicator = new PagerIndicatorTabs() { HorizontalOptions = LayoutOptions.CenterAndExpand };
            pagerIndicator.RowDefinitions.Add(new RowDefinition() { Height = 36 });
            //pagerIndicator.SetBinding(PagerIndicatorTabs.ColumnDefinitionsProperty, "Pages", BindingMode.Default, new SpacingConverter());//It is slashed for xamarin.Forms 2.3.1.114
            pagerIndicator.SetBinding(PagerIndicatorTabs.ItemsSourceProperty, "Pages");
            pagerIndicator.SetBinding(PagerIndicatorTabs.SelectedItemProperty, "CurrentPage");

            return pagerIndicator;
        }




        async void OnButtonCollobarateClicked(Object sender, EventArgs e)
        {
            List<UserDetails> lstSetupdatausers = new List<UserDetails>();
            lstSetupdatausers = await App.G_HTTP_CLIENT.GetAllUsersAsync(docID);
            popUp = new PopupView();
            await Navigation.PushAsync(new PopupView(lstSetupdatausers));
        }

        async void OnButtonCollobarateBackClicked(Object sender, EventArgs e)
        {
            popUpColBack = new CollobarateBackPopup();
            await Navigation.PushAsync( new CollobarateBackPopup());
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
                    FolderName="folder1",
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

    public class SpacingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = value as IEnumerable<ICarouselViewModel>;

            var collection = new ColumnDefinitionCollection();
            foreach (var item in items)
            {
                collection.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            return collection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}