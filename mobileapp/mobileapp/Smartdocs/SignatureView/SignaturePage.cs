using System;
using Smartdocs;
using Xamarin.Forms;

namespace Signature
{
	public class SignaturePage  : ContentPage
	{
		ImageWithTouch DrawingImage;

		public SignaturePage()
		{
			DrawingImage = BuildDrawingFrame();
			StackLayout stack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = 0,
				Spacing = 0
			};

			var backButton = new Button
			{
				Image = "back.png",
				BackgroundColor = Color.FromHex("#00FFFFFF"),
				WidthRequest = 35,
				HeightRequest = 30,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.EndAndExpand
			};
			backButton.Clicked += ClickBack;

			StackLayout backBtnLayout = new StackLayout
			{
				Children = { backButton },
				Padding = new Thickness(0)
			};

			Grid gridDrawing = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition {
						Height = new GridLength (5, GridUnitType.Star)
					},
					new RowDefinition {
						Height = new GridLength (6, GridUnitType.Star)
					},
					new RowDefinition {
						Height = new GridLength (25, GridUnitType.Star)
					},
					new RowDefinition {
						Height = new GridLength (1.5, GridUnitType.Star)
					}
				},
				ColumnDefinitions = {
					new ColumnDefinition {
						Width = new GridLength (0.07, GridUnitType.Star)
					},
					new ColumnDefinition {
						Width = new GridLength (1, GridUnitType.Star)
					},
					new ColumnDefinition {
						Width = new GridLength (0.07, GridUnitType.Star)
					},
				},
				Children =
				{
					{new ContentView {
							Content = backBtnLayout,
							Padding = 0,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.EndAndExpand,
							//BackgroundColor = Color.FromHex("#FF0000")
						}, 1, 0},
					{new ContentView {
							Padding = 0,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.CenterAndExpand
						}, 1, 1},
					{new ContentView {
							Content = BuildDrawingFrame(),
							Padding = 0,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,
						}, 1, 2},
					{new ContentView {
							HorizontalOptions = LayoutOptions.Start,
							VerticalOptions = LayoutOptions.Start,
							Padding = new Thickness(10, 35, 0, 0)
						}, 1, 2},
					{new ContentView {
							HorizontalOptions = LayoutOptions.End,
							VerticalOptions = LayoutOptions.Start,
							Padding = new Thickness(0, 35, 10, 0)
						}, 1, 2},
				}
			};

			Image imgPhoto = new Image 
			{
				Source = "bg_signature_portrait.png", 
				Aspect = Aspect.Fill,
			};

			var clearButton = new Button
			{
				Text = "Clear",
				HorizontalOptions = LayoutOptions.Center
			};
			clearButton.Clicked += ClickClearSign;

			var saveButton = new Button
			{
				Text = "Save",
				HorizontalOptions = LayoutOptions.Center
			};
			saveButton.Clicked += ClickSave;

			var absoluteLayout = new AbsoluteLayout();

			absoluteLayout.Children.Add(imgPhoto, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			absoluteLayout.Children.Add(gridDrawing, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			absoluteLayout.VerticalOptions = LayoutOptions.FillAndExpand;
			absoluteLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

			StackLayout mainStack = new StackLayout { 
				Children = { absoluteLayout }, 
				Orientation = StackOrientation.Vertical, 
				VerticalOptions = LayoutOptions.FillAndExpand 
			};

			stack.Children.Add(mainStack);
			stack.Children.Add(saveButton);
			stack.Children.Add(clearButton);

			Content = stack;

			DrawingImage.CurrentLineWidth = 2;
			DrawingImage.CurrentLineColor = Color.Black;

		}

		ImageWithTouch BuildDrawingFrame()
		{
			DrawingImage = new ImageWithTouch
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
				CurrentLineColor = Color.Black,
			};

			return DrawingImage;
		}

		void ClickClearSign(Object sender, EventArgs e)
		{
			DrawingImage.ClearPath = !DrawingImage.ClearPath;
		}

		void ClickBack(Object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		void ClickSave(Object sender, EventArgs e)
		{
			string savedFileName = App.app_path + "/temp_" + DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss") + ".jpg";
			//If this property is set the Image is stored in the folder path.
			DrawingImage.SavedImagePath = savedFileName;
			App.sign_img_path = savedFileName;

			Navigation.PopModalAsync();
		}
	}

}

