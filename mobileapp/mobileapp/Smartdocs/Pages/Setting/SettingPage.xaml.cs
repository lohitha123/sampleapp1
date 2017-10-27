using System;
using System.Collections.Generic;
using FormsBackgrounding.Messages;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class SettingPage : ContentPage
	{
		public SettingPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			IDictionary<string, object> properties = Application.Current.Properties;
			userId.Text = properties["userId"].ToString();
			companyId.Text = properties["companyId"].ToString();
			portalUrl.Text = properties["companyUrl"].ToString();
			secondUrl.Text = properties["secondUrl"].ToString();

			PollingFRQ.SelectedIndexChanged += (sender, e) =>
			{
				properties["pollingFRQ"] = PollingFRQ.SelectedIndex;
			};

			DateFormat.SelectedIndexChanged += (sender, e) =>
			{
				properties["dateFormat"] = DateFormat.SelectedIndex;
			};

			NumberFormat.SelectedIndexChanged += (sender, e) =>
			{
				properties["numberFormat"] = NumberFormat.SelectedIndex;
			};

			ResetButton.Clicked += ResetButtonClicked;
			LogoutButton.Clicked += LogoutButtonClicked;
		}

		protected override void OnAppearing()
		{
			IDictionary<string, object> properties = Application.Current.Properties;

			if (properties.ContainsKey("pollingFRQ"))
				PollingFRQ.SelectedIndex = Convert.ToInt32(properties["pollingFRQ"]);
			else
				PollingFRQ.SelectedIndex = 0;

			if (properties.ContainsKey("dateFormat"))
				DateFormat.SelectedIndex = Convert.ToInt32(properties["dateFormat"]);
			else
				DateFormat.SelectedIndex = 0;

			if (properties.ContainsKey("numberFormat"))
				NumberFormat.SelectedIndex = Convert.ToInt32(properties["numberFormat"]);
			else
				NumberFormat.SelectedIndex = 0;

			if (properties.ContainsKey("language"))
				Language.SelectedIndex = Convert.ToInt32(properties["language"]);
			else
				Language.SelectedIndex = 0;
		}

		async void LogoutButtonClicked(object sender, EventArgs args)
		{
			Application.Current.Properties["LoggedIn"] = "false";
			Page page = ((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage;
			if (Device.OS == TargetPlatform.iOS)
			{
				page.Navigation.InsertPageBefore(new Login(), page);
				await page.Navigation.PopAsync();
			}
			else if (Device.OS == TargetPlatform.Android)
				await Navigation.PushAsync(new Login());
		}

		async void ResetButtonClicked(object sender, EventArgs args)
		{
			Application.Current.Properties["LoggedIn"] = "false";
			Application.Current.Properties["pollingFRQ"] = 0;
			Application.Current.Properties["dateFormat"] = 0;
			Application.Current.Properties["numberFormat"] = 0;

			App.doctypeloaded = false;
			App.workitemloaded = false;
			App.completedworkitemloaded = false;

			var stop_message = new StopLongRunningTaskMessage();
			MessagingCenter.Send(stop_message, "StopLongRunningTaskMessage");

			Page page = ((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage;

			if (Device.OS == TargetPlatform.iOS)
			{
				//page.Navigation.InsertPageBefore(new Login(), page);
				await page.Navigation.PopAsync();
			}
			else if (Device.OS == TargetPlatform.Android)
                await page.Navigation.PopAsync();
        }

	}
}

