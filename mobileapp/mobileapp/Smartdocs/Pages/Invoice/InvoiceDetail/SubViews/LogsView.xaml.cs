using System;
using System.Collections.Generic;
using System.Diagnostics;
using Acr.UserDialogs;
using Smartdocs.Models;
using Xamarin.Forms;
using Smartdocs.Pages.Invoice.InvoiceDetail.SubViews;
using Smartdocs.SQLite;
using System.Net;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using System.Globalization;

namespace Smartdocs
{
    public partial class LogsView : ContentView
    {
        // TODO : Class Level Declartion.
        string approveCommentReq, rejectCommentReq, collaborateCommentReq, collaborateBackCommentReq;
        public List<LineItem> main_data;
        List<LineItem> sorted_mainitem;
        private PopupView popUp;
        private CollobarateBackPopup popUpColBack;

        public LogsView()
        {

            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

                throw;
            }

            var logsModels = new List<Log>();
            logsModels = App.G_CURRENT_ACTIVE_ITEM.logs;
            PopulateList(logsModels);

            ButtonAddComment.Clicked += AddComment;

            // TODO:Confgigure main table data.
            var activeItem = App.G_CURRENT_ACTIVE_ITEM;
            main_data = new List<LineItem>();

            sorted_mainitem = new List<LineItem>();

            string tracking = "";

            foreach (DocType doctype_item in App.G_DOC_ITEMS)
            {
                if (App.G_DocType.Equals(doctype_item.docTypeName))
                {
                    foreach (DataField datafield_item in doctype_item.dataFields)
                    {
                        if (datafield_item.LineItemType.Equals(""))
                        {
                            var data_value = activeItem.headerData.getValue(datafield_item.FieldName);//get value according to field name

                            DateTime dt = new DateTime();
                            if (datafield_item.DataType.Equals("Date"))
                            {
                                if (!data_value.Equals("") && !data_value.Equals("00.00.0000"))
                                {
                                    tracking += data_value.ToString();
                                    tracking += "/";
                                    //dt = System.Convert.ToDateTime(Constants.changeDateFormat(data_value));//it is crashing in s's device
                                    dt = DateTime.ParseExact(Constants.changeDateFormat(data_value), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                }
                            }

                            var lineitem = new LineItem
                            {
                                Order = datafield_item.Order,
                                FieldType = datafield_item.DataType,
                                BarcodeField = datafield_item.BarCodeField,
                                VisibleLength = datafield_item.VisibleLength,
                                FieldName = datafield_item.FieldName,
                                Material = datafield_item.Label,
                                Amount = data_value,
                                DateData = dt
                            };
                            main_data.Add(lineitem);
                        }
                    }
                }
            }


            // TODO: Add Footer for this screen.
            //var activeItem = App.G_CURRENT_ACTIVE_ITEM;

          
        }

        

        void AddComment(Object sender, EventArgs e)
        {
            var test = new StandardViewModel(UserDialogs.Instance);
            test.PromptApproveComment.Execute(null);
        }

        void PopulateList(List<Log> list)
        {
            var column = LogRow;
            column.Children.Clear();
            View item;

            for (var i = 0; i < list.Count; i++)
            {
                var logItemTapped = new TapGestureRecognizer();
                if (i % 2 == 0)
                {
                    item = new LogsViewItemTemplate();
                    logItemTapped.Tapped += OnLeftItemTapped;
                }
                else
                {
                    item = new LogsViewItemTemplate();
                    logItemTapped.Tapped += OnRightItemTapped;
                }

                list[i].User = list[i].Activity + " by " + list[i].User;
                list[i].Date = Constants.getDateFromFormat(list[i].Date);
                item.BindingContext = list[i];
                item.GestureRecognizers.Add(logItemTapped);
                column.Children.Add(item);
            }

        }

        private void OnLeftItemTapped(Object sender, EventArgs e)
        {
            var selectedItem = (Log)((LogsViewItemTemplate)sender).BindingContext;
            try
            {
                UserDialogs.Instance.AlertAsync("", selectedItem.Comments, "OK", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Test", ex.ToString());
            }

        }

        private void OnRightItemTapped(Object sender, EventArgs e)
        {
            var selectedItem = (Log)((LogsViewItemTemplate)sender).BindingContext;
            try
            {
                UserDialogs.Instance.AlertAsync("", selectedItem.Comments, "OK", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Test", ex.ToString());
            }

        }
    }
}

