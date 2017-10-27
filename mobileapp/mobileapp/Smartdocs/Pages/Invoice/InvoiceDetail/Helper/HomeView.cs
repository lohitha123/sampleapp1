﻿using System;
using Xamarin.Forms;

namespace Smartdocs
{
	public class HomeView : ContentView
	{
		public HomeView()
		{
			BackgroundColor = Color.White;

			var label = new Label
			{
				XAlign = TextAlignment.Center,
				TextColor = Color.Black
			};

			label.SetBinding(Label.TextProperty, "Title");
			this.SetBinding(ContentView.BackgroundColorProperty, "Background");

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					label
				}
			};
		}
	}
}


