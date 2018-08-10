using MixPlayCreator.Base.ViewModel.Items;

namespace MixPlayCreator.WPF.Controls.Items
{
    /// <summary>
    /// Interaction logic for TextItemControl.xaml
    /// </summary>
    public partial class TextItemControl : ItemControlBase
    {
        private TextItemViewModel item;

        public TextItemControl(TextItemViewModel item)
            : base(item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }
    }
}
