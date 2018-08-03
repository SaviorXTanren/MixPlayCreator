using Mixer.Base.Util;
using Newtonsoft.Json;
using System.ComponentModel;
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
        public int ZIndex { get; set; }

        public ItemModel() { }

        public ItemModel(ItemTypeEnum type)
        {
            this.Type = type;
        }

        [JsonIgnore]
        public string TypeString { get { return EnumHelper.GetEnumName(this.Type); } }
    }
}
