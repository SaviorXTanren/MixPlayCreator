using MixPlayCreator.Base.Model;

namespace MixPlayCreator.Base.ViewModel
{
    public class CDKProjectViewModel
    {
        public CDKProjectModel Model { get; set; }

        public CDKProjectViewModel(CDKProjectModel model)
        {
            this.Model = model;
        }

        public string IndexHTMLFilePath { get { return string.Format("{0}\\src\\index.html", this.Model.DirectoryPath); } }

        public string ScriptJSFilePath { get { return string.Format("{0}\\src\\script.js", this.Model.DirectoryPath); } }

        public string LinkedInteractiveGameJSONFilePath { get { return string.Format("{0}\\.cdk\\linkedInteractiveGame.json", this.Model.DirectoryPath); } }

        public string WorldSchemaFilePath { get { return string.Format("{0}\\.cdk\\worldSchema.json", this.Model.DirectoryPath); } }
    }
}
