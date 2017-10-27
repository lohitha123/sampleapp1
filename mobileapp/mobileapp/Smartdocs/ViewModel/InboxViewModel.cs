using System;

namespace Smartdocs
{
	public class InboxViewModel
	{
		public string Title { get; set; }
		public string Status { get; set; }
		public string ImageIcon { get; set; }
		public Type PageType { get; set; }
		public string DocType { get; set; }

		public InboxViewModel( )
		{
			
		}
	}
}

