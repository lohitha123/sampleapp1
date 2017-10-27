using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;
using FormsBackgrounding.Messages;
using FormsBackgrounding.Droid;

namespace Smartdocs.Droid
{
	[Activity (Label = "Smartdocs.Droid", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			UserDialogs.Init(this);
			LoadApplication (new App ());

			WireUpLongRunningTask();
		}

		void WireUpLongRunningTask()
		{
			MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message =>
			{
				var intent = new Intent(this, typeof(LongRunningTaskService));
				StartService(intent);
			});

			MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message =>
			{
				var intent = new Intent(this, typeof(LongRunningTaskService));
				StopService(intent);
			});
		}
	}
}

