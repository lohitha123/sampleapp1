using System;

using Xamarin.Forms;

namespace Smartdocs
{
	public class SmartTaskList : ContentPage
	{
		public SmartTaskList ()
		{
			ScrollView TaskListScroll = new ScrollView ();

			Content = TaskListScroll;

			StackLayout RowStackLayout1 = new StackLayout {
				HeightRequest = Device.OnPlatform(100, 100, 100),
				Padding = new Thickness(5, 5, 5, 5),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center
			};

			StackLayout RowStackLayout2 = new StackLayout {
				HeightRequest = Device.OnPlatform(100, 100, 100),
				Padding = new Thickness(5, 5, 5, 5),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center
			};

			StackLayout ColStackLayout1 = new StackLayout {
				Padding = new Thickness(0, 0, 20, 0),
			};

			StackLayout ColStackLayout2 = new StackLayout {
				Padding = new Thickness(20, 0, 0, 0),
			};

			StackLayout ColStackLayout3 = new StackLayout {
				Padding = new Thickness(0, 0, 20, 0),
			};

			StackLayout ColStackLayout4 = new StackLayout {
				Padding = new Thickness(20, 0, 0, 0),
			};

			RowStackLayout1.Children.Add (ColStackLayout1);
			RowStackLayout1.Children.Add (ColStackLayout2);
			RowStackLayout2.Children.Add (ColStackLayout3);
			RowStackLayout2.Children.Add (ColStackLayout4);

			Button PoButton = new Button {
				Text = "PO",
				HeightRequest = 100,
				WidthRequest = 120,
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.Start,
				BorderWidth = 2,
				BorderColor = Color.Red,
			};

			PoButton.Clicked += async (sender, e) => {
				await Navigation.PushAsync(new WorkItemListPage());
			};

			Button PRButton = new Button {
				Text = "PR",
				HeightRequest = 100,
				WidthRequest = 120,
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.End,
				BorderWidth = 2,
				BorderColor = Color.Red
			};

			PRButton.Clicked += async (sender, e) => {
				await Navigation.PushAsync(new WorkItemListPage());
			};

			Button VenderButton = new Button {
				Text = "VENDER",
				HeightRequest = 100,
				WidthRequest = 120,
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.Start,
				BorderWidth = 2,
				BorderColor = Color.Red
			};

			VenderButton.Clicked += async (sender, e) => {
				await Navigation.PushAsync(new WorkItemListPage());
			};

			Button SpaceButton = new Button {
				Text = "",
				HeightRequest = 100,
				WidthRequest = 120,
				BackgroundColor = Color.White,
				HorizontalOptions = LayoutOptions.Start,
				BorderWidth = 2,
				BorderColor = Color.Red
			};

			SpaceButton.Clicked += async (sender, e) => {
				await Navigation.PushAsync(new WorkItemListPage());
			};
			ColStackLayout1.Children.Add (PoButton);
			ColStackLayout2.Children.Add (PRButton);
			ColStackLayout3.Children.Add (VenderButton);
			ColStackLayout4.Children.Add (SpaceButton);

			StackLayout mainLayout = new StackLayout {
				BackgroundColor = Color.Green,
				Padding = new Thickness(5, 5, 5, 5)
			};

			TaskListScroll.Content = mainLayout;
			mainLayout.Children.Add (RowStackLayout1);
			mainLayout.Children.Add (RowStackLayout2);
		}
	}
}