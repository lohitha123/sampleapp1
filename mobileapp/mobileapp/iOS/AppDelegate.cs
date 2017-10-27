using System;
using System.Collections.Generic;
using System.Linq;
using FormsBackgrounding.iOS;
using FormsBackgrounding.Messages;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Smartdocs.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{

		#region Methods
		iOSLongRunningTaskExample longRunningTaskExample;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App ());

			WireUpLongRunningTask();

			var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
			UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);

			return base.FinishedLaunching (app, options);
		}

		void WireUpLongRunningTask()
		{
			MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", async message =>
			{
				longRunningTaskExample = new iOSLongRunningTaskExample();
				await longRunningTaskExample.Start();
			});

			MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message =>
			{
				longRunningTaskExample.Stop();
			});
		}
		#endregion
	}
}

