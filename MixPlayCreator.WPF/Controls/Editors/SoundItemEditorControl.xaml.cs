using Microsoft.Win32;
using MixPlayCreator.Base.ViewModel.Items;
using System.Windows.Controls;

namespace MixPlayCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for SoundItemEditorControl.xaml
    /// </summary>
    public partial class SoundItemEditorControl : UserControl
    {
        private const string SoundFileBrowserFilter = "MP3 Files (*.mp3)|*.mp3|All files (*.*)|*.*";

        private SoundItemViewModel item;

        public SoundItemEditorControl(SoundItemViewModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }

        private void SourcePathBrowseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = SoundFileBrowserFilter;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            if (fileDialog.ShowDialog() == true)
            {
                this.SourcePathTextBox.Text = fileDialog.FileName;
            }
        }
    }
}
