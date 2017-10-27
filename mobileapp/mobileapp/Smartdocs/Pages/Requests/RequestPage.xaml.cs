using System;
using System.Collections.Generic;
using Smartdocs.Models;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class RequestPage : ContentPage
	{
		public RequestPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private void PopulateList(List<InboxViewModel> list)
		{
			var column = LeftColumn;
			LeftColumn.Children.Clear();
			RightColumn.Children.Clear();

			var itemTapped = new TapGestureRecognizer();
			itemTapped.Tapped += OnItemTapped;

			for (var i = 0; i < list.Count; i++) {
				var item = new InboxItemTemplate();

				if (i % 2 == 0) {
					column = LeftColumn;
				} else {
					column = RightColumn;
				}

				item.GestureRecognizers.Add( itemTapped );
				item.BindingContext = list[i];
				column.Children.Add( item );
			}
		}

		private async void OnItemTapped (Object sender, EventArgs e) {

			var selectedItem = (InboxViewModel)((InboxItemTemplate)sender).BindingContext;

			App.G_PageTitle = selectedItem.Title;
			App.G_DocType = selectedItem.DocType;

			var newinvoice = new NewInvoice();
			await Navigation.PushAsync(newinvoice);
		}

		protected override void OnAppearing()
		{
			
				var inboxModels = new List<InboxViewModel>();

				foreach (DocType item in App.G_DOC_ITEMS)
				{
					if (item.adminData.WorkItemOnly == "")
					{
						var model = new InboxViewModel
						{
							Title = item.adminData.Label,
							Status = item.adminData.SubLabel,
							ImageIcon = "payment.png",
							DocType = item.docTypeName
						};
						inboxModels.Add(model);
					}
				}

				PopulateList(inboxModels);

		}
	}
}

