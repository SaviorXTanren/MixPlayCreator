using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MixPlayCreator.WPF.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectUploadCompleteDialogControl.xaml
    /// </summary>
    public partial class ProjectUploadCompleteDialogControl : UserControl
    {
        public ProjectUploadCompleteDialogControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
