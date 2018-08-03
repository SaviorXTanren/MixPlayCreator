﻿using MixPlayCreator.Base.Model.Items;
using System;
using System.IO;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class PictureItemViewModel : ItemViewModel
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

        public new PictureItemModel Model { get; private set; }

        public PictureItemViewModel() : this(new PictureItemModel()) { }

        public PictureItemViewModel(PictureItemModel model) : base(model) { this.Model = model; }

        public Uri SourceUri { get { return new Uri(this.SourcePath, UriKind.RelativeOrAbsolute); } }

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
