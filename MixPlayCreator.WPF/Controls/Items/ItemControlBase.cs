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

        public double CenterXPosition { get { return (Canvas.GetLeft(this) + (this.ActualWidth / 2.0)); } }
        public double CenterYPosition { get { return (Canvas.GetTop(this) + (this.ActualHeight / 2.0)); } }

        public void SetItemZIndex()
        {
            Canvas.SetZIndex(this, this.Item.ZIndex);
        }

        public void UpdateModelPosition()
        {
            Tuple<double, double> canvasSize = this.ItemCanvas.GetCanvasSize();
            this.Item.XPosition = ((this.CenterXPosition / canvasSize.Item1) * 100.0);
            this.Item.YPosition = ((this.CenterYPosition / canvasSize.Item2) * 100.0);
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
                double xDiff = currentPosition.X - lastMousePosition.X;
                double yDiff = currentPosition.Y - lastMousePosition.Y;

                this.ItemCanvas.SetItemCoordinates(this, Canvas.GetLeft(this) + xDiff, Canvas.GetTop(this) + yDiff);

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
                this.SetItemZIndex();
            }
        }
    }
}
