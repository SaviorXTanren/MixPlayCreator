using MixPlayCreator.Base.Model.Items;
using System.Windows.Controls;

namespace MixPlayerCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for TextItemEditorControl.xaml
    /// </summary>
    public partial class TextItemEditorControl : UserControl
    {
        private TextItemModel item;

        public TextItemEditorControl(TextItemModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }
    }
}
