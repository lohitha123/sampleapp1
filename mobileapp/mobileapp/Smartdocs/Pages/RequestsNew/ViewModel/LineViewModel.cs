using Xamarin.Forms;

namespace Smartdocs.Request.ViewModel
{
	public class LineViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Lines"; }
		}

		public ContentView View
		{
			get { return new NewInvoiceLine(); }
		}
	}
}

