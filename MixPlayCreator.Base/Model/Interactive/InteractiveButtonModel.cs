using Mixer.Base.Model.Interactive;
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

        public override InteractiveButtonControlModel GetInteractiveControl()
        {
            return new InteractiveButtonControlModel()
            {
                controlID = this.ID,
                text = this.ID,
                cost = this.SparkCost,
                disabled = false,
            };
        }
    }
}
