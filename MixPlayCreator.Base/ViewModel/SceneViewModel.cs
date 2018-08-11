using Mixer.Base.Model.Interactive;
using MixPlayCreator.Base.Model;
using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.ViewModel.Items;
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
                    case ItemTypeEnum.Sound:
                        this.Items.Add(new SoundItemViewModel((SoundItemModel)item));
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

        public InteractiveSceneModel GetSceneData()
        {
            InteractiveSceneModel scene = new InteractiveSceneModel()
            {
                sceneID = this.Name
            };

            foreach (ItemViewModel item in this.Items)
            {
                if (item.Interactive != null)
                {
                    item.Interactive.ID = item.Name;
                    scene.buttons.Add(item.Interactive.GetInteractiveControl());
                }
            }
            return scene;
        }
    }
}
