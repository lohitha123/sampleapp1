using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Samples.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            var t = typeof(SignaturePad.Forms.iOS.SignaturePadRenderer);
            this.LoadApplication(new Samples.App());
            return base.FinishedLaunching(app, options);
        }
    }
}
