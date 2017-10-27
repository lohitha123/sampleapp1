using System;
using Xamarin.Forms;

namespace Smartdocs
{
	public class EntryExt : Entry
	{
		public const string ReturnKeyPropertyName = "ReturnKeyType";

		public EntryExt() { }

		public static readonly BindableProperty ReturnKeyTypeProperty = BindableProperty.Create(
			propertyName: ReturnKeyPropertyName,
			returnType: typeof(ReturnKeyTypes),
			declaringType: typeof(EntryExt),
			defaultValue: ReturnKeyTypes.Done);

		public ReturnKeyTypes ReturnKeyType
		{
			get { return (ReturnKeyTypes)GetValue(ReturnKeyTypeProperty); }
			set { SetValue(ReturnKeyTypeProperty, value); }
		}
	}

	// Not all of these are support on Android, consult EntryEditText.ImeOptions
	public enum ReturnKeyTypes : int
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergencyCall,
		Continue
	}
}
