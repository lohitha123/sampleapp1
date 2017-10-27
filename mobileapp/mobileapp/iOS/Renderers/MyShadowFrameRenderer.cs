using Smartdocs;
using Smartdocs.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(MyShadowFrame), typeof(MyShadowFrameRenderer))]
namespace Smartdocs.iOS
{
	public class MyShadowFrameRenderer : FrameRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);
			Layer.ShadowOpacity = 0.2f;
		}
	}
}
