using MixPlayCreator.Base.Model.Items;
using MixPlayerCreator.WPF.Controls.Editors;
using MixPlayerCreator.WPF.Controls.Items;
using System.Windows;

namespace MixPlayerCreator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemTypeModel(ItemTypeEnum.Text)));
            this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemTypeModel(ItemTypeEnum.Picture)));

            ItemControlBase.ItemSelected += ItemControlBase_ItemSelected;
        }

        private void ItemControlBase_ItemSelected(object sender, ItemTypeModel e)
        {
            switch (e.Type)
            {
                case ItemTypeEnum.Text:
                    this.ItemEditorContentControl.Content = new TextItemEditorControl((TextItemModel)e);
                    break;
                case ItemTypeEnum.Picture:
                    break;
            }
        }
    }
}
