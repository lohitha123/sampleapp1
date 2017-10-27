using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Plugin.Media;
using System.IO;
namespace Smartdocs
{
	public partial class NewInvoiceFiles : ContentView
	{
		public NewInvoiceFiles ()
		{
			InitializeComponent ();
			BackgroundColor = Color.FromHex ("#f2f9fc");

			takePhoto.Clicked += async (sender, args) =>
			{

				  if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				  {
					//await NewInvoiceFiles.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					MessagingCenter.Send(this, "MyAlertName", "My actual alert content, or an object if you want");
					  return;
				  }

				  var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
				  {

					  Directory = "Sample",
					  Name = "test.jpg"
				  });

				  if (file == null)
					  return;

				  //DisplayAlert("File Location", file.Path, "OK");

				  image.Source = ImageSource.FromStream(() =>
				  {
					  var stream = file.GetStream();
						
						var memoryStream = new MemoryStream();
					    file.GetStream().CopyTo(memoryStream);
					  	App.imgByteData = memoryStream.ToArray();

					  file.Dispose();
						removeBtn.IsVisible = true;
					  return stream;
				  });
			};

			pickPhoto.Clicked += async (sender, args) =>
			{
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					//DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}
				var file = await CrossMedia.Current.PickPhotoAsync();


				if (file == null)
					return;

				image.Source = ImageSource.FromStream(() =>
		  		{
				  var stream = file.GetStream();

					var memoryStream = new MemoryStream();
					file.GetStream().CopyTo(memoryStream);
					App.imgByteData = memoryStream.ToArray();

				  file.Dispose();
					removeBtn.IsVisible = true;
				  return stream;
			  	});

			};

			removeBtn.Clicked += (sender, e) =>
			{
				App.imgByteData = null;
				image.Source = null;
				removeBtn.IsVisible = false;
			};

		}

	}
}

