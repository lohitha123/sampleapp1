using Smartdocs.ViewModels;
using Xamarin.Forms;


namespace Smartdocs
{
	public class DynamicTemplateLayout : ViewCell
	{
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var vm = BindingContext as ICarouselViewModel;
			var page = vm.View;
			page.BindingContext = vm;
			View = page;
		}
	}
}

