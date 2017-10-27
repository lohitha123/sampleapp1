using System;
using Xamarin.Forms;
using Smartdocs;

namespace Smartdocs.ViewModels
{
	public class LogsViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Logs"; }
		}

		public ContentView View
		{
			get { return new LogsView(); }
		}
	}
}

