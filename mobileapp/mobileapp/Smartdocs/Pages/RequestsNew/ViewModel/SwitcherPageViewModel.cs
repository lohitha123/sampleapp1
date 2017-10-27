using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Smartdocs.Request.ViewModel
{
	public class SwitcherPageViewModel : BaseViewModel
	{
		private IEnumerable<HomeViewModel> _pages;

		public IEnumerable<HomeViewModel> Pages
		{
			get
			{
				return _pages;
			}
			set
			{
				SetObservableProperty(ref _pages, value);
				CurrentPage = Pages.FirstOrDefault();
			}
		}

		private HomeViewModel _currentPage;

		public HomeViewModel CurrentPage
		{
			get
			{
				return _currentPage;
			}
			set
			{
				SetObservableProperty(ref _currentPage, value);
			}
		}
	}

	public class HomeViewModel : BaseViewModel
	{
		public HomeViewModel()
		{
		}

		public string Title { get; set; }

		public Color Background { get; set; }

		public ImageSource ImageSource { get; set; }
	}

}


