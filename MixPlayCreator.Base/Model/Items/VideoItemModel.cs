using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    [DataContract]
    public class VideoItemModel : ItemModel
    {
        [DataMember]
        public string SourcePath { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        public VideoItemModel() : this("Video", string.Empty, 50, 50) { }

        public VideoItemModel(string name, string sourcePath, int width, int height)
            : base(name, ItemTypeEnum.Video)
        {
            this.SourcePath = sourcePath;
            this.Width = width;
            this.Height = height;
            this.IsVisible = false;
        }
    }
}
