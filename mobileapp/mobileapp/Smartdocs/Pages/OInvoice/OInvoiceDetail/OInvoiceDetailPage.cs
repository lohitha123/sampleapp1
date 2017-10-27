using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using Smartdocs.Outbox.ViewModels;
using Smartdocs.Custom;//called CarouseLayout.cs of smartdocs.custom to fix bug in outbox detail

namespace Smartdocs
{
	public class OInvoiceDetailPage : ContentPage
	{
		private View _tabs;

		private RelativeLayout _relativeLayout;

		private TabbedPageViewModel _viewModel;

		public OInvoiceDetailPage ()
		{
			_viewModel = new TabbedPageViewModel();
			BindingContext = _viewModel;
			BackgroundColor = Color.White;

			Title = "Title";

			_relativeLayout = new RelativeLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var pagesCarousel = CreatePagesCarousel();
			_tabs = CreateTabs();
			var tabsHeight = 36;

			_relativeLayout.Children.Add (_tabs, 
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent (parent => parent.Width),
				Constraint.Constant (tabsHeight)
			);

			_relativeLayout.Children.Add (pagesCarousel,
				Constraint.RelativeToParent ((parent) => { return parent.X; }),
				Constraint.RelativeToParent ((parent) => { return tabsHeight; }),
				Constraint.RelativeToParent ((parent) => { return parent.Width; }),
				Constraint.RelativeToView (_tabs, (parent, sibling) => { return parent.Height - (sibling.Height); })
			);

			StackLayout stackLayout = new StackLayout {
				BackgroundColor = Color.White
			};
			StackLayout menu = new StackLayout
			{
				HeightRequest = Device.OnPlatform(56, 46, 46),
				BackgroundColor = Color.White,
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.Center
			};

			var backButtonTap = new TapGestureRecognizer ();
			backButtonTap.Tapped += OnBackButtonClicked;


			Image backButton = new Image {
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = 24,
				HeightRequest = 24,
				Source = "back.png",
			};

			backButton.GestureRecognizers.Add (backButtonTap);

				StackLayout menuBarCenter = new StackLayout {
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.Center
				};

				Label MenuBarTitle = new Label {
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.Center,
					FontSize = 18,
					Text = App.G_DocType + " " + App.G_DocId,
					TextColor = Color.Black
				};

				menuBarCenter.Children.Add (MenuBarTitle);

			menu.Children.Add (backButton);
			menu.Children.Add (menuBarCenter);
			menu.Padding = new Thickness (10, 38, 10, 10);

			stackLayout.Children.Add (menu);
			_relativeLayout.BackgroundColor = Color.White;
			stackLayout.Children.Add (_relativeLayout);
			Content = stackLayout;
			NavigationPage.SetHasNavigationBar(this, false);
		}

		async void OnBackButtonClicked(Object sender, EventArgs e) {
			await Navigation.PopAsync ();
		}

		CarouselLayout CreatePagesCarousel ()
		{
			var carousel = new CarouselLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(typeof(DynamicTemplateLayout))
			};
			carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Pages");
			carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

			return carousel;
		}

		private View CreateTabsContainer()
		{
			return new StackLayout
			{
				Children = { CreateTabs() }
			};
		}

		private View CreateTabs()
		{
			var pagerIndicator = new PagerIndicatorTabs() { HorizontalOptions = LayoutOptions.CenterAndExpand };
			pagerIndicator.RowDefinitions.Add(new RowDefinition() { Height = 36 });
			//pagerIndicator.SetBinding(PagerIndicatorTabs.ColumnDefinitionsProperty, "Pages", BindingMode.Default, new SpacingConverter());
			pagerIndicator.SetBinding(PagerIndicatorTabs.ItemsSourceProperty, "Pages");
			pagerIndicator.SetBinding(PagerIndicatorTabs.SelectedItemProperty, "CurrentPage");

			return pagerIndicator;
		}
	}

}