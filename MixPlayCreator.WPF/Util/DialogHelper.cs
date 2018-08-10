using MaterialDesignThemes.Wpf;
using MixPlayCreator.WPF.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MixPlayCreator.WPF.Util
{
    public static class DialogHelper
    {
        private static object lastResult = null;

        public static async Task<object> ShowMessageDialog(string message)
        {
            return await DialogHelper.ShowDialog(new MessageDialogControl(message));
        }

        public static async Task<object> ShowConfirmationDialog(string message)
        {
            return await DialogHelper.ShowDialog(new ConfirmationDialogControl(message));
        }

        public static async Task<object> ShowBasicTextEntryDialog(string fieldName)
        {
            BasicTextEntryDialogControl textDialog = new BasicTextEntryDialogControl(fieldName);
            await DialogHelper.ShowDialog(textDialog);
            return textDialog.TextEntry;
        }

        public static async Task<object> ShowDialog(UserControl control)
        {
            IEnumerable<Window> windows = Application.Current.Windows.OfType<Window>();
            if (windows.Count() > 0)
            {
                object obj = windows.FirstOrDefault().FindName("MDDialogHost");
                if (obj != null)
                {
                    DialogHost dialogHost = (DialogHost)obj;
                    dialogHost.DialogClosing += DialogHost_DialogClosing;
                    await dialogHost.ShowDialog(control);
                    dialogHost.DialogClosing -= DialogHost_DialogClosing;
                }
            }
            return lastResult;
        }

        private static void DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            lastResult = (eventArgs.Parameter != null) ? eventArgs.Parameter : null;
        }

        public static void HideDialog()
        {
            IEnumerable<Window> windows = Application.Current.Windows.OfType<Window>();
            if (windows.Count() > 0)
            {
                object obj = windows.FirstOrDefault().FindName("MDDialogHost");
                if (obj != null)
                {
                    DialogHost dialogHost = (DialogHost)obj;
                    dialogHost.IsOpen = false;
                }
            }
        }
    }
}
