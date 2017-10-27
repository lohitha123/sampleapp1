using Xamarin.Forms;

namespace Smartdocs.Request.ViewModel
{
	public class MainViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Maintest"; }
		}

		public ContentView View
		{
			get { return new NewInvoiceMain (); }
		}
	}
}
