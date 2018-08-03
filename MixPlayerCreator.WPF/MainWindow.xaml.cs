﻿using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.ViewModel.Items;
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
            this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemModel(ItemTypeEnum.Text)));
            this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemModel(ItemTypeEnum.Picture)));

            ItemViewModel.ItemSelectionChanged += ItemControlBase_ItemSelectionChanged;
        }

        private void ItemControlBase_ItemSelectionChanged(object sender, ItemViewModel e)
        {
            if (e != null)
            {
                switch (e.Type)
                {
                    case ItemTypeEnum.Text:
                        this.ItemEditorContentControl.Content = new TextItemEditorControl((TextItemViewModel)e);
                        break;
                    case ItemTypeEnum.Picture:
                        this.ItemEditorContentControl.Content = new PictureItemEditorControl((PictureItemViewModel)e);
                        break;
                }
            }
            else
            {
                this.ItemEditorContentControl.Content = null;
            }
        }
    }
}
