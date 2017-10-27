using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Smartdocs.iOS;

[assembly: ExportRenderer(typeof(ListView),typeof(CustomListViewRender))]
namespace Smartdocs.iOS
{
	public class CustomListViewRender : ListViewRenderer
	{

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
			base.OnElementChanged(e);
			if (this.Control == null)
				return;
			this.Control.TableFooterView = new UIView();
		}
	
	
	}

	}

