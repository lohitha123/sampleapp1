using Smartdocs.Models;
using System;
using System.Collections.Generic;

namespace Smartdocs.Models
{
    public class Company
    {
        public string companyURL { get; set; }
        public string authenticationType { get; set; }
    }

    public class AdminData
    {
        public string DocumentType { get; set; }
        public string SAPWorkItem { get; set; }
        public string Mode { get; set; }
        public string WorkitemTitle { get; set; }
        public string WorkitemDate { get; set; }
        public string Status { get; set; }
    }

    public class HeaderData
    {
        public string Advance_amount { get; set; }
        public string Company_name { get; set; }
        public string Other_deductions { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks1 { get; set; }
        public string Nature_of_work { get; set; }
        public string Retention { get; set; }
        public string Fund_Centre_Name { get; set; }
        public string Project_site { get; set; }
        public string WBS_Element { get; set; }
        public string Cost_centre_No { get; set; }
        public string Date { get; set; }
        public string Invoice_Value { get; set; }
        public string Payment_terms1 { get; set; }
        public string GL_Name { get; set; }
        public string GL_Cmtmt_item { get; set; }
        public string Vendor_Code { get; set; }
        public string Payment_terms3 { get; set; }
        public string Net_amount_to_be_paid { get; set; }
        public string Payment_terms2 { get; set; }
        public string Invoice_No { get; set; }
        public string Company_Code { get; set; }
        public string Budgeted_Amount { get; set; }
        public string Plant { get; set; }
        public string TDS_amount { get; set; }
        public string Budget_Utilized { get; set; }
        public string Vendor_Name { get; set; }
        public string Remarks3 { get; set; }
        public string Invoice_Date { get; set; }
        public string Agmt_Work_order_No { get; set; }
        public string Material_Service_status { get; set; }
        public string Cost_centre_Name { get; set; }
        public string Fund_Centre_No { get; set; }
        public string Network_Activity_No { get; set; }
        public string Inspection_Status { get; set; }
        public string Proforma_Invoice_No { get; set; }
        public string Project_Site { get; set; }
        public string Proforma_Invoice_Value { get; set; }
        public string Reference_No { get; set; }
        public string Net_Amount_to_be_paid { get; set; }
        public string Advance_Amount { get; set; }
        public string Cost_Centre_Name { get; set; }
        public string Inspected_by { get; set; }
        public string Network_activity_No { get; set; }
        public string Doc_Type { get; set; }
        public string Purchase_Group { get; set; }
        public string PO_Value { get; set; }
        public string Nature_Of_Contract { get; set; }
        public string Warranty { get; set; }
        public string Frieght_scope { get; set; }
        public string Project_Common_Infra { get; set; }
        public string Insurance_scope { get; set; }
        public string Purchase_Document_Type { get; set; }
        public string Vendor { get; set; }
        public string PO_Date { get; set; }
        public string Purchasing_Type { get; set; }
        public string Purchasing_Group { get; set; }
        public string Project { get; set; }
        public string Purchase_Order { get; set; }
        public string Work_Description { get; set; }
        public string GRN_Type { get; set; }
        public string PO_Number { get; set; }
        public string Mtrl_Work_Status2 { get; set; }
        public string Invoice_DC_JMS_No { get; set; }
        public string Mtrl_Work_Status1 { get; set; }
        public string Invoice_DC_JMS_Date { get; set; }
        public string Advance_paid { get; set; }
        public string WTG_Model { get; set; }
        public string Reference_Id { get; set; }
        public string Scope_of_Work_BUIL { get; set; }
        public string Site_Name { get; set; }
        public string WTG_Capacity { get; set; }
        public string WTG_Quantity { get; set; }
        public string WTG_Make { get; set; }
        public string Scope_of_Work_SPV { get; set; }
        public string SPV_Name { get; set; }
        public string State { get; set; }
        public string Scope_of_Work_MEIL { get; set; }
        public string Capacity { get; set; }
        public string Phase { get; set; }

        public string Currency { get; set; }
        public string Details { get; set; }
        public string Material_code { get; set; }

        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string SmartDocID { get; set; }
        public string CreateDate { get; set; }
        public string Comments { get; set; }

        //add 9.30
        public string Budget_Available { get; set; }
        public string Agreement_Work_order_No { get; set; }
        public string Retention_Amount { get; set; }
        public string Retention_Doc_No { get; set; }
        public string Net_Amount_Payable { get; set; }
        public string Material_Receipt_Status { get; set; }

        //add 10.20
        public string PO_NO { get; set; }
        public string GR_No_SES_No { get; set; }

        //add 10.30
        public string Due_Date_As_Per_PO { get; set; }
        //
        // Coromandal Headers
        //public string SBU_DES { get; set; }
        // public string LOCATION_DES { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Total_Amount { get; set; }
        public string Valid_Date { get; set; }
        public string Justification_Summary { get; set; }
        public string Recommended_Vendor { get; set; }
        public string Recommended_Vendor_Name { get; set; }
        public string Recommended_Vendor_Landed_Cost { get; set; }
        public string SBU { get; set; }
        public string Location { get; set; }
        // public string DEPT { get; set; }
        //  public string CATEGORY { get; set; }
        //  public string SCATEGORY { get; set; }
        public string Value { get; set; }
        public string Tax { get; set; }
        public string Cost_Center { get; set; }
        public string CEP_Approval_No { get; set; }
        public string RFWD_No { get; set; }
        public string Proposed_CEP_Amount { get; set; }
        public string RFWD_Value { get; set; }

        public void setValue(string from, string value1)
        {
            if (from.Equals("Advance_amount")) Advance_amount = value1;
            if (from.Equals("Company_name")) Company_name = value1;
            if (from.Equals("Other_deductions")) Other_deductions = value1;
            if (from.Equals("Remarks2")) Remarks2 = value1;
            if (from.Equals("Remarks1")) Remarks1 = value1;
            if (from.Equals("Nature_of_work")) Nature_of_work = value1;
            if (from.Equals("Retention")) Retention = value1;
            if (from.Equals("Fund_Centre_Name")) Fund_Centre_Name = value1;
            if (from.Equals("Project_site")) Project_site = value1;
            if (from.Equals("WBS_Element")) WBS_Element = value1;
            if (from.Equals("Cost_centre_No")) Cost_centre_No = value1;
            if (from.Equals("Date")) Date = value1;
            if (from.Equals("Invoice_Value")) Invoice_Value = value1;
            if (from.Equals("Payment_terms1")) Payment_terms1 = value1;
            if (from.Equals("GL_Name")) GL_Name = value1;
            if (from.Equals("GL_Cmtmt_item")) GL_Cmtmt_item = value1;
            if (from.Equals("Vendor_Code")) Vendor_Code = value1;
            if (from.Equals("Payment_terms3")) Payment_terms3 = value1;
            if (from.Equals("Net_amount_to_be_paid")) Net_amount_to_be_paid = value1;
            if (from.Equals("Payment_terms2")) Payment_terms2 = value1;
            if (from.Equals("Invoice_No")) Invoice_No = value1;
            if (from.Equals("Company_Code")) Company_Code = value1;
            if (from.Equals("Budgeted_Amount")) Budgeted_Amount = value1;
            if (from.Equals("Plant")) Plant = value1;
            if (from.Equals("TDS_amount")) TDS_amount = value1;
            if (from.Equals("Budget_Utilized")) Budget_Utilized = value1;
            if (from.Equals("Vendor_Name")) Vendor_Name = value1;
            if (from.Equals("Remarks3")) Remarks3 = value1;
            if (from.Equals("Invoice_Date")) Invoice_Date = value1;
            if (from.Equals("Agmt_Work_order_No")) Agmt_Work_order_No = value1;
            if (from.Equals("Material_Service_status")) Material_Service_status = value1;
            if (from.Equals("Cost_centre_Name")) Cost_centre_Name = value1;
            if (from.Equals("Fund_Centre_No")) Fund_Centre_No = value1;
            if (from.Equals("Network_Activity_No")) Network_Activity_No = value1;
            if (from.Equals("Inspection_Status")) Inspection_Status = value1;
            if (from.Equals("Proforma_Invoice_No")) Proforma_Invoice_No = value1;
            if (from.Equals("Proforma_Invoice_Value")) Proforma_Invoice_Value = value1;
            if (from.Equals("Reference_No")) Reference_No = value1;
            if (from.Equals("Net_Amount_to_be_paid")) Net_Amount_to_be_paid = value1;
            if (from.Equals("Advance_Amount")) Advance_Amount = value1;
            if (from.Equals("Cost_Centre_Name")) Cost_Centre_Name = value1;
            if (from.Equals("Inspected_by")) Inspected_by = value1;
            if (from.Equals("Doc_Type")) Doc_Type = value1;
            if (from.Equals("Purchase_Group")) Purchase_Group = value1;
            if (from.Equals("PO_Value")) PO_Value = value1;
            if (from.Equals("Nature_Of_Contract")) Nature_Of_Contract = value1;
            if (from.Equals("Warranty")) Warranty = value1;
            if (from.Equals("Frieght_scope")) Frieght_scope = value1;
            if (from.Equals("Project_Common_Infra")) Project_Common_Infra = value1;
            if (from.Equals("Insurance_scope")) Insurance_scope = value1;
            if (from.Equals("Purchase_Document_Type")) Purchase_Document_Type = value1;
            if (from.Equals("Vendor")) Vendor = value1;
            if (from.Equals("PO_Date")) PO_Date = value1;
            if (from.Equals("Purchasing_Type")) Purchasing_Type = value1;
            if (from.Equals("Purchasing_Group")) Purchasing_Group = value1;
            if (from.Equals("Purchase_Order")) Purchase_Order = value1;
            if (from.Equals("Work_Description")) Work_Description = value1;
            if (from.Equals("GRN_Type")) GRN_Type = value1;
            if (from.Equals("PO_Number")) PO_Number = value1;
            if (from.Equals("Mtrl_Work_Status2")) Mtrl_Work_Status2 = value1;
            if (from.Equals("Invoice_DC_JMS_No")) Invoice_DC_JMS_No = value1;
            if (from.Equals("Mtrl_Work_Status1")) Mtrl_Work_Status1 = value1;
            if (from.Equals("Invoice_DC_JMS_Date")) Invoice_DC_JMS_Date = value1;
            if (from.Equals("Advance_paid")) Advance_paid = value1;
            if (from.Equals("WTG_Model")) WTG_Model = value1;
            if (from.Equals("Reference_Id")) Reference_Id = value1;
            if (from.Equals("Scope_of_Work_BUIL")) Scope_of_Work_BUIL = value1;
            if (from.Equals("Site_Name")) Site_Name = value1;
            if (from.Equals("WTG_Capacity")) WTG_Capacity = value1;
            if (from.Equals("WTG_Quantity")) WTG_Quantity = value1;
            if (from.Equals("WTG_Make")) WTG_Make = value1;
            if (from.Equals("Scope_of_Work_SPV")) Scope_of_Work_SPV = value1;
            if (from.Equals("SPV_Name")) SPV_Name = value1;
            if (from.Equals("State")) State = value1;
            if (from.Equals("Scope_of_Work_MEIL")) Scope_of_Work_MEIL = value1;
            if (from.Equals("Capacity")) Capacity = value1;
            if (from.Equals("Phase")) Phase = value1;

            if (from.Equals("Currency")) Currency = value1;
            if (from.Equals("Details")) Details = value1;
            if (from.Equals("Material_code")) Material_code = value1;

            if (from.Equals("InvoiceDate")) InvoiceDate = value1;
            if (from.Equals("InvoiceType")) InvoiceType = value1;
            if (from.Equals("SmartDocID")) SmartDocID = value1;
            if (from.Equals("CreateDate")) CreateDate = value1;
            if (from.Equals("Comments")) Comments = value1;

            //add 9.30
            if (from.Equals("Budget_Available")) Budget_Available = value1;
            if (from.Equals("Agreement_Work_order_No")) Agreement_Work_order_No = value1;
            if (from.Equals("Retention_Amount")) Retention_Amount = value1;
            if (from.Equals("Retention_Doc_No")) Retention_Doc_No = value1;
            if (from.Equals("Net_Amount_Payable")) Net_Amount_Payable = value1;
            if (from.Equals("Material_Receipt_Status")) Material_Receipt_Status = value1;

            if (from.Equals("PO_NO")) PO_NO = value1;
            if (from.Equals("GR_No_SES_No")) GR_No_SES_No = value1;

            //add 10.30
            if (from.Equals("Due_Date_As_Per_PO")) Due_Date_As_Per_PO = value1;

            //Coromandal Headers
            //    if (from.Equals("SBU_DES")) SBU_DES = value1;
            //  if (from.Equals("LOCATION_DES")) LOCATION_DES = value1;
            if (from.Equals("Department")) Department = value1;
            if (from.Equals("Category")) Category = value1;
            if (from.Equals("SubCategory")) SubCategory = value1;
            if (from.Equals("Total_Amount")) Total_Amount = value1;
            if (from.Equals("Valid_Date")) Valid_Date = value1;
            if (from.Equals("Justification_Summary ")) Justification_Summary = value1;
            if (from.Equals("Recommended_Vendor")) Recommended_Vendor = value1;
            if (from.Equals("Recommended_Vendor_Name")) Recommended_Vendor_Name = value1;
            if (from.Equals("Recommended_Vendor_Landed_Cost")) Recommended_Vendor_Landed_Cost = value1;
            if (from.Equals("SBU")) SBU = value1;
            if (from.Equals("Location")) Location = value1;
            //    if (from.Equals("DEPT")) DEPT = value1;
            //    if (from.Equals("CATEGORY")) CATEGORY = value1;
            //   if (from.Equals("SCATEGORY")) SCATEGORY = value1;
            if (from.Equals("Value")) Value = value1;
            if (from.Equals("Tax")) Tax = value1;
            if (from.Equals("Cost_Center")) Cost_Center = value1;
            if (from.Equals("CEP_Approval_No")) CEP_Approval_No = value1;
            if (from.Equals("RFWD_No")) RFWD_No = value1;
            if (from.Equals("Proposed_CEP_Amount")) Proposed_CEP_Amount = value1;
            if (from.Equals("RFWD_Value")) RFWD_Value = value1;
        }

        public string getValue(string from)
        {
            if (from.Equals("Advance_amount")) return Advance_amount;
            if (from.Equals("Company_name")) return Company_name;
            if (from.Equals("Other_deductions")) return Other_deductions;
            if (from.Equals("Remarks2")) return Remarks2;
            if (from.Equals("Remarks1")) return Remarks1;
            if (from.Equals("Nature_of_work")) return Nature_of_work;
            if (from.Equals("Retention")) return Retention;
            if (from.Equals("Fund_Centre_Name")) return Fund_Centre_Name;
            if (from.Equals("Project_site")) return Project_site;
            if (from.Equals("WBS_Element")) return WBS_Element;
            if (from.Equals("Cost_centre_No")) return Cost_centre_No;
            if (from.Equals("Date")) return Date;
            if (from.Equals("Invoice_Value")) return Invoice_Value;
            if (from.Equals("Payment_terms1")) return Payment_terms1;
            if (from.Equals("GL_Name")) return GL_Name;
            if (from.Equals("GL_Cmtmt_item")) return GL_Cmtmt_item;
            if (from.Equals("Vendor_Code")) return Vendor_Code;
            if (from.Equals("Payment_terms3")) return Payment_terms3;
            if (from.Equals("Net_amount_to_be_paid")) return Net_amount_to_be_paid;
            if (from.Equals("Payment_terms2")) return Payment_terms2;
            if (from.Equals("Invoice_No")) return Invoice_No;
            if (from.Equals("Company_Code")) return Company_Code;
            if (from.Equals("Budgeted_Amount")) return Budgeted_Amount;
            if (from.Equals("Plant")) return Plant;
            if (from.Equals("TDS_amount")) return TDS_amount;
            if (from.Equals("Budget_Utilized")) return Budget_Utilized;
            if (from.Equals("Vendor_Name")) return Vendor_Name;
            if (from.Equals("Remarks3")) return Remarks3;
            if (from.Equals("Invoice_Date")) return Invoice_Date;
            if (from.Equals("Agmt_Work_order_No")) return Agmt_Work_order_No;
            if (from.Equals("Material_Service_status")) return Material_Service_status;
            if (from.Equals("Cost_centre_Name")) return Cost_centre_Name;
            if (from.Equals("Fund_Centre_No")) return Fund_Centre_No;
            if (from.Equals("Network_Activity_No")) return Network_Activity_No;
            if (from.Equals("Inspection_Status")) return Inspection_Status;
            if (from.Equals("Proforma_Invoice_No")) return Proforma_Invoice_No;
            if (from.Equals("Proforma_Invoice_Value")) return Proforma_Invoice_Value;
            if (from.Equals("Reference_No")) return Reference_No;
            if (from.Equals("Net_Amount_to_be_paid")) return Net_Amount_to_be_paid;
            if (from.Equals("Advance_Amount")) return Advance_Amount;
            if (from.Equals("Cost_Centre_Name")) return Cost_Centre_Name;
            if (from.Equals("Inspected_by")) return Inspected_by;
            if (from.Equals("Doc_Type")) return Doc_Type;
            if (from.Equals("Purchase_Group")) return Purchase_Group;
            if (from.Equals("PO_Value")) return PO_Value;
            if (from.Equals("Nature_Of_Contract")) return Nature_Of_Contract;
            if (from.Equals("Warranty")) return Warranty;
            if (from.Equals("Frieght_scope")) return Frieght_scope;
            if (from.Equals("Project_Common_Infra")) return Project_Common_Infra;
            if (from.Equals("Insurance_scope")) return Insurance_scope;
            if (from.Equals("Purchase_Document_Type")) return Purchase_Document_Type;
            if (from.Equals("Vendor")) return Vendor;
            if (from.Equals("PO_Date")) return PO_Date;
            if (from.Equals("Purchasing_Type")) return Purchasing_Type;
            if (from.Equals("Purchasing_Group")) return Purchasing_Group;
            if (from.Equals("Purchase_Order")) return Purchase_Order;
            if (from.Equals("Work_Description")) return Work_Description;
            if (from.Equals("GRN_Type")) return GRN_Type;
            if (from.Equals("PO_Number")) return PO_Number;
            if (from.Equals("Mtrl_Work_Status2")) return Mtrl_Work_Status2;
            if (from.Equals("Invoice_DC_JMS_No")) return Invoice_DC_JMS_No;
            if (from.Equals("Mtrl_Work_Status1")) return Mtrl_Work_Status1;
            if (from.Equals("Invoice_DC_JMS_Date")) return Invoice_DC_JMS_Date;
            if (from.Equals("Advance_paid")) return Advance_paid;
            if (from.Equals("WTG_Model")) return WTG_Model;
            if (from.Equals("Reference_Id")) return Reference_Id;
            if (from.Equals("Scope_of_Work_BUIL")) return Scope_of_Work_BUIL;
            if (from.Equals("Site_Name")) return Site_Name;
            if (from.Equals("WTG_Capacity")) return WTG_Capacity;
            if (from.Equals("WTG_Quantity")) return WTG_Quantity;
            if (from.Equals("WTG_Make")) return WTG_Make;
            if (from.Equals("Scope_of_Work_SPV")) return Scope_of_Work_SPV;
            if (from.Equals("SPV_Name")) return SPV_Name;
            if (from.Equals("State")) return State;
            if (from.Equals("Scope_of_Work_MEIL")) return Scope_of_Work_MEIL;
            if (from.Equals("Capacity")) return Capacity;
            if (from.Equals("Phase")) return Phase;

            if (from.Equals("Currency")) return Currency;
            if (from.Equals("Details")) return Details;
            if (from.Equals("Material_code")) return Material_code;

            if (from.Equals("InvoiceDate")) return InvoiceDate;
            if (from.Equals("InvoiceType")) return InvoiceType;
            if (from.Equals("SmartDocID")) return SmartDocID;
            if (from.Equals("CreateDate")) return CreateDate;
            if (from.Equals("Comments")) return Comments;

            //add 9.30
            if (from.Equals("Budget_Available")) return Budget_Available;
            if (from.Equals("Agreement_Work_order_No")) return Agreement_Work_order_No;
            if (from.Equals("Retention_Amount")) return Retention_Amount;
            if (from.Equals("Retention_Doc_No")) return Retention_Doc_No;
            if (from.Equals("Net_Amount_Payable")) return Net_Amount_Payable;
            if (from.Equals("Material_Receipt_Status")) return Material_Receipt_Status;

            if (from.Equals("PO_NO")) return PO_NO;
            if (from.Equals("GR_No_SES_No")) return GR_No_SES_No;

            //
            if (from.Equals("Due_Date_As_Per_PO")) return Due_Date_As_Per_PO;

            //Coromandal CHnages
            //  if (from.Equals("SBU_DES")) return SBU_DES;
            //   if (from.Equals("LOCATION_DES")) return LOCATION_DES;
            if (from.Equals("Department")) return Department;
            if (from.Equals("Category")) return Category;
            if (from.Equals("SubCategory")) return SubCategory;
            if (from.Equals("Total_Amount")) return Total_Amount;
            if (from.Equals("Valid_Date")) return Valid_Date;
            if (from.Equals("Justification_Summary ")) return Justification_Summary;
            if (from.Equals("Recommended_Vendor")) return Recommended_Vendor;
            if (from.Equals("Recommended_Vendor_Name")) return Recommended_Vendor_Name;
            if (from.Equals("Recommended_Vendor_Landed_Cost")) return Recommended_Vendor_Landed_Cost;
            if (from.Equals("SBU")) return SBU;
            if (from.Equals("Location")) return Location;
            //  if (from.Equals("DEPT")) return DEPT;
            //  if (from.Equals("CATEGORY")) return CATEGORY;
            //  if (from.Equals("SCATEGORY")) return SCATEGORY;
            if (from.Equals("Value")) return Value;
            if (from.Equals("Tax")) return Tax;
            if (from.Equals("Cost_Center")) return Cost_Center;
            if (from.Equals("CEP_Approval_No")) return CEP_Approval_No;
            if (from.Equals("RFWD_No")) return RFWD_No;
            if (from.Equals("Proposed_CEP_Amount")) return Proposed_CEP_Amount;
            if (from.Equals("RFWD_Value")) return RFWD_Value;

            else return "";
        }
    }

    public class LineItem
    {
        public string FieldType { get; set; }
        public string BarcodeField { get; set; }
        public string FieldName { get; set; }
        public string Mandatory { get; set; }
        public string VisibleLength { get; set; }
        public string Order { get; set; }

        public DateTime DateData { get; set; }
        public string Amount { get; set; }//main value
        public string Material { get; set; }//main label

        public string getValue(string from)
        {
            if (from.Equals("Amount")) return Amount;
            if (from.Equals("Material")) return Material;

            else return "";
        }
    }

    public class Log
    {
        public string User { get; set; }
        public string Comments { get; set; }
        public string Activity { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
    }

    public class Activity
    {
        public string CommentsRequired { get; set; }
        public string ButtonText { get; set; }
        public string ActivityName { get; set; }
        public string Icon { get; set; }
    }

    public class Attachment
    {
        public string FolderName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }
    }

    public class WorkItem
    {
        public string workItemId { get; set; }
        public string docId { get; set; }
        public AdminData adminData { get; set; }
        public HeaderData headerData { get; set; }
        public List<Log> logs { get; set; }
        public List<Activity> activities { get; set; }
        public List<Attachment> attachments { get; set; }
        public List<LineItem> lineitemData { get; set; }
        public List<UserDetails> userDetailsData { get; set; }
    }
}

//for DocType
public class DataField
{
    public string FieldName { get; set; }
    public string Label { get; set; }
    public string DataType { get; set; }
    public string Tab { get; set; }
    public string Order { get; set; }
    public string Length { get; set; }
    public string BarCodeField { get; set; }
    public string Mandatory { get; set; }
    public string LineItemType { get; set; }
    public string PossibleValues { get; set; }
    public string VisibleLength { get; set; }
    public string ListScreenField { get; set; }
}

public class DocTypeAdminData
{
    public string DocTypeDesc { get; set; }
    public string SignaturePad { get; set; }
    public string WorkItemOnly { get; set; }
    public string Label { get; set; }
    public string SubLabel { get; set; }
    public string IconName { get; set; }
}

public class DocType
{
    public string docTypeName { get; set; }
    public DocTypeAdminData adminData { get; set; }
    public List<DataField> dataFields { get; set; }
    public int itemCount { get; set; }
}

//for SubmitWorkItem

public class SubmitWorkItemAdminData
{
    public string documentType { get; set; }
    public string docId { get; set; }
}

public class SubmitWorkItem
{
    public string workitemId { get; set; }
    public SubmitWorkItemAdminData adminData { get; set; }
    public HeaderData headerData { get; set; }
    public Log logs { get; set; }
    public Activity activities { get; set; }
    public List<Attachment> attachments { get; set; }
    public List<LineItem> lineitemData { get; set; }
    public Users collaborated_Users { get; set; }
}

//for submitRequest
public class SubmitRequestAdminData
{
    public string DocumentType { get; set; }
    public string Location { get; set; }
}

public class SubmitRequest
{
    public string newRequestId { get; set; }
    public SubmitRequestAdminData adminData { get; set; }
    public HeaderData headerData { get; set; }
    public Log logs { get; set; }
    public List<Attachment> attachments { get; set; }
    public List<LineItem> lineitemData { get; set; }
}

//for Delta updata
public class DeltaUpdataData
{
    public string remove { get; set; }
    public List<AddData> add { get; set; }
}

public class AddData
{
    public string workItemId { get; set; }
    public string docId { get; set; }
    public string activity { get; set; }
    public string endDate { get; set; }
    public string actualUser { get; set; }
    public AdminData adminData { get; set; }
    public HeaderData headerData { get; set; }
    public List<LineItem> lineitemData { get; set; }
    public List<Log> logs { get; set; }
    public List<Attachment> attachments { get; set; }
}

public class UserDetails
{
    public string UserId { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }
    public String Title { get; set; }
    public string Location { get; set; }
    public string SAPUserId { get; set; }
    public string PhoneNumber { get; set; }
}

public class Users
{
    public List<string> User { get; set; }
}

