using System;
using System.IO;
using System.Net;
using Android.Webkit;
using Smartdocs;
using Smartdocs.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AndroidHUD;

[assembly: ExportRenderer(typeof(MyPDFWebView), typeof(PDFViewRenderer))]
namespace Smartdocs.Droid
{
	public class PDFViewRenderer : WebViewRenderer
	{
		private string _documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		private string _pdfPath;
		private string _pdfFileName = "thePDFDocument.pdf";
		private string _pdfFilePath;
		private string _pdfURL = @"http://122.15.204.158:8083/smartstore/actiprocess/mytrah?get&pVersion=0046&contRep=A7&docId=F7532D2DCFE51ED6A4BFD2877657CE93&accessMode=r&authId=CN=MEQ&expiration=20140828115528&secKey=MIIBlQYJKoZIhvcNAQcCoIIBhjCCAYICAQExCzAJBgUrDgMCGgUAMAsGCSqGSIb3DQEHATGCAWEwggFdAgEBMBMwDjEMMAoGA1UEAxMDSUQzAgEAMAkGBSsOAwIaBQCgXTAYBgkqhkiG9w0BCQMxCwYJKoZIhvcNAQcBMBwGCSqGSIb3DQEJBTEPFw0xNDA4MjgwOTU1MjhaMCMGCSqGSIb3DQEJBDEWBBR2IS5opwsq7XJaJ5NR8u%2F4HXvUhzCBpwYFKw4DAhswgZ0CQQEkJRHP%2BmN7d8miwTMN55CUSmo3TO8WGCxgY61TX5k%2B7NU4XPf1TULjw3GobwaJX13kquPhfVXk%2BgVy46n4Iw3hAhUBSe%2FQF4BUj%2BpJOF9ROBM4u%2BFEWA8CQQD4mSJbrABjTUWrlnAte8pS22Tq4%2FFPO7jHSqjijUHfXKTrHL1OEqV3SVWcFy5j%2FcqBgX%2Fzm8Q12PFp%2FPjOhh%2BnBC8wLQIVARJXMBniHdaxU8LN7MYcaMK%2Bsr4JAhQ1blE9LzBBi1XPYwXeQOlfpzRApA%3D%3D&sp=true&mobileApp=true";
		private WebClient _webClient = new WebClient();

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		{
			base.OnElementChanged(e);

			var settings = Control.Settings;
			settings.JavaScriptEnabled = true;
			settings.AllowFileAccessFromFileURLs = true;
			settings.AllowUniversalAccessFromFileURLs = true;
			settings.BuiltInZoomControls = true;
			Control.SetWebChromeClient(new WebChromeClient());

			DownloadPDFDocument();
		}

		private void DownloadPDFDocument()
		{
			_pdfPath = _documentsPath + "/PDFView";
			_pdfFilePath = Path.Combine(_pdfPath, _pdfFileName);

			// Check if the PDFDirectory Exists
			if (!Directory.Exists(_pdfPath))
			{
				Directory.CreateDirectory(_pdfPath);
			}
			else {
				// Check if the pdf is there, If Yes Delete It. Because we will download the fresh one just in a moment
				if (File.Exists(_pdfFilePath))
				{
					File.Delete(_pdfFilePath);
				}
			}

			// This will be executed when the pdf download is completed
			_webClient.DownloadDataCompleted += OnPDFDownloadCompleted;
			// Lets downlaod the PDF Document
			var url = new Uri(Control.Url);
			_webClient.DownloadDataAsync(url);
		}

		private void OnPDFDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
		{
			try
			{
				// Okay the download's done, Lets now save the data and reload the webview.
				var pdfBytes = e.Result;
				File.WriteAllBytes(_pdfFilePath, pdfBytes);

				//if (File.Exists(_pdfFilePath))
				//{
				//	var bytes = File.ReadAllBytes(_pdfFilePath);
				//}

				Control.LoadUrl("file:///android_asset/pdfviewer/index.html?file=" + _pdfFilePath);
			}
			catch (Exception se)
			{
				
			}

		}
	}
}
