using System;
using System.Collections.Generic;
using Smartdocs.Models;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class OutboxMainView : ContentView
	{
		public List<LineItem> main_data;

		public OutboxMainView()
		{
			InitializeComponent();

			var activeItem = App.G_CURRENT_COM_ACTIVE_ITEM;

			//confgigure main table data
			main_data = new List<LineItem>();

			foreach (DocType doctype_item in App.G_DOC_ITEMS)
			{
				if (App.G_DocType.Equals(doctype_item.docTypeName))
				{
					foreach (DataField datafield_item in doctype_item.dataFields)
					{
						if (datafield_item.LineItemType.Equals(""))
						{
							var data_value = activeItem.headerData.getValue(datafield_item.FieldName);//get value according to field name
							var lineitem = new LineItem
							{
								FieldType = datafield_item.DataType,
								BarcodeField = datafield_item.BarCodeField,
								VisibleLength = datafield_item.VisibleLength,
								FieldName = datafield_item.FieldName,
								Material = datafield_item.Label,
								Amount = data_value,
								DateData = DateTime.Now.ToLocalTime()
							};
							main_data.Add(lineitem);
						}
					}
				}
			}
			PopulateList(main_data);
		}

		private void PopulateList(List<LineItem> list)
		{
			var column = Row;
			column.Children.Clear();

			for (var i = 0; i < list.Count; i++)
			{
				App.req_inbox_VisibleLength = list[i].VisibleLength;

				var item = new LinesViewItemTemplate(false);

				item.BindingContext = list[i];
				column.Children.Add(item);
			}
		}
	}
}