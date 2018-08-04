using Mixer.Base.Model.Interactive;
using Newtonsoft.Json.Linq;

namespace MixPlayCreator.Base.Model.Interactive
{
    public enum InteractiveTypeEnum
    {
        Button,
        TextBox,
        Joystick,
    }

    public abstract class InteractiveModelBase
    {
        public string ID { get; set; }

        public InteractiveTypeEnum Type { get; set; }

        public InteractiveModelBase() { }

        public InteractiveModelBase(string id, InteractiveTypeEnum type)
        {
            this.ID = id;
            this.Type = type;
        }

        public abstract JObject GetInteractiveJObject();
    }
}
