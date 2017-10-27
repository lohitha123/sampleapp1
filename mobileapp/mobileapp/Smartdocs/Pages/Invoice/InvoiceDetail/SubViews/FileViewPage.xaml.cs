using System;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class FileViewPage : ContentPage
	{
		public FileViewPage(string url, string kind)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "Back");

			if (kind.Equals(Constants.PDF))
				downloadPDFFromUrl(url);
			else if (kind.Equals(Constants.SIGN))
				restoreSignImage(url);
			else if (kind.Equals(Constants.IMAGE))
				downloadImageFromUrl(url);
		}

		void downloadPDFFromUrl(string itemUrl)
		{
			var webView = new MyPDFWebView
			{
				Source = new UrlWebViewSource
				{
					//Url = "http://182.156.74.204:8080/smartstore/actiprocess/actiprocess?get&pVersion=0046&contRep=A6&docId=000C291DC2B61EE69597E412968EC9E7&accessMode=r&authId=CN%3DID3&expiration=20140828115528&secKey=MIIBlQYJKoZIhvcNAQcCoIIBhjCCAYICAQExCzAJBgUrDgMCGgUAMAsGCSqGSIb3DQEHATGCAWEwggFdAgEBMBMwDjEMMAoGA1UEAxMDSUQzAgEAMAkGBSsOAwIaBQCgXTAYBgkqhkiG9w0BCQMxCwYJKoZIhvcNAQcBMBwGCSqGSIb3DQEJBTEPFw0xNDA4MjgwOTU1MjhaMCMGCSqGSIb3DQEJBDEWBBR2IS5opwsq7XJaJ5NR8u%2F4HXvUhzCBpwYFKw4DAhswgZ0CQQEkJRHP%2BmN7d8miwTMN55CUSmo3TO8WGCxgY61TX5k%2B7NU4XPf1TULjw3GobwaJX13kquPhfVXk%2BgVy46n4Iw3hAhUBSe%2FQF4BUj%2BpJOF9ROBM4u%2BFEWA8CQQD4mSJbrABjTUWrlnAte8pS22Tq4%2FFPO7jHSqjijUHfXKTrHL1OEqV3SVWcFy5j%2FcqBgX%2Fzm8Q12PFp%2FPjOhh%2BnBC8wLQIVARJXMBniHdaxU8LN7MYcaMK%2Bsr4JAhQ1blE9LzBBi1XPYwXeQOlfpzRApA%3D%3D&sp=true"
					//Url = "http://docs.google.com/viewer?url=" + "http://developer.xamarin.com/guides/cross-platform/getting_started/introduction_to_mobile_development/offline.pdf"
					Url = itemUrl
				},
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			// Build the page.
			Content = new StackLayout
			{
				Children =
				{
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

		void restoreSignImage(string path)
		{
			var topBar = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 5,
				BackgroundColor = Color.Red
			};

			Content = new StackLayout
			{
				Children = {
					topBar,
					new Image { Source = path }
				},
				Padding = new Thickness(10, 10, 10, 5)
			};

		}
	}

	public class CustomWebView : WebView
	{
		public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
				returnType: typeof(string),
				declaringType: typeof(CustomWebView),
				defaultValue: default(string));

		public string Uri
		{
			get { return (string)GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}
	}
}
