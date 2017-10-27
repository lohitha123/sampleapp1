using System;
using System.Collections.Generic;
using Smartdocs.Models;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class NewInvoiceMain : ContentView
	{
		List<LineItem> main_data;

		public NewInvoiceMain ()
		{
			InitializeComponent ();

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
							var lineitem = new LineItem
							{
								FieldType = datafield_item.DataType,
								BarcodeField = datafield_item.BarCodeField,
								VisibleLength = datafield_item.VisibleLength,
								FieldName = datafield_item.FieldName,
								Mandatory = datafield_item.Mandatory,
								Material = datafield_item.Label,//main label
								Amount = "",//main value
								DateData = DateTime.Now.ToLocalTime()//date value
							};
							main_data.Add(lineitem);
						}
					}
				}
			}
			App.requestMainItem = main_data;
			PopulateList(main_data);

			//foreach (string item in App.G_ARRAY_DOCTYPE)
			//{
			//	PickerDocType.Items.Add(item);
			//}
		}

		private void PopulateList(List<LineItem> list)
		{
			var column = Row;
			column.Children.Clear();

			for (var i = 0; i < list.Count; i++)
			{
				App.req_inbox_VisibleLength = list[i].VisibleLength;
				//set barcode field
				if (list[i].BarcodeField.Equals("X"))
					App.barcodeField = true;
				else
					App.barcodeField = false;

				//set datatype - keyboard type
				App.request_field_type = list[i].FieldType;

				var item = new NewLineItemTemplate();
				item.BindingContext = list[i];
				column.Children.Add(item);
			}
		}

	}
}