using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    [DataContract]
    public class ImageItemModel : ItemModel
    {
        [DataMember]
        public string SourcePath { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        public ImageItemModel() : this("Image", string.Empty, 50, 50) { }

        public ImageItemModel(string name, string sourcePath, int width, int height)
            : base(name, ItemTypeEnum.Image)
        {
            this.SourcePath = sourcePath;
            this.Width = width;
            this.Height = height;
        }
    }
}
