﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text;

namespace Smartdocs.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		public INavigation Navigation { get; set; }

		internal virtual Task Initialize(params object[] args)
		{
			return Task.FromResult(0);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null) return;
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetObservableProperty<T>(
			ref T field,
			T value,
			[CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			field = value;
			OnPropertyChanged(propertyName);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}


