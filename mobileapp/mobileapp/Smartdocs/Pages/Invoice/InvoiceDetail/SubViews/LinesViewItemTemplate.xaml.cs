using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartdocs
{
	public partial class LinesViewItemTemplate : ContentView
	{

		public LinesViewItemTemplate(bool emptyDate)
		{
			InitializeComponent();
			EntryAmount.IsEnabled = App.displayMode;
			DatePicker.IsEnabled = App.displayMode;

			//when a workitem comes in display mode .. if there are fields with empty values ... 
			//can you show them as emply values as opposed to the way you have currently which shows the field type like XXLarge etc.
			//2016.9.8
			if (App.displayMode == true) // visible case
			{
				EntryAmount.Placeholder = App.req_inbox_VisibleLength;
			}
			
			if (App.request_field_type.Equals("Number"))
				EntryAmount.Keyboard = Keyboard.Numeric;

			if (App.request_field_type.Equals("Date"))
			{
				DatePicker.IsVisible = true;
				EntryAmount.IsVisible = false;

				if (emptyDate)
				{
					DatePicker.IsVisible = false;
					EntryAmount.IsVisible = true;
				}
			}
			else {
				DatePicker.IsVisible = false;
				EntryAmount.IsVisible = true;
			}

			EntryAmount.FontSize = App.fontsize;
		}
	}
}