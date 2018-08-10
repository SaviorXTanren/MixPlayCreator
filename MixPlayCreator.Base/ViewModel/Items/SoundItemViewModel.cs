using MixPlayCreator.Base.Model.Items;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class SoundItemViewModel : ItemViewModel
    {
        public string SourcePath
        {
            get { return this.Model.SourcePath; }
            set
            {
                this.Model.SourcePath = value;
                this.NotifyPropertyChanged("SourcePath");
            }
        }

        public new SoundItemModel Model { get; private set; }

        public SoundItemViewModel() : this(new SoundItemModel()) { }

        public SoundItemViewModel(SoundItemModel model) : base(model) { this.Model = model; }
    }
}
