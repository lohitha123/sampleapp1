using System;
using System.Collections.Generic;

namespace Smartdocs
{
	public class HeaderModel
	{
		List<WorkFlowFieldModel> _fields;
		public string WorkItemId { get; set; }

		public List<WorkFlowFieldModel> Fields {
			get { return _fields; }
			set { 
				foreach (WorkFlowFieldModel field in value) {
					WorkFlowFieldModel model = new WorkFlowFieldModel ();
					model.FieldId = field.FieldId;
					model.FieldName = field.FieldName;
					model.FieldValue = field.FieldValue;
					model.SequenceNo = field.SequenceNo;
					_fields.Add (model);
				}
			}
		}


	}
}

