using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class RootPage : MasterDetailPage
	{
		public RootPage ()
		{
			InitializeComponent ();
            this.BackgroundColor = Color.White;
			NavigationPage.SetHasNavigationBar(this, false);
			masterMenuPage.ListView.ItemSelected += OnItemSelected;

			App.G_ROOT_PAGE = this;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e) 
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null) {
				IsPresented = false;
			}
		}
	}
}

