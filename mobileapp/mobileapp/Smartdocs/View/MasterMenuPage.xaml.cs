using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartdocs
{
	public partial class MasterMenuPage : ContentPage
	{
		public ListView ListView { get { return listView; } }

		List<MasterPageItem> masterPageItems;

		public MasterMenuPage ()
		{
			InitializeComponent ();

			masterPageItems = new List<MasterPageItem> ();
			masterPageItems.Add (new MasterPageItem {
				Title = "Inbox",
				IconSource = "inbox.png",
				TargetType = typeof(SmartTaskList)
			});
			//masterPageItems.Add (new MasterPageItem {
			//	Title = "Requests",
			//	IconSource = "requests.png",
			//	TargetType = typeof(SmartTaskList)
			//});
			//masterPageItems.Add (new MasterPageItem {
			//	Title = "Outbox",
			//	IconSource = "outbox.png",
			//	TargetType = typeof(SmartTaskList)
			//});
			masterPageItems.Add (new MasterPageItem {
				Title = "Settings",
				IconSource = "setting.png",
				TargetType = typeof(SmartTaskList)
			});

			masterPageItems.Add (new MasterPageItem {
				Title = "Log out",
				IconSource = "bac.png",
				TargetType = typeof(SmartTaskList)
			});

			listView.ItemsSource = masterPageItems;
		}

		async private void OnItemTapped(Object sender, ItemTappedEventArgs e)
		{
			var selectedItem = ((ListView)sender).SelectedItem;
			int selectedIndex = 0;
			for (int i = 0; i < masterPageItems.Count; i++) 
			{
				string title = ((MasterPageItem)selectedItem).Title;
				if (title.Equals (masterPageItems [i].Title)) {
					selectedIndex = i;
					break;
				} else if (title.Equals ("Log out")) {

					Application.Current.Properties["LoggedIn"] = "false";
					Page page = ((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage;
					if (Device.OS == TargetPlatform.iOS)
					{
						page.Navigation.InsertPageBefore (new Login (), page);
						await page.Navigation.PopAsync ();
					}
					else if (Device.OS == TargetPlatform.Android)
						await Navigation.PushAsync(new Login());
					
					break;
				}
			}

			Element current = this;
			while (current.Parent != null ) {
				current = current.Parent;
				if (current.GetType().Name == "RootPage") {
					break;
				}
			}
			var master = current as MasterDetailPage;
			((TabbedPage)master.Detail).CurrentPage = ((TabbedPage)master.Detail).Children [selectedIndex];
		}
	}
}

