using MixPlayCreator.Base.Model;
using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.ViewModel.Items;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MixPlayCreator.Base.ViewModel
{
    public class SceneViewModel
    {
        public SceneModel Model { get; private set; }

        public SceneViewModel(string name) : this(new SceneModel(name)) { }

        public SceneViewModel(SceneModel model)
        {
            this.Model = model;
            this.Items = new List<ItemViewModel>();
            foreach (ItemModel item in this.Model.Items)
            {
                switch (item.Type)
                {
                    case ItemTypeEnum.Text:
                        this.Items.Add(new TextItemViewModel((TextItemModel)item));
                        break;
                    case ItemTypeEnum.Image:
                        this.Items.Add(new ImageItemViewModel((ImageItemModel)item));
                        break;
                }
            }
        }

        public List<ItemViewModel> Items { get; set; }

        public string Name
        {
            get { return this.Model.Name; }
            set { this.Model.Name = value; }
        }

        public JObject GetSceneData()
        {
            JObject data = new JObject();
            data["sceneID"] = this.Name;

            JArray itemData = new JArray();
            data["controls"] = itemData;
            foreach (ItemViewModel item in this.Items)
            {
                if (item.Interactive != null)
                {
                    item.Interactive.ID = item.Name;
                    itemData.Add(item.Interactive.GetInteractiveJObject());
                }
            }
            return data;
        }
    }
}
