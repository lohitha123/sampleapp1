using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smartdocs.Models;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class OutboxPage : ContentPage
	{
		public OutboxPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private void PopulateList(List<InboxViewModel> list)
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
				else {
					column = RightColumn;
				}
				item.BindingContext = list[i];
				item.GestureRecognizers.Add(inboxItemTapped);
				column.Children.Add(item);
			}

		}

		private async void OnItemTapped(Object sender, EventArgs e)
		{

			var selectedItem = (InboxViewModel)((InboxItemTemplate)sender).BindingContext;
			try
			{
				var page = (Page)Activator.CreateInstance(selectedItem.PageType);
				App.G_DocType = selectedItem.DocType;
				await Navigation.PushAsync(page);

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Test", ex.ToString());
			}
		}

		//public void OnHamburgerIconTapped(Object sender, EventArgs e)
		//{
		//	Element current = this;

		//	while (current.Parent != null ) {
		//		current = current.Parent;
		//		if (current.GetType().Name == "RootPage") {
		//			break;
		//		}
		//	}

		//	var master = current as MasterDetailPage;

		//	if (master != null) {
		//		master.IsPresented = true;
		//	}
		//}

		protected override void OnAppearing()
		{

				var inboxModels = new List<InboxViewModel>();

				foreach (DocType item in App.G_DOC_ITEMS)
				{
					var model = new InboxViewModel
					{
						Title = item.adminData.Label,
						Status = item.adminData.SubLabel,
						ImageIcon = "invoice.png",
						PageType = typeof(OInvoicePage),
						DocType = item.docTypeName
					};

					inboxModels.Add(model);
				}

				PopulateList(inboxModels);

		}
	}
}

