﻿using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.Util;
using MixPlayCreator.Base.ViewModel.Items;
using MixPlayerCreator.WPF.Controls.Items;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlayerCreator.WPF.Controls
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

        public void SetItemCoordinates(FrameworkElement item, int x, int y)
        {
            x = MathHelper.Clamp(x, 0, (int)(this.CanvasRender.ActualWidth - item.ActualWidth));
            y = MathHelper.Clamp(y, 0, (int)(this.CanvasRender.ActualHeight - item.ActualHeight));
            Canvas.SetLeft(item, x);
            Canvas.SetTop(item, y);
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
                        this.AddItemToCanvas(new TextItemControl(new TextItemViewModel()), (int)dropPoint.X, (int)dropPoint.Y);
                        break;
                    case ItemTypeEnum.Picture:
                        this.AddItemToCanvas(new PictureItemControl(new PictureItemViewModel()), (int)dropPoint.X, (int)dropPoint.Y);
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

        private void AddItemToCanvas(ItemControlBase item, int x, int y)
        {
            item.ItemCanvas = this;
            Canvas.SetLeft(item, x);
            Canvas.SetTop(item, y);
            item.Loaded += Item_Loaded;
            this.CanvasRender.Children.Add(item);
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {
            ItemControlBase item = (ItemControlBase)sender;
            this.SetItemCoordinates(item, (int)Canvas.GetLeft(item) - (int)(item.ActualWidth / 2), (int)Canvas.GetTop(item) - (int)(item.ActualHeight / 2));
            Canvas.SetZIndex(item, item.Item.ZIndex);

            ItemViewModel.ItemSelected(item.Item);
            item.Item.IsSelected = true;
        }
    }
}