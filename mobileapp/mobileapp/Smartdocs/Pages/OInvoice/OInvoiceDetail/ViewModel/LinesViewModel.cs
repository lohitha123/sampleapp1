using System;
using Xamarin.Forms;
using Smartdocs;

namespace Smartdocs.Outbox.ViewModels
{
	public class LinesViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Lines"; }
		}

		public ContentView View
		{
			get { return new OutboxLineView(); }
		}

		public string no { get; set; }

		public string material { get; set; }

		public string amount { get; set; }

		public string quantity { get; set; }
	}
}

