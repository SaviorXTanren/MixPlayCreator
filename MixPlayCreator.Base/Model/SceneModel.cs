using MixPlayCreator.Base.Model.Items;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model
{
    [DataContract]
    public class SceneModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<ItemModel> Items { get; set; }

        public SceneModel()
        {
            this.Items = new List<ItemModel>();
        }

        public SceneModel(string name)
            : this()
        {
            this.Name = name;
        }
    }
}
