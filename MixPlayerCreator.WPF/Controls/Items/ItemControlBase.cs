using MixPlayCreator.Base.Model.Items;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlayerCreator.WPF.Controls.Items
{
    public class ItemControlBase : UserControl
    {
        public static event EventHandler<ItemTypeModel> ItemSelected = delegate { };

        public ItemTypeModel Item { get; private set; }

        public ItemControlBase(ItemTypeModel item)
        {
            this.Item = item;

            this.Loaded += ItemControlBase_Loaded;
        }

        protected virtual Task OnLoaded() { return Task.FromResult(0); }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ItemControlBase.ItemSelected(this, this.Item);
            }
        }

        private async void ItemControlBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.OnLoaded();
        }
    }
}
