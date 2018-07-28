using System.Runtime.Serialization;

namespace MixPlayCreator.Base.Model
{
    [DataContract]
    public class CDKProjectModel
    {
        [DataMember]
        public string DirectoryPath { get; set; }

        public CDKProjectModel() { }

        public CDKProjectModel(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }
    }
}
