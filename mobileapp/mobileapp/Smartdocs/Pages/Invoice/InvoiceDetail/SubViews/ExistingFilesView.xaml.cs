using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smartdocs.Pages.Invoice.InvoiceDetail.SubViews
{
   
    public partial class ExistingFilesView : ContentPage
    {
        public ExistingFilesView( List<FileViewModel> lstfiles)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MenuBarlblTitle.Text = "Smartdocs ID " + App.G_DocId;

            PopulateList(lstfiles);
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
                else
                {
                    column = RightColumn;
                }
                item.BindingContext = list[i];

                item.GestureRecognizers.Add(inboxItemTapped);
                column.Children.Add(item);
            }
        }

        private async void OnItemTapped(Object sender, EventArgs e)
        {
            var selectedItem = (FileViewModel)((InboxItemTemplate)sender).BindingContext;
            try
            {
                var fileViewPage = new FileViewPage("", "");
                if (selectedItem.Type.Equals(Constants.PDF) || selectedItem.Type.Equals("PDF"))
                {
                    fileViewPage = new FileViewPage(selectedItem.Url, Constants.PDF);
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        CrossShare.Current.OpenBrowser(selectedItem.Url, new Plugin.Share.Abstractions.BrowserOptions
                        {
                            UseSafariWebViewController = true

                        });
                    }
                    else
                        Navigation.PushAsync(fileViewPage);

                }
                else if (selectedItem.Type.Equals(Constants.SIGN))
                {
                    fileViewPage = new FileViewPage(selectedItem.Url, Constants.SIGN);
                    Navigation.PushAsync(fileViewPage);
                }
                else
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        Uri myUri = new Uri(selectedItem.Url, UriKind.Absolute);
                        Device.OpenUri(myUri);
                    }
                    else
                    {
                        fileViewPage = new FileViewPage(selectedItem.Url, Constants.IMAGE);
                        //Navigation.PushAsync(fileViewPage);
                        CrossShare.Current.OpenBrowser(selectedItem.Url, new Plugin.Share.Abstractions.BrowserOptions
                        {
                            UseSafariWebViewController = true
                        });

                        //System.Diagnostics.Process.Start(@"C:\abc.doc");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Test", ex.ToString());
            }
        }
        private async void imgBack_Clicked(object sender,EventArgs e)
        {
          await  Navigation.PopAsync();
        }
    }
}