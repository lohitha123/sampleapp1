using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;
using FormsBackgrounding.Messages;
using Smartdocs;
using System.Collections.Generic;
using System;
using Acr.UserDialogs;
using Plugin.LocalNotifications;
using System.Diagnostics;
using Smartdocs.Models;
using System.Net;
using Plugin.Connectivity;

namespace FormsBackgrounding
{
	public class TaskCounter
	{
		IDictionary<string, object> properties = Application.Current.Properties;

		public async Task RunCounter(CancellationToken token)
		{
			await Task.Run (async () => {

				for (long i = 0; i < long.MaxValue; i++) {
					//token.ThrowIfCancellationRequested ();
					Debug.WriteLine(i);

					int pollingFRQ;
					if (properties.ContainsKey("pollingFRQ"))
					{
						if (Convert.ToInt32(properties["pollingFRQ"]) == 0)
							pollingFRQ = 300;
						else if (Convert.ToInt32(properties["pollingFRQ"]) == 1)
							pollingFRQ = 900;
						else
							pollingFRQ = -1;
					}else
						pollingFRQ = 300;

					if (i == pollingFRQ)
					{
						if (CrossConnectivity.Current.IsConnected)
						{
						   	Debug.WriteLine("You are online");

						 await	SendWorkitemFromSQLWhenOnline();

						await	SendRequestWhenOnline();

							var deltaUpdate = await App.G_HTTP_CLIENT.GetWorkItemsDeltaDataAsync(Constants.GETWORKITEM_API);
							if (deltaUpdate != null)
							{
								Device.BeginInvokeOnMainThread(async () =>
								{
									//ShowToast("There is updated data in inbox");
									ShowLocalNotification("There is updated data in inbox");
								});
								App.G_WORK_ITEMS = await App.G_HTTP_CLIENT.GetAllWorkItemsAsync(Constants.GETWORKITEM_API);
							}
							else {
								Device.BeginInvokeOnMainThread(async () =>
								{
									//ShowToast("There is no delta update");
									//ShowLocalNotification("There is no delta update");
								});
								App.G_WORK_ITEMS = await App.G_HTTP_CLIENT.GetAllWorkItemsAsync(Constants.GETWORKITEM_API);
							}

							//var stop_message = new StopLongRunningTaskMessage();
							//MessagingCenter.Send(stop_message, "StopLongRunningTaskMessage");

							break;
						}else
						{
							Debug.WriteLine("You are offline");
							// start polling data WHEN offline
							var start_message = new StartLongRunningTaskMessage();
							MessagingCenter.Send(start_message, "StartLongRunningTaskMessage");

							break;
						}
					}

					await Task.Delay(1000);
					var message = new TickedMessage { 
						Message = i.ToString()
					};

					Device.BeginInvokeOnMainThread(() => {
						MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
					});
				}
			}, token);
		}

		void ShowToast(string toastMessage)
		{
			UserDialogs.Instance.Toast(new ToastConfig(toastMessage).SetDuration(TimeSpan.FromSeconds(6)));
		}

		void ShowLocalNotification(string notificationMsg)
		{
			CrossLocalNotifications.Current.Show("SmartDocs", notificationMsg);
		}

		async Task SendWorkitemFromSQLWhenOnline()
		{
			var pending_workitem_list = new List<WorkItem>();
			var dbInit = new DataAccessLayer(null);
			pending_workitem_list = await dbInit.PullWorkitemData(Constants.PENDING);

			foreach (WorkItem pending_item in pending_workitem_list)
			{
				var activeItem = pending_item;
				var admin_data = new SubmitWorkItemAdminData
				{
					docId = activeItem.docId,
					documentType = activeItem.adminData.DocumentType
				};

				var header_data = new HeaderData();
				header_data = activeItem.headerData;

				var activitydata = new Activity
				{
					ActivityName = "Approve",
					ButtonText = "Approve",
					Icon = "Approve",
					CommentsRequired = "true"
				};

				var logdata = new Log();
				foreach (Log log_item in pending_item.logs)
				{
					logdata = log_item;
				}

				var attachmentdata = new List<Attachment>();
				attachmentdata = activeItem.attachments;

				var submitWorkitemData = new SubmitWorkItem
				{
					workitemId = activeItem.workItemId,
					adminData = admin_data,
					headerData = header_data,
					logs = logdata,
					activities = activitydata,
					attachments = attachmentdata,
					lineitemData = activeItem.lineitemData
				};

				var result = await App.G_HTTP_CLIENT.SubmitWorkItemAsync(submitWorkitemData, Constants.SubmitWorkitem_API);
				if (result != null && result.StatusCode == HttpStatusCode.Created)
				{
					Debug.WriteLine("======Success to SendWorkitemWhenOnline");
					await dbInit.SetWorkitemStatus(activeItem.workItemId, Constants.COMPLETED);

					Device.BeginInvokeOnMainThread(async () =>
					{
						ShowToast("Workitem Synced Succesfully");
					});
				}
				else
				{
					Debug.WriteLine("======Fail to SendWorkitemWhenOnline");
					Device.BeginInvokeOnMainThread(async () =>
					{
						ShowToast("Failed to Sync workitem");
					});
				}
			}
		}

		async Task SendRequestWhenOnline()
		{
			var submitRequest_list = new List<SubmitRequest>();
			var dbInit = new DataAccessLayer(null);
			submitRequest_list = await dbInit.PullSubmitRequestData();

			for (int i = 0; i < submitRequest_list.Count; i++)
			{
				var result = await App.G_HTTP_CLIENT.SubmitRequestAsync(submitRequest_list[i], Constants.SubmitRequest_API);
				if (result.StatusCode == HttpStatusCode.Created)
				{
					var resultString = result.Content.ReadAsStringAsync().Result;
					Debug.WriteLine("======Success to Send Request:" + resultString);
					Device.BeginInvokeOnMainThread(async () =>
					{
						ShowToast("Succeed to send Request:" + resultString);
					});

					await dbInit.RemoveSubmitRequest(submitRequest_list[i].newRequestId);
				}
				else {
					Debug.WriteLine("======Fail to Send Request");
					Device.BeginInvokeOnMainThread(async () =>
					{
						ShowToast("Fail to send Request");
					});
				}
			}

		}
	}
}