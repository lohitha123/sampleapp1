using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smartdocs.Custom
{
    public class CustomEditor : Editor
    {
        /// <summary>
        /// PlaceholderProperty Property
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create<CustomEditor, string>(p => p.Placeholder, string.Empty);

        /// <summary>
        ///  MaxLengthProperty Property
        /// </summary>
        //public static readonly BindableProperty MaxLengthProperty =
        //    BindableProperty.Create("MaxLength", typeof(int), typeof(CustomEditor), int.MaxValue);



        public static BindableProperty PlaceholderColorProperty
           = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(CustomEditor), Color.Gray);



        /// <summary>
        /// HasBordersProperty Property
        /// </summary>
        public static readonly BindableProperty HasBordersProperty =
            BindableProperty.Create("HasBorders", typeof(bool), typeof(CustomEditor), true);

        /// <summary>
        /// Placeholder Set or Get
        /// </summary>
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }


        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }



        /// <summary>
        /// MaxLength set or get
        /// </summary>
        //public int MaxLength
        //{
        //    get { return (int)this.GetValue(MaxLengthProperty); }
        //    set { this.SetValue(MaxLengthProperty, value); }
        //}

        /// <summary>
        /// HasBorders Set or Get
        /// </summary>
        public bool HasBorders
        {
            get { return (bool)this.GetValue(HasBordersProperty); }
            set { this.SetValue(HasBordersProperty, value); }
        }

    }

}

