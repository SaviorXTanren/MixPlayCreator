using MixPlayCreator.Base.ViewModel.Items;
using System.Windows.Controls;

namespace MixPlayerCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for TextItemEditorControl.xaml
    /// </summary>
    public partial class TextItemEditorControl : UserControl
    {
        private TextItemViewModel item;

        public TextItemEditorControl(TextItemViewModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
            this.GeneralItemEditor.SetItem(item);
        }
    }
}
