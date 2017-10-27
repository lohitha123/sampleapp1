using System;

namespace Smartdocs
{
	public class LogModel
	{
		public LogModel() {
			DocId = "0";
			FlowId = "Field Id";
			WorkItemId = "WorkItem";
			Activity = "Activity";
			Comments = "Comments";
			CurrentAgent = "CurrentAgent";
		}

		public string DocId { get; set; }
		public string FlowId { get; set; }
		public string WorkItemId { get; set; }
		public string Activity { get; set; }
		public string Comments { get; set; }
		public string CurrentAgent { get; set; }

	}
}

