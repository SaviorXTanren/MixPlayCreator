using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model
{
    [DataContract]
    public class CDKProjectModel
    {
        [DataMember]
        public string DirectoryPath { get; set; }

        [DataMember]
        public List<SceneModel> Scenes { get; set; }

        public CDKProjectModel()
        {
            this.Scenes = new List<SceneModel>();
        }

        public CDKProjectModel(string directoryPath)
            : this()
        {
            this.DirectoryPath = directoryPath;
        }
    }
}
