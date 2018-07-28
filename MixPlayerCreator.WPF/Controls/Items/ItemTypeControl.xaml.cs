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
        private ItemTypeModel itemType;

        public ItemTypeControl(ItemTypeModel itemType)
        {
            this.itemType = itemType;

            InitializeComponent();

            this.DataContext = this.itemType;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("ItemType", this.itemType.Type);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }
    }
}
