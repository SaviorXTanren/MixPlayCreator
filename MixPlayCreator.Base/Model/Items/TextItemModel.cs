using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Items
{
    [DataContract]
    public class TextItemModel : ItemTypeModel
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int Size { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string Font { get; set; }

        public TextItemModel() { }

        public TextItemModel(string text) : this(text, 24, "Black", "Arial") { }

        public TextItemModel(string text, int size, string color, string font)
            : base(ItemTypeEnum.Text)
        {
            this.Text = text;
            this.Size = size;
            this.Color = color;
            this.Font = font;
        }
    }
}
