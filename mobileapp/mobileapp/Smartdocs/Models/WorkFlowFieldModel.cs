using System;

namespace Smartdocs
{
	public class WorkFlowFieldModel
	{
		public WorkFlowFieldModel() {
			FieldId = 0;
			FieldName = "Field Name";
			FieldValue = "0";
			FieldValue = "0";
		}
		public int FieldId { get; set; }
		public string FieldName { get; set; }
		public string FieldValue { get; set; }
		public string SequenceNo { get; set; }
	}
}

