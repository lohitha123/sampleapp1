using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Smartdocs
{
	public partial class NewInvoiceComment : ContentView
	{
		public NewInvoiceComment()
		{
			InitializeComponent();

			ButtonSave.Clicked += SaveComment;
		}

		void SaveComment(Object sender, EventArgs e)
		{
			App.requestComment = TextComment.Text;
		}
	}
}

