using System;
using Xamarin.Forms;
using System.Net;
using FormsBackgrounding.Messages;
using Acr.UserDialogs;
using Plugin.Connectivity;
using System.Net.Http;
using Smartdocs.SQLite;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smartdocs
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //loginButton.Image = (Xamarin.Forms.FileImageSource)ImageSource.FromFile("login_button.png");

            //companyid.Completed += (s, e) =>
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Task.Run(() => Task.Delay(400));
            //        username.Focus();
            //    });
            //};

            //username.Completed += (s, e) =>
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Task.Run(() => Task.Delay(400));
            //        password.Focus();
            //    });
            //};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Content.FindByName<Button>("loginButton").Clicked += OnLoginButtonClicked;

            indicator.VerticalOptions = LayoutOptions.CenterAndExpand;
            indicator.IsRunning = false;
            indicator.Color = Device.OnPlatform(Color.Black, Color.Default, Color.Default);

            // Make Navigation Bar Title Invisible
            NavigationPage.SetHasNavigationBar(this, false);

            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey("LoggedIn") && properties["LoggedIn"].Equals("true"))
            {
                Navigation.PushAsync(new RootPage());
            }
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {

            if (CheckValidate())
            {

                //string companyUrl = "";
                HttpResponseMessage result = null;

                if (CrossConnectivity.Current.IsConnected)
                {

                    UserDialogs.Instance.ShowLoading("Logging in...");

                    var comUrlList = new List<string>();
                    comUrlList = await App.G_HTTP_CLIENT.GetCompanyUrlAsync(companyid.Text.Trim());
                    //comUrlList.Add("http://182.156.74.204:8080/rest/authentication/getToken");
                    //comUrlList.Add("http://182.156.74.204:8080/rest/authentication/getToken");

                    for (int i = 0; i < 2; i++)
                    {
                        int length = comUrlList[i].Length;
                        Constants.SERVER = comUrlList[i].Substring(0, length - 28);

                        result = await App.G_HTTP_CLIENT.LoginAsync(username.Text.Trim(), password.Text.Trim());

                        //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        //{
                        if (result == null)
                        {
                            //=== for preventing issue in ipv6
                            //if (i == 0)
                            //{
                            //	GetUserInfoSQLite();
                            //	break;
                            //}
                            //==== end
                            if (i == 1)
                            {
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Error", "Can't connect server.", "Ok");
                            }
                        }
                        else
                        {
                            if (result.StatusCode == HttpStatusCode.OK)
                            {



                                UserDialogs.Instance.HideLoading();

                                if (i == 0)
                                    Application.Current.Properties["secondUrl"] = comUrlList[1];
                                else if (i == 1)
                                    Application.Current.Properties["secondUrl"] = comUrlList[0];

                                var token = result.Content.ReadAsStringAsync().Result;
                                Constants.SECRET_TOKEN = token;

                                SetUserInfoSQLite();
                                GotoHomePage(token);

                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Warning", "Invalid credentials", "Ok");
                            }

                            break;
                        }
                        //});
                    }

                }
                else
                {//offline mode
                    GetUserInfoSQLite();
                }

            }
            else
            {
                await DisplayAlert("Warning!", "Username and password is required!", "Ok");
            }
        }

        private bool CheckValidate()
        {

            if (String.IsNullOrEmpty(username.Text) || String.IsNullOrEmpty(password.Text) || String.IsNullOrEmpty(companyid.Text))
                return false;
            else
                return true;
        }

        void GotoHomePage(string token)
        {
            Application.Current.Properties["LoggedIn"] = "true";
            Application.Current.Properties["token"] = token;
            Application.Current.Properties["userId"] = username.Text.Trim();
            Application.Current.Properties["password"] = password.Text.Trim();
            Application.Current.Properties["companyId"] = companyid.Text.Trim();
            Application.Current.Properties["companyUrl"] = Constants.SERVER;

            //Navigation.InsertPageBefore (new RootPage (), this);
            Navigation.PushAsync(new RootPage());
            //await Navigation.PopAsync();
        }

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //ticker.Text = message.Message;
                });
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //ticker.Text = "Cancelled";
                });
            });
        }

        async void SetUserInfoSQLite()
        {
            var dbInit = new DataAccessLayer(null);
            await dbInit.InitUserInfoDB();

            var userInfo = new UserInfo();
            userInfo.userID = username.Text.Trim();
            userInfo.password = password.Text.Trim();
            userInfo.portalURL = Constants.SERVER;
            userInfo.companyID = companyid.Text.Trim();
            userInfo.token = Constants.SECRET_TOKEN;
            await userInfo.Save();
        }

        async void GetUserInfoSQLite()
        {
            //Constants.ShowToast("You are offline");

            var dbInit = new DataAccessLayer(null);
            List<UserInfo> userinfo = await dbInit.PullUserInfo(username.Text.Trim(), password.Text.Trim());
            Debug.WriteLine(userinfo);

            if (userinfo.Count == 0)
                await DisplayAlert("Warning", "You are online now, but Invalid credentials", "Ok");
            else
            {
                Constants.SERVER = userinfo[0].portalURL;
                Constants.SECRET_TOKEN = userinfo[0].token;
                GotoHomePage(userinfo[0].token);
            }
        }
    }
}

