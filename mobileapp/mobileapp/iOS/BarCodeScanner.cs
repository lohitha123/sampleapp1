using System;
using System.Threading.Tasks;
using ZXing.Mobile;
using UIKit;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(Smartdocs.iOS.BarCodeScanner))]

namespace Smartdocs.iOS
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

		public byte[] ReadImageFile(string imageLocation)
		{
			byte[] imageData = null;
			FileInfo fileInfo = new FileInfo(imageLocation);
			long imageFileLength = fileInfo.Length;
			FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
			BinaryReader br = new BinaryReader(fs);
			imageData = br.ReadBytes((int)imageFileLength);
			return imageData;
		}
	}
}

