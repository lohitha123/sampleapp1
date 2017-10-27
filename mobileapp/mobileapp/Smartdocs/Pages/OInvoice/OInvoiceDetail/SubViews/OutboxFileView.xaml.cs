using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Smartdocs.Models;
using System.Diagnostics;

namespace Smartdocs
{
	public partial class OutboxFileView : ContentView
	{
		public OutboxFileView()
		{
			InitializeComponent();

			var fileModels = new List<FileViewModel>();

			foreach (Attachment item in App.G_CURRENT_COM_ACTIVE_ITEM.attachments)
			{
				var model = new FileViewModel
				{
					Title = item.Name,
					Type = item.Type,
					ImageIcon = "jpg64.png",
					Url = item.URL
				};

				fileModels.Add(model);
			}

			PopulateList(fileModels);
		}

		private void PopulateList(List<FileViewModel> list)
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
				else {
					column = RightColumn;
				}
				item.BindingContext = list[i];
				item.GestureRecognizers.Add(inboxItemTapped);
				column.Children.Add(item);
			}
		}

		private void OnItemTapped(Object sender, EventArgs e)
		{
			var selectedItem = (FileViewModel)((InboxItemTemplate)sender).BindingContext;
			try
			{
				if (selectedItem.Type.Equals("pdf") || selectedItem.Type.Equals("doc"))
				{
					downloadPDFFromUrl(selectedItem.Url);

				}
				else
					downloadImageFromUrl(selectedItem.Url);

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Test", ex.ToString());
			}

		}

		void downloadPDFFromUrl(string itemUrl)
		{
			Label header = new Label
			{
				Text = "PDF",
				FontSize = 30,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};

			var webView = new WebView
			{
				Source = new UrlWebViewSource
				{
					//Url = "http://182.156.74.204:8080/smartstore/actiprocess/actiprocess?get&pVersion=0046&contRep=A6&docId=000C291DC2B61EE69597E412968EC9E7&accessMode=r&authId=CN%3DID3&expiration=20140828115528&secKey=MIIBlQYJKoZIhvcNAQcCoIIBhjCCAYICAQExCzAJBgUrDgMCGgUAMAsGCSqGSIb3DQEHATGCAWEwggFdAgEBMBMwDjEMMAoGA1UEAxMDSUQzAgEAMAkGBSsOAwIaBQCgXTAYBgkqhkiG9w0BCQMxCwYJKoZIhvcNAQcBMBwGCSqGSIb3DQEJBTEPFw0xNDA4MjgwOTU1MjhaMCMGCSqGSIb3DQEJBDEWBBR2IS5opwsq7XJaJ5NR8u%2F4HXvUhzCBpwYFKw4DAhswgZ0CQQEkJRHP%2BmN7d8miwTMN55CUSmo3TO8WGCxgY61TX5k%2B7NU4XPf1TULjw3GobwaJX13kquPhfVXk%2BgVy46n4Iw3hAhUBSe%2FQF4BUj%2BpJOF9ROBM4u%2BFEWA8CQQD4mSJbrABjTUWrlnAte8pS22Tq4%2FFPO7jHSqjijUHfXKTrHL1OEqV3SVWcFy5j%2FcqBgX%2Fzm8Q12PFp%2FPjOhh%2BnBC8wLQIVARJXMBniHdaxU8LN7MYcaMK%2Bsr4JAhQ1blE9LzBBi1XPYwXeQOlfpzRApA%3D%3D&sp=true"
					Url = itemUrl
				},
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			// Build the page.
			this.Content = new StackLayout
			{
				Children =
				{
					//header,
					webView
				}
			};
		}

		void downloadImageFromUrl(string itemUrl)
		{
			var webImage = new Image { Aspect = Aspect.AspectFit };

			webImage.Source = ImageSource.FromUri(new Uri(itemUrl));

			// Other examples of how to set the Image Source
			//			webImage.Source = "http://xamarin.com/content/images/pages/forms/example-app.png";
			//
			//			webImage.Source = new UriImageSource {
			//				Uri = new Uri("http://xamarin.com/content/images/pages/forms/example-app.png"),
			//				CachingEnabled = false
			//			};
			//
			//			webImage.Source = new UriImageSource {
			//				Uri = new Uri("http://xamarin.com/content/images/pages/forms/example-app.png"),
			//				CachingEnabled = true,
			//				CacheValidity = new TimeSpan(5,0,0,0)
			//			};

			Content = new StackLayout
			{
				Children = {
					new Label {
						Text = "",
						FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
						FontAttributes = FontAttributes.Bold
					},
					webImage
				},
				Padding = new Thickness(0, 20, 0, 0),
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
		}
	}
}

