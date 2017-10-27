using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Smartdocs.Models;
using System.Diagnostics;
using Plugin.Share;
using System.Threading;
using Smartdocs.Pages.Invoice.InvoiceDetail.SubViews;
using Rg.Plugins.Popup.Extensions;
using Acr.UserDialogs;
using System.Globalization;
using Plugin.Connectivity;
using System.Net;
using Smartdocs.SQLite;
using System.Linq;
using System.ComponentModel;
using Smartdocs.Pages.Inbox;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using NativeTest.Droid;
using Android.Views;
using Android.OS;
#endif

#if __IOS__
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
#endif

namespace Smartdocs
{


    public class FilesData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private string folderName;

        public string FolderName
        {
            get { return folderName; }

            set
            {
                if (folderName != value)
                {
                    folderName = value;
                    OnPropertyChanged("FolderName");
                }

            }
        }
        // public string FolderName { get; set; }
    }
    public partial class FilesView : ContentView
    {
        // TODO : Class Level Declartion.
        string approveCommentReq, rejectCommentReq, collaborateCommentReq, collaborateBackCommentReq;
        public List<LineItem> main_data;
        List<LineItem> sorted_mainitem;
        private PopupView popUp;
        private CollobarateBackPopup popUpColBack;

        private List<FilesData> lstfoldernamedetails = new List<FilesData>();
        private List<FileViewModel> lstfiledetails = new List<FileViewModel>();
        public FilesView()
        {
            try
            {
                InitializeComponent();
                lstfoldernamedetails = new List<FilesData>();
                lstfiledetails = new List<FileViewModel>();
            }
            catch (Exception)
            {

                throw;
            }


            var fileModels = new List<FileViewModel>();

            foreach (Attachment item in App.G_CURRENT_ACTIVE_ITEM.attachments)
            {
                var model = new FileViewModel
                {
                    Title = item.Name,
                    Type = item.Type,
                    FolderName = item.FolderName,
                    ImageIcon = "jpg64.png",
                    Url = item.URL
                };

                fileModels.Add(model);
            }

            if (!App.sign_img_path.Equals(""))
            {
                var signFileModel = new FileViewModel
                {
                    Title = "Sign",
                    Type = "sign",
                    ImageIcon = "jpg64.png",
                    Url = App.sign_img_path
                };
                Debug.WriteLine("sign path" + App.sign_img_path);
                fileModels.Add(signFileModel);
            }

            PopulateList(fileModels);
            lstfiledetails = fileModels;
            AddButton.Clicked += TakePicture;

            // TODO:Confgigure main table data.
            var activeItem = App.G_CURRENT_ACTIVE_ITEM;
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


        }



        async void TakePicture(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddNewFilePage());

            if (Device.OS == TargetPlatform.iOS)
            {
                //iOS related code 
            }
        }


        private void PopulateList(List<FileViewModel> list)
        {
            var filesGroup = list.GroupBy(x => x.FolderName).ToList();
            foreach (var item in filesGroup)
            {
                var foldername = item.Key;
                FilesData flsdat = new FilesData();
                flsdat.FolderName = foldername;
                lstfoldernamedetails.Add(flsdat);
            }

            

            var column = FilesViewRow;
            FilesViewRow.Children.Clear();
        
            var ItemTapped = new TapGestureRecognizer();
            ItemTapped.Tapped += OnGridTapGestureRecognizerTapped;

            for (var i = 0; i < lstfoldernamedetails.Count; i++)
            {
                var item = new FileItemTemplate();

                
                item.BindingContext = lstfoldernamedetails[i];

                item.GestureRecognizers.Add(ItemTapped);
                column.Children.Add(item);
            }

        }

      
        private async void OnGridTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            
            var selectedItem = (FilesData)((FileItemTemplate)sender).BindingContext;

            List<FileViewModel> fileslist = new List<FileViewModel>();

            foreach (var item in lstfiledetails)
            {
                if (item.FolderName == selectedItem.FolderName.ToString())
                {
                    fileslist.Add(item);
                }
            }

            if (fileslist != null && fileslist.Count > 0)
            {
                await Navigation.PushAsync(new ExistingFilesView(fileslist));
            }
        }

    
    }
}

