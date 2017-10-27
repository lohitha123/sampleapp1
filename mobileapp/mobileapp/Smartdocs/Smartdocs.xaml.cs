using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Smartdocs.Models;

namespace Smartdocs
{
    public partial class App : Application
    {
        public static App G_App { get; set; }
        public static HttpHandler G_HTTP_CLIENT { get; set; }
        public static bool G_IS_LOGGEDIN { get; set; }
        public static Page G_ROOT_PAGE { get; set; }
        public static WorkItem G_CURRENT_ACTIVE_ITEM { get; set; }
        public static WorkItem G_CURRENT_COM_ACTIVE_ITEM { get; set; }

        public static string G_PageTitle { get; set; }
        public static string G_DocType { get; set; }
        public static string G_DocId { get; set; }

        public static bool doctypeloaded { get; set; }
        public static bool workitemloaded { get; set; }
        public static bool completedworkitemloaded { get; set; }

        public static bool displayMode { get; set; }
        public static string approveComment { get; set; }

        //for Request
        public static List<LineItem> requestMainItem { get; set; }
        public static string requestComment { get; set; }
        public static bool barcodeField { get; set; }
        public static string request_field_type { get; set; }
        public static string req_inbox_VisibleLength { get; set; }
        public static int fontsize { get; set; }
        //end

        public static string sign_img_path { get; set; }
        public static string app_path { get; set; }
        public static byte[] imgByteData { get; set; }
        public static string fileName { get; set; }
        public static string fileExt { get; set; }

        public static bool navigation_flag { get; set; }

        public static List<WorkItem> G_WORK_ITEMS
        {
            get;
            set;
        }

        public static List<WorkItem> G_COMPLETE_WORK_ITEMS
        {
            get;
            set;
        }

        public static List<DocType> G_DOC_ITEMS
        {
            get;
            set;
        }

        public static List<string> G_ARRAY_DOCTYPE//no use
        {
            get;
            set;
        }

        public static List<string> G_ARRAY_DOCTYPE_ForRequest//no use
        {
            get;
            set;
        }

        public static List<UserDetails> G_UserDetails
        {
            get;
            set;
        }

        public App()
        {
            InitializeComponent();

            G_ARRAY_DOCTYPE = new List<string>();
            G_ARRAY_DOCTYPE_ForRequest = new List<string>();
            G_WORK_ITEMS = new List<WorkItem>();
            G_COMPLETE_WORK_ITEMS = new List<WorkItem>();
            G_CURRENT_ACTIVE_ITEM = new WorkItem();
            G_CURRENT_COM_ACTIVE_ITEM = new WorkItem();
            requestMainItem = new List<LineItem>();
            G_UserDetails = new List<UserDetails>();
            G_DocType = "";
            sign_img_path = "";

            IDictionary<string, object> properties = Application.Current.Properties;

            if (properties.ContainsKey("LoggedIn") && properties["LoggedIn"].Equals("true"))
            {
                Constants.SECRET_TOKEN = properties["token"].ToString();
                Constants.SERVER = properties["companyUrl"].ToString();
                //MainPage = new NavigationPage (new RootPage ());//it is crashed in android
                MainPage = new NavigationPage(new Login());
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }

            G_App = this;
            G_HTTP_CLIENT = new HttpHandler();
            G_IS_LOGGEDIN = false;
        }
    }
}