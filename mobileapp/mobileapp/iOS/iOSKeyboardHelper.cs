
using Smartdocs.iOS;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
[assembly: Dependency(typeof(iOSKeyboardHelper))]
namespace Smartdocs.iOS
{
    class iOSKeyboardHelper:IKeyboardHelper
    {

        public void DeviceHideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}
