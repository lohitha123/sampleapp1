
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Smartdocs;
namespace Smartdocs
{
   public class KeyboardHelperBR
    {
        public static void HideKeyboard()
        {
            DependencyService.Get<IKeyboardHelper>().DeviceHideKeyboard();
        }
    }
}
