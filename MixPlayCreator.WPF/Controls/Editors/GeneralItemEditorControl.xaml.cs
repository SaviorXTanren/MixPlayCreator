using MixPlayCreator.Base.Model.Interactive;
using MixPlayCreator.Base.ViewModel.Items;
using System.Windows.Controls;

namespace MixPlayCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for GeneralItemEditorControl.xaml
    /// </summary>
    public partial class GeneralItemEditorControl : UserControl
    {
        private ItemViewModel item;

        public GeneralItemEditorControl()
        {
            InitializeComponent();
        }

        public void SetItem(ItemViewModel item)
        {
            this.item = item;
            this.DataContext = this.item;
            this.VisibleCheckBox.IsChecked = this.item.IsVisible;
            this.InteractiveButtonCheckBox.IsChecked = this.item.IsInteractive;
        }

        private void VisibleCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.item.IsVisible = this.VisibleCheckBox.IsChecked.GetValueOrDefault();
        }

        private void InteractiveButtonCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.InteractiveButtonCheckBox.IsChecked.GetValueOrDefault())
            {
                this.item.Interactive = new InteractiveButtonModel(this.item.Name);
                this.SparkCostTextBox.IsEnabled = true;
            }
            else
            {
                this.item.Interactive = null;
                this.SparkCostTextBox.IsEnabled = false;
            }
        }
    }
}
