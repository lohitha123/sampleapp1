
using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace Smartdocs
{
    public class Constants
    {

        public static string SERVER = "";
        //public static string SERVER = "http://182.156.74.204:8080/";
        public static string GETCOMPANY_API = "http://smartdocs-mobile-login.appspot.com/rest/api/getCompanyURL/";
        public static string LOGIN_API = "rest/authentication/getToken";
        public static string GETDOCTYPE_API = "rest/api/getdoctype/";
        public static string GETWORKITEM_API = "rest/api/getworkitemdatalist/";
        public static string GETCOMPLETRWORKITEM_API = "rest/api/getcompletedworkitems/";
        public static string SubmitRequest_API = "rest/api/submitrequest";
        public static string SubmitWorkitem_API = "rest/api/submitworkitem";
        public static string SaveAttachment_API = "rest/api/saveattachment";
        public static string GETUSERS_API = "rest/api/getUsers";
        public static string getStepDataUsers = "rest/api/getStepDataUsers/";
        public static string SECRET_TOKEN { get; set; }

        public static string NEW = "New";
        public static string COMPLETED = "Completed";
        public static string PENDING = "Pending";
        public static string REQUEST = "Request";

        public static string PDF = "pdf";
        public static string IMAGE = "image";
        public static string SIGN = "sign";

        public static string getDate()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (DateTime.Now.IsDaylightSavingTime() == true)
            {
                now = now.AddHours(1);
            }
            //string currentTime = (string.Format("Current Time: {0}", now));
            string currentDate = now.Year.ToString() + now.Month + now.Day;
            return currentDate;
        }

        public static string getTime()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (DateTime.Now.IsDaylightSavingTime() == true)
            {
                now = now.AddHours(1);
            }
            string currentTime = now.Hour.ToString() + now.Minute + now.Second;
            return currentTime;
        }

        public static string getDateFromFormat(string originalDate)
        {
            string result;
            IDictionary<string, object> properties = Application.Current.Properties;

            if (properties.ContainsKey("dateFormat") && properties["dateFormat"].ToString().Equals("1"))
            {
                result = originalDate.Substring(4, 2) + "-" + originalDate.Substring(6, 2) + "-" + originalDate.Substring(0, 4);
            }
            else
                result = originalDate.Substring(6, 2) + "-" + originalDate.Substring(4, 2) + "-" + originalDate.Substring(0, 4);

            return result;
        }

        public static string changeDateFormat(string orignalDate)
        {
            string result = "";
            result = orignalDate.Substring(4, 2) + "/" + orignalDate.Substring(6, 2) + "/" + orignalDate.Substring(0, 4);
            return result;
        }

        public static string removeZeroFromNumber(string originalNumber)
        {
            string result = "";
            for (int i = 0; i < originalNumber.Length; i++)
            {
                if (!originalNumber.Substring(i, 1).Equals("0"))
                {
                    result += originalNumber.Substring(i, originalNumber.Length - i);
                    break;
                }
            }
            return result;
        }

        public static void ShowToast(string toastMessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(toastMessage).SetDuration(TimeSpan.FromSeconds(10)));
        }
    }
}