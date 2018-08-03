using MixPlayCreator.Base.ViewModel.Items;

namespace MixPlayerCreator.WPF.Controls.Items
{
    /// <summary>
    /// Interaction logic for PictureItemControl.xaml
    /// </summary>
    public partial class PictureItemControl : ItemControlBase
    {
        private PictureItemViewModel item;

        public PictureItemControl(PictureItemViewModel item)
            : base(item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }
    }
}
