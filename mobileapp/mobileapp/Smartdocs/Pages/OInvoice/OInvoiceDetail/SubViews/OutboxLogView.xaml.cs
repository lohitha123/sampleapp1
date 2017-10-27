using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Smartdocs.Models;
using Acr.UserDialogs;
using System.Diagnostics;

namespace Smartdocs
{
	public partial class OutboxLogView : ContentView
	{
		public OutboxLogView()
		{
			InitializeComponent();

			var logsModels = new List<Log>();
			logsModels = App.G_CURRENT_COM_ACTIVE_ITEM.logs;
			PopulateList(logsModels);
		}

		void PopulateList(List<Log> list)
		{
			var column = LogRow;
			column.Children.Clear();
			View item;

			for (var i = 0; i < list.Count; i++)
			{
				var logItemTapped = new TapGestureRecognizer();
				if (i % 2 == 0)
				{
					item = new LogsViewItemTemplate();
					logItemTapped.Tapped += OnLeftItemTapped;
				}
				else {
					item = new LogsViewItemTemplate();
					logItemTapped.Tapped += OnRightItemTapped;
				}

				list[i].User = list[i].Activity + " by " + list[i].User;
				list[i].Date = Constants.getDateFromFormat(list[i].Date);
				item.BindingContext = list[i];
				item.GestureRecognizers.Add(logItemTapped);
				column.Children.Add(item);
			}

		}

		private void OnLeftItemTapped(Object sender, EventArgs e)
		{
			var selectedItem = (Log)((LogsViewItemTemplate)sender).BindingContext;
			try
			{
				UserDialogs.Instance.AlertAsync("", selectedItem.Comments, "OK", null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Test", ex.ToString());
			}

		}

		private void OnRightItemTapped(Object sender, EventArgs e)
		{
			var selectedItem = (Log)((LogsViewItemTemplate)sender).BindingContext;
			try
			{
				UserDialogs.Instance.AlertAsync("", selectedItem.Comments, "OK", null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Test", ex.ToString());
			}

		}
	}
}

