using System;
using System.Threading.Tasks;
using ZXing.Mobile;
using Xamarin.Forms;
using Android;

[assembly: Dependency(typeof(Smartdocs.Droid.BarCodeScanner))]

namespace Smartdocs.Droid
{
	public class BarCodeScanner : BarCodeScannerService
	{
		public BarCodeScanner ()
		{
			
		}

		public async Task<string> ScanAsync()
		{
			var scanner = new ZXing.Mobile.MobileBarcodeScanner ();
			var scanResult = await scanner.Scan ();

			if (scanResult != null)
				return scanResult.Text;
			else
				return "";
		}
	}
}

