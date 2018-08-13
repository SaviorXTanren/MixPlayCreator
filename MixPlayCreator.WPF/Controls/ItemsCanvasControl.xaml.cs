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

        public Tuple<double, double> GetCanvasSize() { return new Tuple<double, double>(this.CanvasRender.ActualWidth, this.CanvasRender.ActualHeight); }

        public void RefreshChildren()
        {
            this.CanvasRender.Children.Clear();
            Tuple<double, double> canvasSize = this.GetCanvasSize();
            foreach (ItemViewModel item in App.CurrentScene.Items)
            {
                this.AddItemToCanvas(item, ((0.01 * item.XPosition) * canvasSize.Item1), ((0.01 * item.YPosition) * canvasSize.Item2));
            }
        }

        public void SetItemCoordinates(FrameworkElement item, double x, double y)
        {
            x = MathHelper.Clamp(x, 0.0, this.CanvasRender.ActualWidth - item.ActualWidth);
            y = MathHelper.Clamp(y, 0.0, this.CanvasRender.ActualHeight - item.ActualHeight);
            Canvas.SetLeft(item, x);
            Canvas.SetTop(item, y);
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
                        this.AddNewItemToCanvas(new TextItemViewModel(), dropPoint.X, dropPoint.Y);
                        break;
                    case ItemTypeEnum.Image:
                        this.AddNewItemToCanvas(new ImageItemViewModel(), dropPoint.X, dropPoint.Y);
                        break;
                    case ItemTypeEnum.Sound:
                        this.AddNewItem(new SoundItemViewModel());
                        break;
                    case ItemTypeEnum.Video:
                        this.AddNewItemToCanvas(new VideoItemViewModel(), dropPoint.X, dropPoint.Y);
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

        private void AddNewItemToCanvas(ItemViewModel item, double x, double y)
        {
            App.CurrentScene.Items.Add(item);

            item.ZIndex = App.CurrentScene.Items.Count;

            this.AddItemToCanvas(item, x, y, isNewItem: true);
        }

        private void AddItemToCanvas(ItemViewModel item, double x, double y, bool isNewItem = false)
        {
            switch (item.Type)
            {
                case ItemTypeEnum.Text:
                    this.AddItemControlToCanvas(new TextItemControl((TextItemViewModel)item), x, y, isNewItem);
                    break;
                case ItemTypeEnum.Image:
                    this.AddItemControlToCanvas(new ImageItemControl((ImageItemViewModel)item), x, y, isNewItem);
                    break;
                case ItemTypeEnum.Video:
                    this.AddItemControlToCanvas(new VideoItemControl((VideoItemViewModel)item), x, y, isNewItem);
                    break;
            }
        }

        private void AddItemControlToCanvas(ItemControlBase item, double x, double y, bool isNewItem = false)
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

            this.SetItemCoordinates(item, Canvas.GetLeft(item) - (item.ActualWidth / 2.0), Canvas.GetTop(item) - (item.ActualHeight / 2.0));

            item.UpdateModelPosition();
            
            item.SetItemZIndex();
        }

        private void Item_NewLoaded(object sender, RoutedEventArgs e)
        {
            ItemControlBase item = (ItemControlBase)sender;
            ItemViewModel.ItemAdded(item.Item);
        }
    }
}
