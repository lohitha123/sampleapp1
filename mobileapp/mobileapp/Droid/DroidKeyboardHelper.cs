using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Android.Views.InputMethods;
using Smartdocs.Droid;

[assembly: Dependency(typeof(DroidKeyboardHelper))]
namespace Smartdocs.Droid
{
    class DroidKeyboardHelper : IKeyboardHelper
    {
        public void DeviceHideKeyboard()
        {
            var context = Forms.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                if (activity.CurrentFocus != null)
                {
                    var token = activity.CurrentFocus.WindowToken;
                    inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
                    activity.Window.DecorView.ClearFocus();
                }
            }
        }
    }
}