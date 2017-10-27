using System;

using Xamarin.Forms;

namespace Smartdocs.ViewModels
{
	public class SignatureModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Signature Page"; }
		}

		public ContentView View
		{
			get { return new SignatureView (); }
		}
	}
}


