using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    [DataContract]
    public class TextItemModel : ItemTypeModel
    {
        [DataMember]
        public string Text { get; set; }

        public TextItemModel() { }

        public TextItemModel(string text) : base(ItemTypeEnum.Text) { this.Text = text; }
    }
}
