using Mixer.Base.Util;
using MixPlayCreator.Base.Model.Interactive;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    public enum ItemTypeEnum
    {
        Text = 0,
        Picture = 1,
    }

    [DataContract]
    public class ItemModel
    {
        [DataMember]
        public ItemTypeEnum Type { get; set; }

        [DataMember]
        public bool IsVisible { get; set; } = true;

        [DataMember]
        public int ZIndex { get; set; }

        [DataMember]
        public InteractiveModelBase Interactive { get; set; }

        public ItemModel() { }

        public ItemModel(ItemTypeEnum type)
        {
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
                    case ItemTypeEnum.Text: return "/Assets/Text.png";
                    case ItemTypeEnum.Picture: return "/Assets/Image.png";
                }
                return null;
            }
        }
    }
}
