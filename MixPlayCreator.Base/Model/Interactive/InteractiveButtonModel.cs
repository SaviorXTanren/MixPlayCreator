using Mixer.Base.Model.Interactive;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model.Interactive
{
    [DataContract]
    public class InteractiveButtonModel : InteractiveModelBase
    {
        [DataMember]
        public int SparkCost { get; set; }

        public InteractiveButtonModel() { }

        public InteractiveButtonModel(string id) : base(id, InteractiveTypeEnum.Button) { }

        public override JObject GetInteractiveJObject()
        {
            InteractiveButtonControlModel button = new InteractiveButtonControlModel()
            {
                controlID = this.ID,
                text = this.ID,
                cost = this.SparkCost,
                disabled = false,
            };
            return JObject.FromObject(button);
        }
    }
}
