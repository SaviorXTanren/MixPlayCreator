using MaterialDesignThemes.Wpf;
using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.ViewModel;
using MixPlayCreator.Base.ViewModel.Items;
using MixPlayerCreator.WPF.Controls;
using MixPlayerCreator.WPF.Controls.Editors;
using MixPlayerCreator.WPF.Controls.Items;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

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

            ItemViewModel.ItemSelectionChanged += ItemControlBase_ItemSelectionChanged;

            App.Project = new CDKProjectViewModel(@"S:\Code\MixPlayCreator\CDKProjectSample");
            App.CurrentScene = App.Project.Scenes.First();
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

        private void ItemControlBase_ItemSelectionChanged(object sender, ItemViewModel e)
        {
            if (e != null)
            {
                switch (e.Type)
                {
                    case ItemTypeEnum.Text:
                        this.ItemEditorContentControl.Content = new TextItemEditorControl((TextItemViewModel)e);
                        break;
                    case ItemTypeEnum.Image:
                        this.ItemEditorContentControl.Content = new ImageItemEditorControl((ImageItemViewModel)e);
                        break;
                }
            }
            else
            {
                this.ItemEditorContentControl.Content = null;
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
