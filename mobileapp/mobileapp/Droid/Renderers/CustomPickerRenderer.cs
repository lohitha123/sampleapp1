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
using Smartdocs.Custom;
using Smartdocs.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Smartdocs.Droid.Renderers
{

    public class CustomPickerRenderer : PickerRenderer
    {
        Spinner mySpinner;
        // Override the OnModelChanged method so we can tweak this renderer post-initial setup
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(4, Android.Graphics.Color.Black);
                gd.SetCornerRadius(20);
                this.Control.SetBackgroundDrawable(gd);
                //// lets get a reference to the native control
                //var nativeEditText = (TextView)Control;
                //// do whatever you want to the EditText here!
                //mySpinner = new Spinner(this.Context);
                //nativeEditText.SetBackgroundColor(global::Android.Graphics.Color.Yellow);
                //mySpinner.SetBackgroundResource(CustomRenderer.Android.Resource.Drawable.spinner_border);
                //this.SetNativeControl(mySpinner);
            }
        }
    }
}