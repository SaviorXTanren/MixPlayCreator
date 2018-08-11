using Mixer.Base;
using Mixer.Base.Model.Interactive;
using MixPlayCreator.Base.Model;
using MixPlayCreator.Base.Util;
using MixPlayCreator.Base.ViewModel.Items;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MixPlayCreator.Base.ViewModel
{
    public class CDKProjectViewModel
    {
        public const string MixPlayCreatorSettingsFileBrowserFilter = "MixPlay Creator Settings|*.mixplay|All files (*.*)|*.*";

        public static string ApplicationDirectory { get { return Path.GetDirectoryName(typeof(CDKProjectViewModel).Assembly.Location); } }
        public static string TemplateIndexHTMLFilePath { get { return Path.Combine(CDKProjectViewModel.ApplicationDirectory, "Assets", "index.html"); } }
        public static string TemplateScriptJSFilePath { get { return Path.Combine(CDKProjectViewModel.ApplicationDirectory, "Assets", "scripts.js"); } }

        public CDKProjectModel Model { get; set; }

        public CDKProjectViewModel(string directoryPath)
            : this(new CDKProjectModel(directoryPath), null)
        {
            this.Scenes.Add(new SceneViewModel("default"));
        }

        public CDKProjectViewModel(CDKProjectModel model, string settingsFilePath)
        {
            this.Model = model;
            this.Scenes = new List<SceneViewModel>();
            foreach (SceneModel scene in this.Model.Scenes)
            {
                this.Scenes.Add(new SceneViewModel(scene));
            }
            this.SettingsFilePath = settingsFilePath;
        }

        public List<SceneViewModel> Scenes { get; set; }
        public string SettingsFilePath { get; set; }

        public string SourceFolderPath { get { return string.Format("{0}\\src", this.Model.DirectoryPath); } }
        public string IndexHTMLFilePath { get { return string.Format("{0}\\index.html", this.SourceFolderPath); } }
        public string ScriptJSFilePath { get { return string.Format("{0}\\scripts.js", this.SourceFolderPath); } }

        public string LinkedInteractiveGameJSONFilePath { get { return string.Format("{0}\\.cdk\\linkedInteractiveGame.json", this.Model.DirectoryPath); } }
        public string WorldSchemaFilePath { get { return string.Format("{0}\\.cdk\\worldSchema.json", this.Model.DirectoryPath); } }

        public string DefaultSettingFileName { get { return new DirectoryInfo(this.Model.DirectoryPath).Name + ".mixplay"; } }

        public async Task Save()
        {
            await this.SaveSettings();
            await this.SaveWorldSchema();
            await this.SaveHTMLFiles();
        }

        public async Task UploadLinkedInteractiveGame(MixerConnection connection)
        {
            InteractiveGameListingModel linkedGame = null;
            using (StreamReader reader = new StreamReader(File.OpenRead(this.LinkedInteractiveGameJSONFilePath)))
            {
                string fileContents = await reader.ReadToEndAsync();
                linkedGame = SerializerHelper.DeserializeObjectFromString<InteractiveGameListingModel>(fileContents);
            }

            InteractiveGameVersionModel version = linkedGame.versions[0];

            version = await connection.Interactive.GetInteractiveGameVersion(version);
            version.controls.scenes = new List<InteractiveSceneModel>();
            foreach (SceneViewModel scene in this.Scenes)
            {
                version.controls.scenes.Add(scene.GetSceneData());
            }

            version = await connection.Interactive.UpdateInteractiveGameVersion(version);
        }

        private async Task SaveSettings()
        {
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

            using (StreamWriter writer = new StreamWriter(File.Open(this.SettingsFilePath, FileMode.Create)))
            {
                await writer.WriteAsync(SerializerHelper.SerializeObjectToString(this.Model));
            }
        }

        private async Task SaveWorldSchema()
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
                            List<InteractiveSceneModel> sceneData = new List<InteractiveSceneModel>();
                            foreach (SceneViewModel scene in this.Scenes)
                            {
                                sceneData.Add(scene.GetSceneData());
                            }
                            schema["world"]["scenes"] = JArray.FromObject(sceneData);
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(File.Open(this.WorldSchemaFilePath, FileMode.Create)))
                    {
                        await writer.WriteAsync(SerializerHelper.SerializeObjectToString(schemas));
                    }
                }
            }
        }

        private async Task SaveHTMLFiles()
        {
            string htmlFileContents = string.Empty;
            using (StreamReader reader = new StreamReader(File.OpenRead(CDKProjectViewModel.TemplateIndexHTMLFilePath)))
            {
                htmlFileContents = await reader.ReadToEndAsync();
            }

            using (StreamWriter writer = new StreamWriter(File.Open(this.IndexHTMLFilePath, FileMode.Create)))
            {
                await writer.WriteAsync(htmlFileContents);
            }

            string scriptFileContents = string.Empty;
            using (StreamReader reader = new StreamReader(File.OpenRead(CDKProjectViewModel.TemplateScriptJSFilePath)))
            {
                scriptFileContents = await reader.ReadToEndAsync();
            }

            StringBuilder definedItems = new StringBuilder();
            definedItems.AppendLine();
            definedItems.AppendLine();
            definedItems.AppendLine("function initializeItems()");
            definedItems.AppendLine("{");
            foreach (SceneViewModel scene in this.Scenes)
            {
                foreach (ItemViewModel item in scene.Items)
                {
                    definedItems.Append("\t");
                    if (item is TextItemViewModel)
                    {
                        TextItemViewModel textItem = (TextItemViewModel)item;
                        definedItems.Append("addText(\"" + item.Name + "\", \"" + scene.Name + "\", \"" + textItem.Text + "\", " + textItem.Size + ", \"" + textItem.Color.ToLower() + "\", \"" + textItem.Font + "\", " + item.LeftPosition + ", " + item.TopPosition + ", " + item.IsVisible.ToString().ToLower() + ", " + item.IsInteractive.ToString().ToLower() + ");");
                    }
                    else if (item is ImageItemViewModel)
                    {
                        ImageItemViewModel imageItem = (ImageItemViewModel)item;
                        definedItems.Append("addImage(\"" + item.Name + "\", \"" + scene.Name + "\", \"" + Path.GetFileName(imageItem.SourcePath) + "\", " + imageItem.Width + ", " + imageItem.Height + ", " + item.LeftPosition + ", " + item.TopPosition + ", " + item.IsVisible.ToString().ToLower() + ", " + item.IsInteractive.ToString().ToLower() + ");");
                        if (File.Exists(imageItem.SourcePath))
                        {
                            File.Copy(imageItem.SourcePath, Path.Combine(this.SourceFolderPath, Path.GetFileName(imageItem.SourcePath)), overwrite: true);
                        }
                    }
                    else if (item is SoundItemViewModel)
                    {
                        SoundItemViewModel soundItem = (SoundItemViewModel)item;
                        if (File.Exists(soundItem.SourcePath))
                        {
                            File.Copy(soundItem.SourcePath, Path.Combine(this.SourceFolderPath, Path.GetFileName(soundItem.SourcePath)), overwrite: true);
                        }
                    }
                    definedItems.AppendLine();
                }
            }
            definedItems.AppendLine("}");

            using (StreamWriter writer = new StreamWriter(File.Open(this.ScriptJSFilePath, FileMode.Create)))
            {
                await writer.WriteAsync(scriptFileContents);
                await writer.WriteAsync(definedItems.ToString());
            }
        }
    }
}
