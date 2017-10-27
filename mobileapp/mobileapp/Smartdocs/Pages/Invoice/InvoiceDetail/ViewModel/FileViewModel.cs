using System;
using Xamarin.Forms;
using Smartdocs;

namespace Smartdocs.ViewModels
{
	public class FileViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Files"; }
		}

		public ContentView View
		{
			get { return new FilesView(); }
		}
	}
}

