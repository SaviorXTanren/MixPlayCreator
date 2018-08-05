using MixPlayCreator.Base.Model;
using MixPlayCreator.Base.Util;
using MixPlayCreator.Base.ViewModel.Items;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MixPlayCreator.Base.ViewModel
{
    public class CDKProjectViewModel
    {
        public static string ApplicationDirectory { get { return Path.GetDirectoryName(typeof(CDKProjectViewModel).Assembly.Location); } }

        public CDKProjectModel Model { get; set; }

        public CDKProjectViewModel(string directoryPath)
            : this(new CDKProjectModel(directoryPath))
        {
            this.Scenes.Add(new SceneViewModel("default"));
        }

        public CDKProjectViewModel(CDKProjectModel model)
        {
            this.Model = model;
            this.Scenes = new List<SceneViewModel>();
            foreach (SceneModel scene in this.Model.Scenes)
            {
                this.Scenes.Add(new SceneViewModel(scene));
            }
        }

        public List<SceneViewModel> Scenes { get; set; }


        public string IndexHTMLFilePath { get { return string.Format("{0}\\src\\index.html", this.Model.DirectoryPath); } }

        public string ScriptJSFilePath { get { return string.Format("{0}\\src\\script.js", this.Model.DirectoryPath); } }


        public string LinkedInteractiveGameJSONFilePath { get { return string.Format("{0}\\.cdk\\linkedInteractiveGame.json", this.Model.DirectoryPath); } }

        public string WorldSchemaFilePath { get { return string.Format("{0}\\.cdk\\worldSchema.json", this.Model.DirectoryPath); } }


        public string SettingsDirectory { get { return Path.Combine(ApplicationDirectory, "Settings"); } }


        public async Task Save()
        {
            await this.SaveToSettings();
            await this.SaveToCDKProject();
        }

        private async Task SaveToSettings()
        {
            if (!Directory.Exists(this.SettingsDirectory))
            {
                Directory.CreateDirectory(this.SettingsDirectory);
            }

            this.Model.Scenes.Clear();
            foreach (SceneViewModel scene in this.Scenes)
            {
                this.Model.Scenes.Add(scene.Model);
                scene.Model.Items.Clear();
                foreach (ItemViewModel item in scene.Items)
                {
                    scene.Model.Items.Add(item.Model);
                }
            }

            string settingsFilePath = Path.Combine(this.SettingsDirectory, new DirectoryInfo(this.Model.DirectoryPath).Name + ".json");
            using (StreamWriter writer = new StreamWriter(File.Open(settingsFilePath, FileMode.Create)))
            {
                await writer.WriteAsync(SerializerHelper.SerializeObjectToString(this.Model));
            }
        }

        private async Task SaveToCDKProject()
        {
            if (File.Exists(this.WorldSchemaFilePath))
            {
                string fileContents = null;
                using (StreamReader reader = new StreamReader(File.OpenRead(this.WorldSchemaFilePath)))
                {
                    fileContents = await reader.ReadToEndAsync();
                }

                if (!string.IsNullOrEmpty(fileContents))
                {
                    JArray schemas = SerializerHelper.DeserializeObjectFromString<JArray>(fileContents);
                    foreach (JToken schema in schemas)
                    {
                        if (schema["name"] != null && schema["name"].ToString().Equals("$working"))
                        {
                            JArray sceneData = new JArray();
                            foreach (SceneViewModel scene in this.Scenes)
                            {
                                sceneData.Add(scene.GetSceneData());
                            }
                            schema["world"]["scenes"] = sceneData;
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(File.Open(this.WorldSchemaFilePath, FileMode.Create)))
                    {
                        await writer.WriteAsync(SerializerHelper.SerializeObjectToString(schemas));
                    }
                }
            }
        }
    }
}
