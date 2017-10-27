using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FormsBackgrounding.Messages;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Smartdocs.Models;
using Xamarin.Forms;

namespace Smartdocs
{
    public class HttpHandler
    {
        private HttpClient httpClient;
        public string userId;

        public HttpHandler()
        {
            httpClient = new HttpClient();
            //httpClient = new HttpClient(new NativeMessageHandler());//for modern HttpClient
            //{
            //	Timeout = TimeSpan.FromSeconds(150)
            //};
        }

        public async Task<List<DocType>> GetDocTypeAsync()
        {
            try
            {
                IDictionary<string, object> properties = Application.Current.Properties;
                if (properties.ContainsKey("userId"))
                    userId = properties["userId"].ToString();

                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);
                var s = Constants.SECRET_TOKEN;

                var request = new HttpRequestMessage(HttpMethod.Get, Constants.GETDOCTYPE_API + userId);

                var response = await httpClient.SendAsync(request);

                string result = "";

                var docTypeList = new List<DocType>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;

                    JToken jsonArray = JValue.Parse(result);

                    foreach (var json in jsonArray)
                    {
                        DocType tmp = JsonConvert.DeserializeObject<DocType>(json.ToString());
                        docTypeList.Add(tmp);
                    }
                }

                return docTypeList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<WorkItem>> GetAllWorkItemsAsync(string apiurl)
        {
            try
            {

                IDictionary<string, object> properties = Application.Current.Properties;
                //if (properties.ContainsKey("userId"))
                //    userId = properties["userId"].ToString();

                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);
                var s = Constants.SECRET_TOKEN;

                var request = new HttpRequestMessage(HttpMethod.Get, apiurl + userId);

                var response = await httpClient.SendAsync(request);

                string result = "";

                var workItemList = new List<WorkItem>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;

                    JToken jsonArray = JValue.Parse(result);

                    foreach (var json in jsonArray)
                    {
                        WorkItem tmp = JsonConvert.DeserializeObject<WorkItem>(json.ToString());
                        workItemList.Add(tmp);
                    }
                }

                if (apiurl.Equals(Constants.GETWORKITEM_API))
                {
                    // start polling data when online
                    var start_message = new StartLongRunningTaskMessage();
                    MessagingCenter.Send(start_message, "StartLongRunningTaskMessage");
                }

                return workItemList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<DeltaUpdataData> GetWorkItemsDeltaDataAsync(string apiurl)
        {
            var workItemDeltaData = new DeltaUpdataData();

            try
            {

                IDictionary<string, object> properties = Application.Current.Properties;
                if (properties.ContainsKey("userId"))
                    userId = properties["userId"].ToString();

                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);
                var s = Constants.SECRET_TOKEN;

                var request = new HttpRequestMessage(HttpMethod.Get, apiurl + "D/" + userId);

                var response = await httpClient.SendAsync(request);

              var  result = response.Content.ReadAsStringAsync().Result;
                JToken jsonArray = JValue.Parse(result);

                var remove = jsonArray["remove"].ToString();
                var add = jsonArray["add"];
                workItemDeltaData.remove = remove;
                var lstAddData= new List<AddData>();
                foreach (var json in add)
                {            
                    var addData = JsonConvert.DeserializeObject<AddData>(json.ToString());
                    lstAddData.Add(addData);
                }

                workItemDeltaData.add = lstAddData;

                if(workItemDeltaData.add != null && workItemDeltaData.add.Count > 0)
                {
                    return workItemDeltaData;
                }
                else
                {
                    return null;
                }

              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<string>> GetCompanyUrlAsync(string companyid)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, Constants.GETCOMPANY_API + companyid);

                var response = await httpClient.SendAsync(request);

                string result = "";
                var comUrlList = new List<string>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;

                    JToken jsonArray = JValue.Parse(result);

                    comUrlList.Add(jsonArray["internalURL"].ToString());
                    comUrlList.Add(jsonArray["externalURL"].ToString());

                }

                return comUrlList;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<HttpResponseMessage> LoginAsync(string username, string password)
        {
            try
            {
                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.LOGIN_API);
                request.Content = new StringContent("{\"userId\":\"" + username + "\",\"password\": \"" + password + "\"}",
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.SendAsync(request);

                return response;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<HttpResponseMessage> SubmitWorkItemAsync(SubmitWorkItem item, string apiurl)
        {
            try
            {
                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);

                var json = JsonConvert.SerializeObject(item);

                var request = new HttpRequestMessage(HttpMethod.Post, apiurl);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<HttpResponseMessage> SubmitRequestAsync(SubmitRequest item, string apiurl)
        {
            try
            {
                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);

                var json = JsonConvert.SerializeObject(item);

                var request = new HttpRequestMessage(HttpMethod.Post, apiurl);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<string> uploadImage(byte[] image)
        {
            string resultURL = "";
            try
            {
                //StreamContent scontent = new StreamContent(mediaFile.GetStream());
                var byteContent = new ByteArrayContent(image);
                byteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "newimage",
                    Name = "image"
                };
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                var multi = new MultipartFormDataContent();
                multi.Add(byteContent);
                httpClient.BaseAddress = new Uri(Constants.SERVER);

                HttpResponseMessage result = await httpClient.PostAsync(Constants.SaveAttachment_API, multi);
                string resContent = await result.Content.ReadAsStringAsync();

                JToken jsonArray = JValue.Parse(resContent);
                resultURL = jsonArray["URL"].ToString();
                Debug.WriteLine(resultURL);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                resultURL = null;
            }

            return resultURL;
        }

        public async Task<List<UserDetails>> GetAllUsersAsync()
        {
            try
            {
                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);
                var s = Constants.SECRET_TOKEN;

                var request = new HttpRequestMessage(HttpMethod.Get, Constants.GETUSERS_API);
                var response = await httpClient.SendAsync(request);
                string result = "";
                var userDetailsList = new List<UserDetails>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    JToken jsonArray = JValue.Parse(result);
                    foreach (var json in jsonArray)
                    {
                        UserDetails tmp = JsonConvert.DeserializeObject<UserDetails>(json.ToString());
                        userDetailsList.Add(tmp);
                    }
                }
                return userDetailsList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<UserDetails>> GetAllUsersAsync(string docid)
        {
            try
            {
                httpClient.BaseAddress = new Uri(Constants.SERVER);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Constants.SECRET_TOKEN);
                var s = Constants.SECRET_TOKEN;

                var request = new HttpRequestMessage(HttpMethod.Get, Constants.getStepDataUsers+ docid);
                var response = await httpClient.SendAsync(request);
                string result = "";
                var userDetailsList = new List<UserDetails>();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    JToken jsonArray = JValue.Parse(result);
                    foreach (var json in jsonArray)
                    {
                        UserDetails tmp = JsonConvert.DeserializeObject<UserDetails>(json.ToString());
                        userDetailsList.Add(tmp);
                    }
                }
                return userDetailsList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }
    }

}
