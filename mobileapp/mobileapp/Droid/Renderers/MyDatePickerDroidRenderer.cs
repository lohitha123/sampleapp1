using Smartdocs;
using Smartdocs.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerDroidRenderer))]

namespace Smartdocs.Droid
{
	public class MyDatePickerDroidRenderer : DatePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control == null || Element == null || e.OldElement != null) return;

			var formsDatePicker = Element;
			var androidDatePicker = Control;

			if (formsDatePicker.IsEnabled)
			{
				androidDatePicker.SetTextColor(Android.Graphics.Color.Black);
			}
			else
			{
				androidDatePicker.SetTextColor(Android.Graphics.Color.Gray);
			}
		}
	}
}
