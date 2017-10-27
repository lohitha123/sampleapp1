using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Smartdocs.Models;

namespace Smartdocs
{
	public partial class OutboxLineView : ContentView
	{
		public OutboxLineView()
		{
			InitializeComponent();

			var activeItem = App.G_CURRENT_COM_ACTIVE_ITEM;
			var line_data = new List<LineItem>();

			foreach (DocType doctype_item in App.G_DOC_ITEMS)
			{
				if (App.G_DocType.Equals(doctype_item.docTypeName))//get current docType
				{
					foreach (LineItem line_item in activeItem.lineitemData)
					{
						var lineitem = new LineItem
						{
							Material = line_item.Material,
							Amount = line_item.Amount
						};
						line_data.Add(lineitem);
					}
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
				var item = new LinesViewItemTemplate(false);

				item.BindingContext = list[i];
				column.Children.Add(item);
			}

		}
	}
}

