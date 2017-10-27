using System;
using Xamarin.Forms;
using Smartdocs;

namespace Smartdocs.Outbox.ViewModels
{
	public class FileViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Files"; }
		}

		public ContentView View
		{
			get { return new OutboxFileView(); }
		}
	}
}

