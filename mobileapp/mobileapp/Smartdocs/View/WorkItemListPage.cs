using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartdocs
{
	public class WorkItemListPage : ContentPage
	{
		public WorkItemListPage ()
		{
			Label header = new Label {
				Text = "Work Item List"
			};

			List<WorkFlowFieldModel> M_WorkItems = new List<WorkFlowFieldModel> {
				new WorkFlowFieldModel(),
				new WorkFlowFieldModel(),
				new WorkFlowFieldModel(),
				new WorkFlowFieldModel(),
				new WorkFlowFieldModel(),
				new WorkFlowFieldModel(),
				new WorkFlowFieldModel()
			};

			ListView listView = new ListView
	 		{
				ItemsSource = M_WorkItems,
				ItemTemplate = new DataTemplate(() => {
					Label label = new Label();
					label.SetBinding(Label.TextProperty, "FieldId");
					Label fieldNameLabel = new Label();
					fieldNameLabel.SetBinding(Label.TextProperty, "FieldName");
					return new ViewCell  {
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

			listView.ItemSelected += (sender, e) => {
//				Navigation.PushAsync(new DetailPage());
			};
		}
	}
}