using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;
using Smartdocs.Models;
using Smartdocs;
using FormsBackgrounding.Messages;
using Smartdocs.Pages.Invoice.InvoiceDetail.SubViews;

namespace Smartdocs
{
    public partial class InvoicePage : ContentPage
    {
        public InvoicePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            var backButtonTap = new TapGestureRecognizer();
            backButtonTap.Tapped += OnBackButtonClicked;

        }

        private void PopulateList(List<InvoiceModel> list)
        {
            var column = InvoiceRow;
            column.Children.Clear();

            var invoiceItemTapGestureRecognizer = new TapGestureRecognizer();
            invoiceItemTapGestureRecognizer.Tapped += OnInvoiceTapped;

            for (var i = 0; i < list.Count; i++)
            {
                var item = new InvoiceItemTemplate();
                item.BindingContext = list[i];
                item.GestureRecognizers.Add(invoiceItemTapGestureRecognizer);
                //var mainGrid = item.FindByName<StackLayout>("MainContainer");
                //mainGrid.GestureRecognizers.Add (invoiceItemTapGestureRecognizer);
                column.Children.Add(item);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupView.isBack)
            {
                PopupView.isBack = false;
                return true;
            }
            else
                return false;
        }


        private async void OnInvoiceTapped(Object sender, EventArgs e)
        {

            var selectedItem = (InvoiceModel)((InvoiceItemTemplate)sender).BindingContext;
            WorkItem viewItem = new WorkItem();
            foreach (WorkItem item in App.G_WORK_ITEMS)
            {
                if (selectedItem.InvoiceID.Equals(Constants.removeZeroFromNumber(item.docId)))
                {//item.workitemid
                    viewItem = item;
                    break;
                }
            }
            App.G_CURRENT_ACTIVE_ITEM = viewItem;
            App.G_DocId = selectedItem.InvoiceID;

            //display mode
            var displaymode = viewItem.adminData.Mode;

            if (displaymode.Equals("D"))
                App.displayMode = false;
            else if (displaymode.Equals("C"))
                App.displayMode = true;
            else
                App.displayMode = false;

            InvoiceRow.IsVisible = false;
            await Navigation.PushAsync(new InvoiceDetailPage());
        }

        protected override void OnAppearing()
        {

            InvoiceRow.IsVisible = true;

            //pagelabel.Text = App.G_DocType;
            var mh = new MainHeader();
            mh.changeHeaderTitle(App.G_DocType);//it is not working

            var dataFieldsList = new List<DataField>();
            foreach (DocType doctype_item in App.G_DOC_ITEMS)//get dataFields of current doc type
            {
                if (App.G_DocType.Equals(doctype_item.docTypeName))
                {
                    foreach (DataField datafield_item in doctype_item.dataFields)
                    {
                        var temp_dataField = new DataField();
                        temp_dataField.FieldName = datafield_item.FieldName;
                        temp_dataField.Label = datafield_item.Label;
                        temp_dataField.Order = datafield_item.Order;
                        temp_dataField.ListScreenField = datafield_item.ListScreenField;

                        dataFieldsList.Add(temp_dataField);
                    }
                }
            }
            var s_dataFieldList = new List<DataField>();
            // s_dataFieldList = Sort(dataFieldsList);//sort by order value
            s_dataFieldList = SortByListScreenField(dataFieldsList);

            //Revert, if ListScreenField attribute is empty for all doctype. 2016.10.6
            if (s_dataFieldList.Count == 0)
                s_dataFieldList = dataFieldsList;//revert

            var invoiceModels = new List<InvoiceModel>();

            foreach (WorkItem item in App.G_WORK_ITEMS)
            {

                var currentDocType = item.adminData.DocumentType;
                if (App.G_DocType.Equals(currentDocType))
                {
                    string date_value = "";

                    if(s_dataFieldList.Count > 0)
                    {
                        if (s_dataFieldList[1].Label.Equals("Date"))
                        {
                            date_value = item.headerData.getValue(s_dataFieldList[1].FieldName);
                            date_value = Constants.getDateFromFormat(date_value);
                        }
                    }
                    

                    var model = new InvoiceModel
                    {
                        WorkitemTitle = item.adminData.WorkitemTitle,
                        InvoiceID = Constants.removeZeroFromNumber(item.docId),
                        L1 = s_dataFieldList[0].Label,
                        L2 = s_dataFieldList[1].Label,
                        L3 = s_dataFieldList[2].Label,
                        V1 = item.headerData.getValue(s_dataFieldList[0].FieldName),
                        //V2 = date_value,
                        //Fix for CompanyName Display
                        V2 = s_dataFieldList[1].Label.Equals("Date") ? date_value : item.headerData.getValue(s_dataFieldList[1].FieldName),
                        V3 = item.headerData.getValue(s_dataFieldList[2].FieldName)
                    };
                    invoiceModels.Add(model);
                }
            }

            PopulateList(invoiceModels);

            //var inboxModels = new List<InboxViewModel>();

            //foreach (DocType item in App.G_DOC_ITEMS)
            //{
            //	if (item.itemCount != 0)//display only if there are workitems
            //	{
            //		var model = new InboxViewModel
            //		{
            //			Title = item.adminData.Label,
            //			Status = item.adminData.SubLabel + "(" + item.itemCount + ")",
            //			ImageIcon = "invoice.png",
            //			PageType = typeof(InvoicePage),
            //			DocType = item.docTypeName
            //		};

            //		if (item.adminData.WorkItemOnly == "")
            //			App.G_ARRAY_DOCTYPE_ForRequest.Add(model.Title);//no use

            //		App.G_ARRAY_DOCTYPE.Add(model.Title);//no use
            //		inboxModels.Add(model);
            //	}
            //}

            //PopulateList(inboxModels);
        }

        async void OnBackButtonClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        List<DataField> Sort(List<DataField> o_dataFieldList)
        {
            var sorted_dataFieldList = new List<DataField>();
            var origin_dataFieldList = new List<DataField>();
            origin_dataFieldList = o_dataFieldList;

            while (origin_dataFieldList.Count > 0)
            {
                int index = 0;
                int candidate = Convert.ToInt32(origin_dataFieldList[0].Order);

                for (int j = 0; j < origin_dataFieldList.Count; j++)
                {
                    var temp = Convert.ToInt32(origin_dataFieldList[j].Order);
                    if (candidate > temp)
                    {
                        candidate = temp;
                        index = j;
                    }
                }
                sorted_dataFieldList.Add(origin_dataFieldList[index]);
                origin_dataFieldList.RemoveAt(index);

            }

            return sorted_dataFieldList;
        }

        List<DataField> SortByListScreenField(List<DataField> o_dataFieldList)
        {
            var sorted_dataFieldList = new List<DataField>();
            for (int i = 1; i < 4; i++)
            {
                string stringOfInt = "";
                stringOfInt = i.ToString();
                foreach (DataField dataFieldItem in o_dataFieldList)
                {
                    if (dataFieldItem.ListScreenField != null && dataFieldItem.ListScreenField.Equals(stringOfInt))
                        sorted_dataFieldList.Add(dataFieldItem);
                }
            }

            return sorted_dataFieldList;
        }
    }
}

