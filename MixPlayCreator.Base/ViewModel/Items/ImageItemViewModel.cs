using MixPlayCreator.Base.Model.Items;
using System;
using System.IO;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class ImageItemViewModel : ItemViewModel
    {
        public string SourcePath
        {
            get { return this.Model.SourcePath; }
            set
            {
                this.Model.SourcePath = value;
                this.NotifyPropertyChanged("SourcePath");
                this.NotifyPropertyChanged("SourceUri");
                this.NotifyPropertyChanged("StaticImageVisible");
                this.NotifyPropertyChanged("GifImageVisible");
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

        public new ImageItemModel Model { get; private set; }

        public ImageItemViewModel() : this(new ImageItemModel()) { }

        public ImageItemViewModel(ImageItemModel model) : base(model) { this.Model = model; }

        public Uri SourceUri { get { return new Uri(this.SourcePath, UriKind.RelativeOrAbsolute); } }

        public bool DefaultImageVisible { get { return string.IsNullOrEmpty(this.SourcePath); } }

        public bool StaticImageVisible { get { return !this.GifImageVisible; } }

        public bool GifImageVisible
        {
            get
            {
                string extension = Path.GetExtension(this.SourcePath);
                return (!string.IsNullOrEmpty(extension) && extension.ToLower().Equals(".gif"));
            }
        }
    }
}
