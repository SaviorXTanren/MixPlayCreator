using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    [DataContract]
    public class SoundItemModel : ItemModel
    {
        [DataMember]
        public string SourcePath { get; set; }

        public SoundItemModel() : this("Sound", null) { }

        public SoundItemModel(string name, string sourcePath)
            : base(name, ItemTypeEnum.Sound)
        {
            this.SourcePath = sourcePath;
        }
    }
}
