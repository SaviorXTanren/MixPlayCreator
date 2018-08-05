using MaterialDesignThemes.Wpf;
using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.ViewModel;
using MixPlayCreator.Base.ViewModel.Items;
using MixPlayerCreator.WPF.Controls;
using MixPlayerCreator.WPF.Controls.Editors;
using MixPlayerCreator.WPF.Controls.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlayerCreator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            this.AllItems.ItemsSource = this.items;

            this.Loaded += MainWindow_Loaded;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && App.Project != null)
            {
                foreach (SceneViewModel scene in App.Project.Scenes)
                {
                    foreach (ItemViewModel item in scene.Items.ToList())
                    {
                        if (item.IsSelected)
                        {
                            scene.Items.Remove(item);
                            ItemViewModel.ItemDeleted(item);
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemModel("Text", ItemTypeEnum.Text)));
            this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemModel("Image", ItemTypeEnum.Image)));

            ItemViewModel.ItemAdditionOccurred += ItemViewModel_ItemAdditionOccurred;
            ItemViewModel.ItemSelectionChanged += ItemControlBase_ItemSelectionChanged;
            ItemViewModel.ItemDeletionOccurred += ItemViewModel_ItemDeletionOccurred;

            App.Project = new CDKProjectViewModel(@"S:\Code\MixPlayCreator\CDKProjectSample");
            App.CurrentScene = App.Project.Scenes.First();

            this.RefreshAllItemsList();
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void SaveProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Project != null)
            {
                HashSet<string> itemIDs = new HashSet<string>();
                foreach (SceneViewModel scene in App.Project.Scenes)
                {
                    foreach (ItemViewModel item in scene.Items)
                    {
                        if (itemIDs.Contains(item.Name))
                        {
                            await this.ShowMessageDialog(string.Format("There already exists an item called {0}. Please rename it to something unique.", item.Name));
                            return;
                        }
                        itemIDs.Add(item.Name);
                    }
                }

                await App.Project.Save();
            }
        }

        private void ItemViewModel_ItemAdditionOccurred(object sender, ItemViewModel e)
        {
            this.RefreshAllItemsList();
        }

        private void ItemControlBase_ItemSelectionChanged(object sender, ItemViewModel item)
        {
            if (item != null)
            {
                switch (item.Type)
                {
                    case ItemTypeEnum.Text:
                        this.ItemEditorContentControl.Content = new TextItemEditorControl((TextItemViewModel)item);
                        break;
                    case ItemTypeEnum.Image:
                        this.ItemEditorContentControl.Content = new ImageItemEditorControl((ImageItemViewModel)item);
                        break;
                }
                
                if (this.AllItems.SelectedItem != item)
                {
                    this.AllItems.SelectedItem = item;
                }
            }
            else
            {
                this.ItemEditorContentControl.Content = null;
                this.AllItems.SelectedIndex = -1;
            }
        }

        private void ItemViewModel_ItemDeletionOccurred(object sender, ItemViewModel e)
        {
            this.RefreshAllItemsList();
        }

        private void AllItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AllItems.SelectedIndex >= 0)
            {
                ItemViewModel item = (ItemViewModel)this.AllItems.SelectedItem;
                ItemViewModel.ItemSelected(item);
            }
        }

        private void RefreshAllItemsList()
        {
            this.items.Clear();
            foreach (ItemViewModel item in App.CurrentScene.Items.OrderBy(i => i.Name))
            {
                this.items.Add(item);
            }
        }

        private async Task LoadingOperation(Func<Task> function)
        {
            this.StatusBar.Visibility = Visibility.Visible;
            this.IsEnabled = false;

            await function();

            this.IsEnabled = true;
            this.StatusBar.Visibility = Visibility.Collapsed;
        }

        private async Task ShowMessageDialog(string message)
        {
            await this.ShowDialog(new MessageDialogControl(message));
        }

        private async Task ShowDialog(UserControl control)
        {
            IEnumerable<Window> windows = Application.Current.Windows.OfType<Window>();
            if (windows.Count() > 0)
            {
                object obj = windows.FirstOrDefault().FindName("MDDialogHost");
                if (obj != null)
                {
                    DialogHost dialogHost = (DialogHost)obj;
                    await dialogHost.ShowDialog(control);
                }
            }
        }

        private void HideDialog()
        {
            IEnumerable<Window> windows = Application.Current.Windows.OfType<Window>();
            if (windows.Count() > 0)
            {
                object obj = windows.FirstOrDefault().FindName("MDDialogHost");
                if (obj != null)
                {
                    DialogHost dialogHost = (DialogHost)obj;
                    dialogHost.IsOpen = false;
                }
            }
        }
    }
}
