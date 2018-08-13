using MixPlayCreator.Base.Model.Items;
using System;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class VideoItemViewModel : ItemViewModel
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

        public int Width
        {
            get { return this.Model.Width; }
            set
            {
                this.Model.Width = value;
                this.NotifyPropertyChanged("Width");
            }
        }

        public int Height
        {
            get { return this.Model.Height; }
            set
            {
                this.Model.Height = value;
                this.NotifyPropertyChanged("Height");
            }
        }

        public new VideoItemModel Model { get; private set; }

        public VideoItemViewModel() : this(new VideoItemModel()) { }

        public VideoItemViewModel(VideoItemModel model) : base(model) { this.Model = model; }
    }
}
