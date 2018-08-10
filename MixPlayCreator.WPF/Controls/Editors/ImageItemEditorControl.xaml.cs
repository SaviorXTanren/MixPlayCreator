using Microsoft.Win32;
using MixPlayCreator.Base.ViewModel.Items;
using System.Windows.Controls;

namespace MixPlayCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for ImageItemEditorControl.xaml
    /// </summary>
    public partial class ImageItemEditorControl : UserControl
    {
        private const string ImageFileBrowserFilter = "All Image Files|*.bmp;*.gif;*.jpg;*.jpeg;*.png;|All files (*.*)|*.*";

        private ImageItemViewModel item;

        public ImageItemEditorControl(ImageItemViewModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
            this.GeneralItemEditor.SetItem(this.item);
        }

        private void SourcePathBrowseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = ImageFileBrowserFilter;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            if (fileDialog.ShowDialog() == true)
            {
                this.SourcePathTextBox.Text = fileDialog.FileName;
            }
        }
    }
}
