using Mixer.Base.Util;
using MixPlayCreator.Base.Model.Interactive;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    public enum ItemTypeEnum
    {
        Text = 0,
        Image = 1,
        Sound = 2,
        Video = 3,
    }

    [DataContract]
    public class ItemModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ItemTypeEnum Type { get; set; }

        [DataMember]
        public bool IsVisible { get; set; } = true;

        [DataMember]
        public double XPosition { get; set; }

        [DataMember]
        public double YPosition { get; set; }

        [DataMember]
        public int ZIndex { get; set; }

        [DataMember]
        public InteractiveModelBase Interactive { get; set; }

        public ItemModel() { }

        public ItemModel(string name, ItemTypeEnum type)
        {
            this.Name = name;
            this.Type = type;
        }

        [JsonIgnore]
        public string TypeString { get { return EnumHelper.GetEnumName(this.Type); } }

        [JsonIgnore]
        public string TypeImageSourcePath
        {
            get
            {
                switch (this.Type)
                {
                    case ItemTypeEnum.Text: return "/Assets/TextItem.png";
                    case ItemTypeEnum.Image: return "/Assets/ImageItem.png";
                    case ItemTypeEnum.Sound: return "/Assets/SoundItem.png";
                    case ItemTypeEnum.Video: return "/Assets/VideoItem.png";
                }
                return null;
            }
        }
    }
}
