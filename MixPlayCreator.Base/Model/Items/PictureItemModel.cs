using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    [DataContract]
    public class PictureItemModel : ItemModel
    {
        [DataMember]
        public string SourcePath { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        public PictureItemModel() : this("/Assets/MixItUpLogo.png", 50, 50) { }

        public PictureItemModel(string sourcePath, int width, int height)
            : base(ItemTypeEnum.Picture)
        {
            this.SourcePath = sourcePath;
            this.Width = width;
            this.Height = height;
        }
    }
}
