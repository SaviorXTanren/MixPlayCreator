using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.Util;
using MixPlayCreator.Base.ViewModel.Items;
using MixPlayCreator.WPF.Controls.Items;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlayCreator.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ItemsCanvasControl.xaml
    /// </summary>
    public partial class ItemsCanvasControl : UserControl
    {
        public ItemsCanvasControl()
        {
            InitializeComponent();
        }

        public Tuple<int, int> GetCanvasSize() { return new Tuple<int, int>((int)this.CanvasRender.ActualWidth, (int)this.CanvasRender.ActualHeight); }

        public void RefreshChildren()
        {
            this.CanvasRender.Children.Clear();
            Tuple<int, int> canvasSize = this.GetCanvasSize();
            foreach (ItemViewModel item in App.CurrentScene.Items)
            {
                this.AddItemToCanvas(item, item.GetCanvasLeftPosition(canvasSize.Item1), item.GetCanvasTopPosition(canvasSize.Item2));
            }
        }

        public void SetItemCoordinates(FrameworkElement item, int x, int y)
        {
            x = MathHelper.Clamp(x, 0, (int)(this.CanvasRender.ActualWidth - item.ActualWidth));
            y = MathHelper.Clamp(y, 0, (int)(this.CanvasRender.ActualHeight - item.ActualHeight));
            Canvas.SetLeft(item, x);
            Canvas.SetTop(item, y);
        }

        public Tuple<int, int> GetItemCoordinates(FrameworkElement item) { return new Tuple<int, int>((int)Canvas.GetLeft(item), (int)Canvas.GetTop(item)); }

        public void SetItemZIndex(ItemControlBase item)
        {
            Canvas.SetZIndex(item, item.Item.ZIndex);
        }

        public void RemoveSelectedItem(FrameworkElement item)
        {
            this.CanvasRender.Children.Remove(item);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent("ItemType"))
            {
                ItemTypeEnum type = (ItemTypeEnum)e.Data.GetData("ItemType");
                Point dropPoint = e.GetPosition(this.CanvasRender);
                switch (type)
                {
                    case ItemTypeEnum.Text:
                        this.AddNewItemToCanvas(new TextItemViewModel(), (int)dropPoint.X, (int)dropPoint.Y);
                        break;
                    case ItemTypeEnum.Image:
                        this.AddNewItemToCanvas(new ImageItemViewModel(), (int)dropPoint.X, (int)dropPoint.Y);
                        break;
                    case ItemTypeEnum.Sound:
                        this.AddNewItem(new SoundItemViewModel());
                        break;
                }
                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            ItemViewModel.ItemSelected(null);

            e.Handled = true;
        }

        private void AddNewItem(ItemViewModel item)
        {
            App.CurrentScene.Items.Add(item);
            ItemViewModel.ItemAdded(item);
        }

        private void AddNewItemToCanvas(ItemViewModel item, int x, int y)
        {
            App.CurrentScene.Items.Add(item);

            item.ZIndex = App.CurrentScene.Items.Count;

            this.AddItemToCanvas(item, x, y, isNewItem: true);
        }

        private void AddItemToCanvas(ItemViewModel item, int x, int y, bool isNewItem = false)
        {
            switch (item.Type)
            {
                case ItemTypeEnum.Text:
                    this.AddItemControlToCanvas(new TextItemControl((TextItemViewModel)item), x, y, isNewItem);
                    break;
                case ItemTypeEnum.Image:
                    this.AddItemControlToCanvas(new ImageItemControl((ImageItemViewModel)item), x, y, isNewItem);
                    break;
            }
        }

        private void AddItemControlToCanvas(ItemControlBase item, int x, int y, bool isNewItem = false)
        {
            item.ItemCanvas = this;
            item.Loaded += Item_Loaded;
            if (isNewItem)
            {
                item.Loaded += Item_NewLoaded;
            }

            Canvas.SetLeft(item, x);
            Canvas.SetTop(item, y);
            this.CanvasRender.Children.Add(item);
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            ItemControlBase item = (ItemControlBase)sender;

            this.SetItemCoordinates(item, (int)Canvas.GetLeft(item) - (int)(item.ActualWidth / 2), (int)Canvas.GetTop(item) - (int)(item.ActualHeight / 2));

            item.UpdateModelPosition();
            
            this.SetItemZIndex(item);
        }

        private void Item_NewLoaded(object sender, RoutedEventArgs e)
        {
            ItemControlBase item = (ItemControlBase)sender;
            ItemViewModel.ItemAdded(item.Item);
        }
    }
}
