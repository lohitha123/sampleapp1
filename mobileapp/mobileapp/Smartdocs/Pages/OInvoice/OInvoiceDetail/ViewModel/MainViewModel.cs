﻿using System;
using Xamarin.Forms;
using Smartdocs;

namespace Smartdocs.Outbox.ViewModels
{
	public class MainViewModel : BaseViewModel, ICarouselViewModel
	{
		public string PageTitle
		{
			get { return "Main"; }
		}

		public ContentView View
		{
			get { return new OutboxMainView(); }
		}
	}
}
