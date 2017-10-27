using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace Smartdocs.Request
{
	public class CarouselLayout1 : ScrollView
	{
		private readonly StackLayout _stack;

		private int _selectedIndex;

		public CarouselLayout1()
		{
			Orientation = ScrollOrientation.Horizontal;

			_stack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 0
			};

			Content = _stack;
			BackgroundColor = Color.White;
		}


		public IList<View> Children
		{
			get
			{
				return _stack.Children;
			}
		}

		private bool _layingOutChildren;

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			base.LayoutChildren(x, y, width, height);
			if (_layingOutChildren) return;

			_layingOutChildren = true;
			foreach (var child in Children) child.WidthRequest = width;
			_layingOutChildren = false;
		}

		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create<CarouselLayout1, int>(
				carousel => carousel.SelectedIndex,
				0,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((CarouselLayout1)bindable).UpdateSelectedItem();
				}
			);

		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				SetValue(SelectedIndexProperty, value);
			}
		}

		private void UpdateSelectedItem()
		{
			SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
		}

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create<CarouselLayout1, IList>(
				view => view.ItemsSource,
				null,
				propertyChanging: (bindableObject, oldValue, newValue) =>
				{
					((CarouselLayout1)bindableObject).ItemsSourceChanging();
				},
				propertyChanged: (bindableObject, oldValue, newValue) =>
				{
					((CarouselLayout1)bindableObject).ItemsSourceChanged();
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

		private void ItemsSourceChanging()
		{
			if (ItemsSource == null) return;
			_selectedIndex = ItemsSource.IndexOf(SelectedItem);
		}

		private void ItemsSourceChanged()
		{
			_stack.Children.Clear();
			foreach (var item in ItemsSource)
			{
				var content = (DynamicTemplateLayout)ItemTemplate.CreateContent();
				content.BindingContext = item;
				_stack.Children.Add(content.View);
			}

			if (_selectedIndex >= 0) SelectedIndex = _selectedIndex;
		}

		public DataTemplate ItemTemplate
		{
			get;
			set;
		}

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create<CarouselLayout1, object>(
				view => view.SelectedItem,
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{
					((CarouselLayout1)bindable).UpdateSelectedIndex();
				}
			);

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

		private void UpdateSelectedIndex()
		{
			if (SelectedItem == BindingContext) return;

			SelectedIndex = Children
				.Select(c => c.BindingContext)
				.ToList()
				.IndexOf(SelectedItem);
		}
	}
}


