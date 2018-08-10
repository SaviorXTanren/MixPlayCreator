using MixPlayCreator.Base.ViewModel.Items;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MixPlayCreator.WPF.Controls.Items
{
    public class ItemControlBase : UserControl
    {
        public ItemViewModel Item { get; private set; }
        public ItemsCanvasControl ItemCanvas { get; set; }

        public bool IsItemHeld { get; private set; }

        private Point lastMousePosition;

        public ItemControlBase(ItemViewModel item)
        {
            this.Item = item;

            this.Loaded += ItemControlBase_Loaded;
            this.MouseMove += ItemControlBase_MouseMove;

            ItemViewModel.ItemSelectionChanged += ItemControlBase_ItemSelectionChanged;
            ItemViewModel.ItemDeletionOccurred += ItemViewModel_ItemDeletionOccurred;
            ItemViewModel.ItemZIndexChangeOccurred += ItemViewModel_ItemZIndexChangeOccurred;
        }

        public void UpdateModelPosition()
        {
            Tuple<int, int> position = this.ItemCanvas.GetItemCoordinates(this);
            Tuple<int, int> canvasSize = this.ItemCanvas.GetCanvasSize();
            this.Item.SetCanvasLeftPosition(position.Item1, canvasSize.Item1);
            this.Item.SetCanvasTopPosition(position.Item2, canvasSize.Item2);
        }

        protected virtual Task OnLoaded() { return Task.FromResult(0); }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            ItemViewModel.ItemSelected(this.Item);

            this.IsItemHeld = true;
            this.Item.IsSelected = true;
            this.lastMousePosition = e.GetPosition(this.Parent as UIElement);
            this.CaptureMouse();

            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            this.IsItemHeld = false;
            this.lastMousePosition = new Point();
            this.ReleaseMouseCapture();
        }

        private void ItemControlBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsItemHeld)
            {
                Point currentPosition = e.GetPosition(this.Parent as UIElement);
                var transform = this.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    this.RenderTransform = transform;
                }
                int xDiff = (int)(currentPosition.X - lastMousePosition.X);
                int yDiff = (int)(currentPosition.Y - lastMousePosition.Y);

                this.ItemCanvas.SetItemCoordinates(this, (int)Canvas.GetLeft(this) + xDiff, (int)Canvas.GetTop(this) + yDiff);

                this.UpdateModelPosition();

                this.lastMousePosition = currentPosition;
            }
        }

        private async void ItemControlBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.OnLoaded();
        }

        private void ItemControlBase_ItemSelectionChanged(object sender, ItemViewModel item)
        {
            this.Item.IsSelected = (this.Item.Equals(item));
        }

        private void ItemViewModel_ItemDeletionOccurred(object sender, ItemViewModel item)
        {
            if (item != null && this.Item.Equals(item))
            {
                this.ItemCanvas.RemoveSelectedItem(this);
            }
        }

        private void ItemViewModel_ItemZIndexChangeOccurred(object sender, ItemViewModel item)
        {
            if (item != null && this.Item.Equals(item))
            {
                this.ItemCanvas.SetItemZIndex(this);
            }
        }
    }
}
