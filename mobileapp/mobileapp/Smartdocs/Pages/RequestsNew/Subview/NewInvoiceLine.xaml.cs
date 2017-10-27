using System.Collections.Generic;
using Xamarin.Forms;
using Smartdocs.Models;

namespace Smartdocs
{
	public partial class NewInvoiceLine : ContentView
	{
		public NewInvoiceLine()
		{
			InitializeComponent();

			var line_data = new List<LineItem>();

			foreach (DocType doctype_item in App.G_DOC_ITEMS)
			{
				if (App.G_DocType.Equals(doctype_item.docTypeName))//get current docType
				{
						var lineitem = new LineItem
						{
							Material = "Material",
							Amount = ""
						};
						line_data.Add(lineitem);
				}
			}

			PopulateList(line_data);
		}

		private void PopulateList(List<LineItem> list)
		{
			var column = Row;
			column.Children.Clear();

			for (var i = 0; i < list.Count; i++)
			{
				App.barcodeField = false;
				var item = new NewLineItemTemplate();
				item.BindingContext = list[i];
				column.Children.Add(item);
			}

		}
	}
}

