using System;
using System.Collections.Generic;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class AddNewFilePage : ContentPage
	{

        private RelativeLayout _relativeLayout;
        public AddNewFilePage()
		{
            try
            {
                InitializeComponent();

				MenuBarlblTitle.Text = "Smartdocs ID " + App.G_DocId;
             
            }
            catch (Exception ex)
            {

                throw;
            }

			BackgroundColor = Color.FromHex("#f2f9fc");
           NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "Back");
           // base.OnBackButtonPressed();


		}


        private async void imgBack_Clicked(object sender,EventArgs e )
        {
            await Navigation.PopAsync();
        } 

        async void OnBackButtonClicked(Object sender, EventArgs e)
        {
            App.imgByteData = null;
            await Navigation.PopAsync();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    try
        //    {
        //       // UpdatePtwyHeaders();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //      //  CatchBlock(ex, "OnBackButtonPressed");
        //        return false;
        //    }

        //}

        private async void takePhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //await NewInvoiceFiles.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {

                Directory = "Sample",
                Name = "ImageTakenByCamera.jpg"
            });

            if (file == null)
                return;

            App.fileName = Path.GetFileName(file.Path);
            App.fileExt = Path.GetExtension(file.Path);

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
        }

        private async void pickPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                //DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();


            if (file == null)
                return;

            App.fileName = Path.GetFileName(file.Path);
            App.fileExt = Path.GetExtension(file.Path);

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);
                App.imgByteData = memoryStream.ToArray();

                file.Dispose();
                if (Device.OS == TargetPlatform.iOS)
                    removeBtn.IsVisible = true; // it is crashed in android

                return stream;
            });
        }

        private void removeBtn_Clicked(object sender, EventArgs e)
        {
            image.Source = null;
            removeBtn.IsVisible = false;
        }

        private void ButtonApprove_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();

        }

        private void ButtonReject_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}

