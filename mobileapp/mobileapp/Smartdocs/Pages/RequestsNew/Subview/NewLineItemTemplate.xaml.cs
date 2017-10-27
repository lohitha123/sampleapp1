using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class NewLineItemTemplate : ContentView
	{
		public NewLineItemTemplate()
		{
			InitializeComponent();

			BarcodeScanner.IsVisible = App.barcodeField;
			var barCodeScannerGesture = new TapGestureRecognizer();
			barCodeScannerGesture.Tapped += OnItemTapped;
			BarcodeScanner.GestureRecognizers.Add(barCodeScannerGesture);

			EntryAmount.Placeholder = App.req_inbox_VisibleLength;
			if (App.request_field_type.Equals("Number"))
				EntryAmount.Keyboard = Keyboard.Numeric;

			if (App.request_field_type.Equals("Date"))
			{
				DatePicker.IsVisible = true;
				EntryAmount.IsVisible = false;
			}
			else { 
				DatePicker.IsVisible = false;
				EntryAmount.IsVisible = true;
			}
		}

		private async void OnItemTapped(Object sender, EventArgs e)
		{
			#if __ANDROID__
			// Initialize the scanner first so it can track the current context
			MobileBarcodeScanner.Initialize (Application);
			#endif
			var BarCode = await DependencyService.Get<BarCodeScannerService>().ScanAsync();

			EntryAmount.Text = BarCode;
		}
	}
}