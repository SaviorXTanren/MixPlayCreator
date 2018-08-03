using Microsoft.Win32;
using MixPlayCreator.Base.ViewModel.Items;
using System.Windows.Controls;

namespace MixPlayerCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for PictureItemEditorControl.xaml
    /// </summary>
    public partial class PictureItemEditorControl : UserControl
    {
        private const string PictureFileBrowserFilter = "All Picture Files|*.bmp;*.gif;*.jpg;*.jpeg;*.png;|All files (*.*)|*.*";

        private PictureItemViewModel item;

        public PictureItemEditorControl(PictureItemViewModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
            this.VisibleCheckBox.IsChecked = this.item.IsVisible;
        }

        private void SourcePathBrowseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = PictureFileBrowserFilter;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            if (fileDialog.ShowDialog() == true)
            {
                this.SourcePathTextBox.Text = fileDialog.FileName;
            }
        }

        private void VisibleCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.item.IsVisible = this.VisibleCheckBox.IsChecked.GetValueOrDefault();
        }
    }
}
