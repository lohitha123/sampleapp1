using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Smartdocs.Droid;
using Smartdocs;
using System;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryDroidRenderer))]

namespace Smartdocs.Droid
{
	public class MyEntryDroidRenderer : EntryRenderer
	{
		//Android.Graphics.Color _lightGray = ((Color)App.Current.Resources["LightGray"]).ToAndroid();

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null || Element == null || e.OldElement != null) return;

			//GradientDrawable gd = new GradientDrawable();
			//gd.SetColor(_lightGray);
			//gd.SetCornerRadius(75);
			//Control.SetBackground(gd);

			var formsEntry = Element;
			var androidEntry = Control;

			if (formsEntry.IsEnabled)
			{
				androidEntry.SetTextColor(Android.Graphics.Color.Black);
			}
			else 
			{
				androidEntry.SetTextColor(Android.Graphics.Color.Gray);
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var androidEditText = sender as Entry;

			if (e.PropertyName == nameof(androidEditText.IsEnabled))
			{
				Console.WriteLine(androidEditText);
				Control.SetTextColor(Android.Graphics.Color.Gray);
			}
		}
	}
}
