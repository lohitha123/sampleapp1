using Xamarin.Forms;
using System;
using System.Net;
using Smartdocs.Request.ViewModel;
using Smartdocs.Request;
using Smartdocs.Models;
using System.Collections.Generic;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Smartdocs.SQLite;

namespace Smartdocs
{
	public class NewInvoice : ContentPage
	{
		private View _tabs;

		private RelativeLayout _relativeLayout;

		private TabbedPageViewModel _viewModel;

		public NewInvoice ()
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

			_relativeLayout.Children.Add (_tabs, 
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent (parent => parent.Width),
				Constraint.Constant (tabsHeight)
			);

			_relativeLayout.Children.Add (pagesCarousel,
				Constraint.RelativeToParent ((parent) => { return parent.X; }),
				Constraint.RelativeToParent ((parent) => { return tabsHeight; }),
				Constraint.RelativeToParent ((parent) => { return parent.Width; }),
				Constraint.RelativeToView (_tabs, (parent, sibling) => { return parent.Height - (sibling.Height); })
			);

			StackLayout stackLayout = new StackLayout {
				BackgroundColor = Color.White
			};
			StackLayout menu = new StackLayout
			{
				HeightRequest = Device.OnPlatform(56, 46, 46),
				BackgroundColor = Color.White,
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.Center
			};

			var backButtonTap = new TapGestureRecognizer ();
			backButtonTap.Tapped += OnBackButtonClicked;


			Image backButton = new Image {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = 24,
				HeightRequest = 24,
				Source = "back.png",
			};

			backButton.GestureRecognizers.Add (backButtonTap);

			StackLayout menuBarCenter = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center
			};

			Label MenuBarTitle = new Label {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				FontSize = 18,
				Text = App.G_PageTitle,
				TextColor = Color.Black
			};

			menuBarCenter.Children.Add (MenuBarTitle);

			Button sendButton = new Button {
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Center,
				Text = "Submit",
				FontAttributes = FontAttributes.Bold,
				HeightRequest = 18
			};

			sendButton.Clicked += OnSendButtonClicked;

			menu.Children.Add (backButton);
			menu.Children.Add (menuBarCenter);
			menu.Children.Add (sendButton);

			menu.Padding = new Thickness (10, 38, 10, 10);

			stackLayout.Children.Add (menu);
			_relativeLayout.BackgroundColor = Color.White;
			stackLayout.Children.Add (_relativeLayout);
			Content = stackLayout;
			NavigationPage.SetHasNavigationBar(this, false);
		}

		async void OnBackButtonClicked(Object sender, EventArgs e) {
			await Navigation.PopAsync ();
		}

		async void OnSendButtonClicked(Object sender, EventArgs e) {

			var current_date = Constants.getDate();
			var current_time = Constants.getTime();
			IDictionary<string, object> properties = Application.Current.Properties;

			var admin_data = new SubmitRequestAdminData
			{
				DocumentType = App.G_DocType,
				Location = "test location"
			};

			var header_data = new HeaderData();
			foreach (LineItem lineitem in App.requestMainItem)
			{
				if ((lineitem.Mandatory != null) && lineitem.Mandatory.Equals("X"))
				{
					if (lineitem.FieldType.Equals("Date") && lineitem.DateData.Equals(""))
					{
						await DisplayAlert("Warning", "Please fill out this field \n" + lineitem.Material, "Ok");
						return;
					}
					else if (!lineitem.FieldType.Equals("Date") && lineitem.Amount.Equals(""))
					{
						await DisplayAlert("Warning", "Please fill out this field \n" + lineitem.Material, "Ok");
						return;
					}

				}

				if (lineitem.FieldType.Equals("Date"))
					header_data.setValue(lineitem.FieldName, lineitem.DateData.ToString());
				else
					header_data.setValue(lineitem.FieldName, lineitem.Amount);
			}

			var logdata = new Log
			{
				User = properties["userId"].ToString(),
				Date = current_date,
				Time = current_time,
				Activity = "Approved",
				Comments = App.requestComment
			};
		
			string uploadFileResult = await App.G_HTTP_CLIENT.uploadImage(App.imgByteData);
			var attachmentdataList = new List<Attachment>
			{
				new Attachment {
					Name = "Invoice",
					Type = "img",
					URL = uploadFileResult
				}
			};

			var linedataList = new List<LineItem>
			{
				new LineItem {
					Amount = "5464",
					Material = "test"
				}
			};

			var submitWorkitemData = new SubmitRequest
			{
				newRequestId = "NEW"+current_date+current_time,
				adminData = admin_data,
				headerData = header_data,
				logs = logdata,
				attachments = attachmentdataList,
				lineitemData = linedataList
			};

			if (CrossConnectivity.Current.IsConnected)
			{
				((Button)sender).IsEnabled = false;

				UserDialogs.Instance.ShowLoading("Sending...");
				var result = await App.G_HTTP_CLIENT.SubmitRequestAsync(submitWorkitemData, Constants.SubmitRequest_API);
				UserDialogs.Instance.HideLoading();

				Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
				{
					((Button)sender).IsEnabled = true;
					if (result == null)
					{
						await DisplayAlert("Error", "Failed to send request.", "Ok");
						((Button)sender).IsEnabled = true;
					}
					else {
						if (result.StatusCode == HttpStatusCode.Created)
						{
							var resultString = result.Content.ReadAsStringAsync().Result;
							await DisplayAlert("", "Workitem Action submitted Successfully \n" + resultString, "OK");
							((Button)sender).IsEnabled = true;
							await Navigation.PopAsync();
						}
						else {
							await DisplayAlert("Warning", "Failed to submit workitem Action", "Ok");
							((Button)sender).IsEnabled = true;
						}
					}
				});
			}
			else
			{
				SetSubmitRequestDataSQLDB(submitWorkitemData);
				await UserDialogs.Instance.AlertAsync("Your action will be done once you are online", "You are offline!", "OK", null);
				await Navigation.PopAsync();
			}
		}

		CarouselLayout1 CreatePagesCarousel ()
		{
			var carousel = new CarouselLayout1
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(typeof(DynamicTemplateLayout))
			};
			carousel.SetBinding(CarouselLayout1.ItemsSourceProperty, "Pages");
			carousel.SetBinding(CarouselLayout1.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

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
			//pagerIndicator.SetBinding(PagerIndicatorTabs.ColumnDefinitionsProperty, "Pages", BindingMode.Default, new SpacingConverter());
			pagerIndicator.SetBinding(PagerIndicatorTabs.ItemsSourceProperty, "Pages");
			pagerIndicator.SetBinding(PagerIndicatorTabs.SelectedItemProperty, "CurrentPage");

			return pagerIndicator;
		}

		async void SetSubmitRequestDataSQLDB(SubmitRequest requestData)
		{
			var dbInit = new DataAccessLayer(null);

			var submitReqAdminData = new SubmitRequestData();
			submitReqAdminData.newRequestId = requestData.newRequestId;
			submitReqAdminData.DocumentType = requestData.adminData.DocumentType;
			submitReqAdminData.Location = requestData.adminData.Location;
			await submitReqAdminData.Save();

			var rHeaderData = new SQLHeaderData();
			rHeaderData.Kind = Constants.REQUEST;
			await rHeaderData.Save();

			var rLogData = new LogData();
			rLogData.WorkitemID = requestData.newRequestId;
			rLogData.User = requestData.logs.User;
			rLogData.Comments = requestData.logs.Comments;
			rLogData.Activity = requestData.logs.Activity;
			rLogData.Time = requestData.logs.Time;
			rLogData.Date = requestData.logs.Date;
			rLogData.Kind = Constants.REQUEST;
			await rLogData.Save();

			foreach (Attachment attach_item in requestData.attachments)
			{
				var rAttachData = new AttachmentData();
				rAttachData.WorkitemID = requestData.newRequestId;
				rAttachData.Name = attach_item.Name;
				rAttachData.URL = attach_item.URL;
				rAttachData.Type = attach_item.Type;
				rAttachData.Kind = Constants.REQUEST;
				await rAttachData.Save();
			}

			foreach (LineItem line_item in requestData.lineitemData)
			{
				var rLineItemData = new LineItemData();
				rLineItemData.WorkitemID = requestData.newRequestId;
				rLineItemData.Amount = line_item.Amount;
				rLineItemData.Material = line_item.Material;
				rLineItemData.Kind = Constants.REQUEST;
				await rLineItemData.Save();
			}
		}
	}



}


