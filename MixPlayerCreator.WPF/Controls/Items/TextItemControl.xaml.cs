using MixPlayCreator.Base.Model.Items;

namespace MixPlayerCreator.WPF.Controls.Items
{
    /// <summary>
    /// Interaction logic for TextItemControl.xaml
    /// </summary>
    public partial class TextItemControl : ItemControlBase
    {
        private TextItemModel item;

        public TextItemControl(TextItemModel item)
            : base(item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }
    }
}
