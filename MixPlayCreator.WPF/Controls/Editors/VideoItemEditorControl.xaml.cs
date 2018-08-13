using Microsoft.Win32;
using MixPlayCreator.Base.ViewModel.Items;
using System.Windows.Controls;

namespace MixPlayCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for VideoItemEditorControl.xaml
    /// </summary>
    public partial class VideoItemEditorControl : UserControl
    {
        private const string VideoFileBrowserFilter = "MP4/WEBM Files|*.mp4;*.webm|All files (*.*)|*.*";

        private VideoItemViewModel item;

        public VideoItemEditorControl(VideoItemViewModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }

        private void SourcePathBrowseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = VideoFileBrowserFilter;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            if (fileDialog.ShowDialog() == true)
            {
                this.SourcePathTextBox.Text = fileDialog.FileName;
            }
        }
    }
}
