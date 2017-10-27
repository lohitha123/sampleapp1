using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Smartdocs.Outbox
{
	public class PagerIndicatorTabs : Grid
	{
		public Color DotColor { get; set; }

		public double DotSize { get; set; }

		private List<MenuItemMoel> subMenus;

		public PagerIndicatorTabs()
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand;
			VerticalOptions = LayoutOptions.Center;
			DotColor = Color.Black;
			Device.OnPlatform(iOS: () => BackgroundColor = Color.Gray);
			RowSpacing = ColumnSpacing = 0;

			var assembly = typeof(PagerIndicatorTabs).GetTypeInfo().Assembly;
			foreach (var res in assembly.GetManifestResourceNames())
				System.Diagnostics.Debug.WriteLine("found resource: " + res);

			BackgroundColor = Color.White;

			subMenus = new List<MenuItemMoel> {
				new MenuItemMoel {
					MenuIcon = "main.png",
					MenuTitle = "MAIN"
				},
				new MenuItemMoel {
					MenuIcon = "lines.png",
					MenuTitle = "LINES"
				},
				new MenuItemMoel {
					MenuIcon = "files.png",
					MenuTitle = "FILES"
				},
				new MenuItemMoel {
					MenuIcon = "logs.png",
					MenuTitle = "TEST"
				}
			};
		}

		private void CreateTabs()
		{
			if (Children != null && Children.Count > 0) Children.Clear();
			int i = 0;

			foreach (var item in ItemsSource)
			{
				var index = Children.Count;
				var tab = new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(7)
				};

				Device.OnPlatform(
					iOS: () =>
					{
						tab.Children.Add(new Image { 
							Source = subMenus[i].MenuIcon, 
							HeightRequest = 20,
							VerticalOptions = LayoutOptions.End
						});
						tab.Children.Add(new Label 
							{ 
								Text = subMenus[i].MenuTitle,
								FontSize = 13, 
								HorizontalOptions = LayoutOptions.End,
								VerticalOptions = LayoutOptions.End,
								XAlign = TextAlignment.End
							});
					},
					Android: () =>
					{
						tab.Children.Add(new Image { Source = subMenus[index].MenuIcon, HeightRequest = 25 });
					}
				);
				var tgr = new TapGestureRecognizer();
				tgr.Command = new Command(() =>
					{
						SelectedItem = ItemsSource[index];
					});
				tab.GestureRecognizers.Add(tgr);
				Children.Add(tab, index, 0);
				i++;
			}
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<PagerIndicatorTabs, IList>(
				pi => pi.ItemsSource,
				null,
				BindingMode.OneWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((PagerIndicatorTabs)bindable).ItemsSourceChanged();
				}
			);

		public IList ItemsSource
		{
			get
			{
				return (IList)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<PagerIndicatorTabs, object>(
				pi => pi.SelectedItem,
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((PagerIndicatorTabs)bindable).SelectedItemChanged();
				});

		public object SelectedItem
		{
			get
			{
				return GetValue(SelectedItemProperty);
			}
			set
			{
				SetValue(SelectedItemProperty, value);
			}
		}

		private void ItemsSourceChanged()
		{
			if (ItemsSource == null) return;

			CreateTabs();
		}

		private void SelectedItemChanged()
		{
			var selectedIndex = ItemsSource.IndexOf(SelectedItem);
			var pagerIndicators = Children.Cast<StackLayout>().ToList();

			foreach (var pi in pagerIndicators)
			{
				UnselectTab(pi);
			}

			if (selectedIndex > -1)
			{
				SelectTab(pagerIndicators[selectedIndex]);
			}
		}

		private static void UnselectTab(StackLayout tab)
		{
			tab.Opacity = 0.5;
			((Label)tab.Children [1]).TextColor = Color.FromHex ("#000000");
			var obj = ((Image)tab.Children [0]).Source;
			var text = ((FileImageSource)obj).File;

			if (text.Contains("act_")) {
				((Image)tab.Children [0]).Source = text.Substring(4);
			}
		}

		private static void SelectTab(StackLayout tab)
		{
			tab.Opacity = 1.0;
			((Label)tab.Children [1]).TextColor = Color.FromHex ("#0d73a2");

			var obj = ((Image)tab.Children [0]).Source;
			var text = ((FileImageSource)obj).File;
			((Image)tab.Children [0]).Source = "act_" + text;
		}
	}

	public class MenuItemMoel {
		public string MenuIcon { get; set;}
		public string MenuTitle { get; set; }
	}
}

