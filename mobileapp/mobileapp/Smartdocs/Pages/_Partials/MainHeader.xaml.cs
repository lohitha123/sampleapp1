using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartdocs
{
	public partial class MainHeader : ContentView
	{
		public MainHeader ()
		{
			InitializeComponent ();
		}
		public void OnHamburgerIconTapped(Object sender, EventArgs e)
		{
			Element current = this;

			while (current.Parent != null ) {
				current = current.Parent;
				if (current.GetType().Name == "RootPage") {
					break;
				}
			}

			var master = current as MasterDetailPage;

			if (master != null) {
				master.IsPresented = true;
			}
		}

		public void changeHeaderTitle(string title)
		{
			HeadTitle.Text = App.G_DocType;
		}
	}
}

