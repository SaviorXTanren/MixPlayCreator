using MixPlayCreator.Base.Model.Items;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlayerCreator.WPF.Controls.Items
{
    /// <summary>
    /// Interaction logic for ItemTypeControl.xaml
    /// </summary>
    public partial class ItemTypeControl : UserControl
    {
        private ItemModel itemType;

        public ItemTypeControl(ItemModel itemType)
        {
            this.itemType = itemType;

            InitializeComponent();

            this.DataContext = this.itemType;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            DataObject data = new DataObject();
            data.SetData("ItemType", this.itemType.Type);

            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        }
    }
}
