using Xamarin.Forms;

namespace Smartdocs.Request.ViewModel
{
	public class CommentViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Comment"; }
		}

		public ContentView View
		{
			get { return new NewInvoiceComment(); }
		}
	}
}


