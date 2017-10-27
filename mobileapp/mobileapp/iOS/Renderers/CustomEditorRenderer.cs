
using AtPar.iOS.CustomRenderer;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Smartdocs.Custom;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace AtPar.iOS.CustomRenderer
{
    class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if(Element != null)
                {

                    var view = (CustomEditor)Element;
                    //Font Size
                     Element.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Editor));

                    this.Control.Layer.CornerRadius = 1f;
                    //Border color set as purple
                    this.Control.Layer.BorderColor = Color.FromHex("#6C276A").ToCGColor();
                    this.Control.Layer.BorderWidth = 1f;

                    this.Control.KeyboardType = UIKeyboardType.ASCIICapable;


                    var Placeholder = view.Placeholder;
                    if (string.IsNullOrEmpty(Element.Text))
                    {
                        Control.Text = Placeholder;
                        Control.TextColor = UIColor.LightGray;
                    }
                    else
                    {
                        Control.Text = Element.Text;
                        Control.TextColor = UIColor.FromRGB(77, 77, 77);
                    }

                    Control.ShouldBeginEditing += (UITextView textView) =>
                    {
                        if (textView.Text == Placeholder)
                        {
                            textView.Text = "";
                            textView.TextColor = UIColor.FromRGB(77, 77, 77); // Text Color
                        }

                        return true;
                    };

                    Control.ShouldEndEditing += (UITextView textView) =>
                    {
                        if (string.IsNullOrEmpty(Element.Text))
                        {
                            Control.Text = Placeholder;
                            Control.TextColor = UIColor.LightGray;
                        }
                        else
                        {
                            Control.Text = Element.Text;
                            Control.TextColor = UIColor.FromRGB(77, 77, 77);
                        }
                        return true;
                    };
                }
                
            }
        }

    }
}

