using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.Util;
using MixPlayerCreator.WPF.Controls.Items;
using System.Windows;
using System.Windows.Controls;

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
                        this.AddElementToCanvas(new TextItemControl(new TextItemModel("Text")), (int)dropPoint.X, (int)dropPoint.Y);
                        break;
                }
                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }

        private void AddElementToCanvas(FrameworkElement element, int x, int y)
        {
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
            element.Loaded += Element_Loaded;
            this.CanvasRender.Children.Add(element);
        }

        private void Element_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            int x = MathHelper.Clamp((int)Canvas.GetLeft(element) - (int)(element.ActualWidth / 2), 0, (int)this.CanvasRender.ActualWidth);
            int y = MathHelper.Clamp((int)Canvas.GetTop(element) - (int)(element.ActualHeight / 2), 0, (int)this.CanvasRender.ActualHeight);
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
        }
    }
}
