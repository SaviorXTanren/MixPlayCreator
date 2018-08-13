using MixPlayCreator.Base.ViewModel.Items;

namespace MixPlayCreator.WPF.Controls.Items
{
    /// <summary>
    /// Interaction logic for VideoItemControl.xaml
    /// </summary>
    public partial class VideoItemControl : ItemControlBase
    {
        private VideoItemViewModel item;

        public VideoItemControl(VideoItemViewModel item)
            : base(item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
        }
    }
}
