using MixPlayCreator.Base.ViewModel.Items;

namespace MixPlayCreator.WPF.Controls.Items
{
    /// <summary>
    /// Interaction logic for ImageItemControl.xaml
    /// </summary>
    public partial class ImageItemControl : ItemControlBase
    {
        private ImageItemViewModel item;

        public ImageItemControl(ImageItemViewModel item)
            : base(item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }
    }
}
