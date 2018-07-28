using Mixer.Base.Util;
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
    public class ItemTypeModel
    {
        [DataMember]
        public ItemTypeEnum Type { get; set; }

        public ItemTypeModel() { }

        public ItemTypeModel(ItemTypeEnum type)
        {
            this.Type = type;
        }

        [JsonIgnore]
        public string TypeString { get { return EnumHelper.GetEnumName(this.Type); } }
    }
}
