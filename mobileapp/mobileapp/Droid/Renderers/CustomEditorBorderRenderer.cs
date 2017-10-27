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
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorBorderRenderer))]
namespace Smartdocs.Droid.Renderers
{
    public class CustomEditorBorderRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(4, Android.Graphics.Color.LightGray);
                this.Control.SetBackgroundDrawable(gd);


                var view = (CustomEditor)Element;
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#4D4D4D"));
                Control.SetHintTextColor(global::Android.Graphics.Color.ParseColor("#E6E6E6"));

                //if (view.HasBorders)
                //{
                //    this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.AllBorders);
                //}
                //else
                //{
                //    this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.NoBorders);
                //}
            }

            if (e.NewElement != null)
            {
                var element = e.NewElement as CustomEditor;
                this.Control.Hint = element.Placeholder;
            }

        }



        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomEditor.PlaceholderProperty.PropertyName)
            {
                if (Element != null)
                {
                    var element = this.Element as CustomEditor;
                    this.Control.Hint = element.Placeholder;
                    Control.SetHintTextColor(global::Android.Graphics.Color.ParseColor("#787474"));
                }
            }
        }


    }
}