using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartdocs
{
	public class LogsPage : ContentPage
	{
		public LogsPage ()
		{
			Label header = new Label {
				Text = "Work Item List"
			};

			List<LogModel> M_Logs = new List<LogModel> {
				new LogModel(),
				new LogModel(),
			};

			ListView listView = new ListView
			{
				ItemsSource = M_Logs,
				ItemTemplate = new DataTemplate(() => {
					Label label = new Label();
					label.SetBinding(Label.TextProperty, "WorkItem");
					Label fieldNameLabel = new Label();
					fieldNameLabel.SetBinding(Label.TextProperty, "Activity");

					return new ViewCell {
						View = new StackLayout
						{
							Padding = new Thickness(5, 5, 5, 5),
							Orientation = StackOrientation.Horizontal,
							HorizontalOptions = LayoutOptions.Center,
							Children = {
								new StackLayout {
									VerticalOptions = LayoutOptions.Center,
									Spacing = 0,
									Children =
									{
										label, fieldNameLabel
									}
								}
							}
						}
					};
				}),
			};

			Content = new StackLayout { 
				Children = {
					header, listView
				}
			};
		}
	}
}


