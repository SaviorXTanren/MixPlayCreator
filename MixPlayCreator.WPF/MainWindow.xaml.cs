﻿using Microsoft.Win32;
using Mixer.Base;
using Mixer.Base.Model.User;
using Mixer.Base.Util;
using MixPlayCreator.Base.Model;
using MixPlayCreator.Base.Model.Items;
using MixPlayCreator.Base.Util;
using MixPlayCreator.Base.ViewModel;
using MixPlayCreator.Base.ViewModel.Items;
using MixPlayCreator.WPF.Controls.Dialogs;
using MixPlayCreator.WPF.Controls.Editors;
using MixPlayCreator.WPF.Controls.Items;
using MixPlayCreator.WPF.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlayCreator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<SceneViewModel> scenes = new ObservableCollection<SceneViewModel>();
        private ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();

        private MixerConnection connection;
        private UserModel user;

        private List<OAuthClientScopeEnum> scopes = new List<OAuthClientScopeEnum>()
        {
            OAuthClientScopeEnum.channel__details__self,
            OAuthClientScopeEnum.interactive__manage__self,
            OAuthClientScopeEnum.interactive__robot__self,
            OAuthClientScopeEnum.user__details__self,
        };

        public MainWindow()
        {
            InitializeComponent();

            this.CurrentSceneComboBox.ItemsSource = this.scenes;
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
            foreach (ItemTypeEnum type in EnumHelper.GetEnumList<ItemTypeEnum>())
            {
                this.ItemsTypeStackPanel.Children.Add(new ItemTypeControl(new ItemModel(type.ToString(), type)));
            }

            ItemViewModel.ItemAdditionOccurred += ItemViewModel_ItemAdditionOccurred;
            ItemViewModel.ItemSelectionChanged += ItemControlBase_ItemSelectionChanged;
            ItemViewModel.ItemDeletionOccurred += ItemViewModel_ItemDeletionOccurred;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private async void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(() =>
            {
                using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        App.Project = new CDKProjectViewModel(folderDialog.SelectedPath);
                        this.SwitchToProjectGrid();
                    }
                }
                return Task.FromResult(0);
            });
        }

        private async void LoadProjectButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(async () =>
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = CDKProjectViewModel.MixPlayCreatorSettingsFileBrowserFilter;
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                if (fileDialog.ShowDialog() == true)
                {
                    using (StreamReader reader = new StreamReader(File.OpenRead(fileDialog.FileName)))
                    {
                        string fileContents = await reader.ReadToEndAsync();
                        CDKProjectModel project = SerializerHelper.DeserializeObjectFromString<CDKProjectModel>(fileContents);
                        App.Project = new CDKProjectViewModel(project, fileDialog.FileName);
                    }

                    this.SwitchToProjectGrid();
                }
            });
        }

        private void SwitchToProjectGrid()
        {
            this.StartupGrid.Visibility = Visibility.Hidden;
            this.MainProjectGrid.Visibility = Visibility.Visible;
            this.RefreshSceneList();
            this.CurrentSceneComboBox.SelectedIndex = 0;
        }

        private async void SaveProjectButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(async () =>
            {
                await this.SaveProject();
            });
        }

        private async void PreviewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(async () =>
            {
                if (await this.SaveProject())
                {
                    Process.Start(App.Project.IndexHTMLFilePath);
                }
            });
        }

        private async Task<bool> SaveProject()
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
                            await DialogHelper.ShowMessageDialog(string.Format("There already exists an item called {0}. Please rename it to something unique.", item.Name));
                            return false;
                        }
                        itemIDs.Add(item.Name);
                    }
                }

                if (string.IsNullOrEmpty(App.Project.SettingsFilePath))
                {
                    SaveFileDialog fileDialog = new SaveFileDialog();
                    fileDialog.Filter = CDKProjectViewModel.MixPlayCreatorSettingsFileBrowserFilter;
                    fileDialog.CheckPathExists = true;
                    fileDialog.FileName = App.Project.DefaultSettingFileName;
                    if (fileDialog.ShowDialog() == true)
                    {
                        App.Project.SettingsFilePath = fileDialog.FileName;
                    }
                }

                if (!string.IsNullOrEmpty(App.Project.SettingsFilePath))
                {
                    await App.Project.Save();
                    return true;
                }
            }
            return false;
        }

        private async void UploadProjectButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(async () =>
            {
                if (this.connection == null)
                {
                    this.connection = await MixerConnection.ConnectViaLocalhostOAuthBrowser(ConfigurationManager.AppSettings["ClientID"], scopes);
                    if (this.connection != null)
                    {
                        try
                        {
                            this.user = await this.connection.Users.GetCurrentUser();
                        }
                        catch (Exception)
                        {
                            await DialogHelper.ShowMessageDialog("Unable to get your user information from Mixer, please try again");
                        }
                    }
                    else
                    {
                        await DialogHelper.ShowMessageDialog("Unable to log in to Mixer, please ensure you approve in a timely manner");
                    }
                }

                if (this.connection != null)
                {
                    await App.Project.UploadLinkedInteractiveGame(this.connection);
                    await DialogHelper.ShowDialog(new ProjectUploadCompleteDialogControl());
                }
            });
        }

        private async void AddSceneButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(async () =>
            {
                object result = await DialogHelper.ShowBasicTextEntryDialog("Scene Name");
                if (result != null && !string.IsNullOrEmpty(result.ToString()))
                {
                    SceneViewModel newScene = new SceneViewModel(result.ToString());
                    App.Project.Scenes.Add(newScene);
                    this.RefreshSceneList();
                    this.CurrentSceneComboBox.SelectedItem = newScene;
                }
            });
        }

        private void CurrentSceneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CurrentSceneComboBox.SelectedIndex >= 0)
            {
                App.CurrentScene = (SceneViewModel)this.CurrentSceneComboBox.SelectedItem;
                this.RefreshItemsList();
                this.ItemsCanvas.RefreshChildren();
            }
        }

        private async void DeleteSceneButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoadingOperation(async () =>
            {
                if (App.Project.Scenes.Count >= 2)
                {
                    object result = await DialogHelper.ShowConfirmationDialog("Are you sure you wish to delete this scene?");
                    if (result != null && result.ToString().Equals("True"))
                    {
                        App.Project.Scenes.Remove(App.CurrentScene);
                        this.RefreshSceneList();
                        this.CurrentSceneComboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    await DialogHelper.ShowMessageDialog("You must have at least 1 scene");
                }
            });
        }

        private void ItemViewModel_ItemAdditionOccurred(object sender, ItemViewModel item)
        {
            string originalName = item.Name;
            int index = 1;
            IEnumerable<ItemViewModel> allItems = App.Project.Scenes.SelectMany(s => s.Items);
            while (allItems.Any(i => !i.Equals(item) && i.Name.Equals(item.Name)))
            {
                item.Name = originalName + index.ToString();
                index++;
            }

            this.RefreshItemsList();
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
                    case ItemTypeEnum.Sound:
                        this.ItemEditorContentControl.Content = new SoundItemEditorControl((SoundItemViewModel)item);
                        break;
                    case ItemTypeEnum.Video:
                        this.ItemEditorContentControl.Content = new VideoItemEditorControl((VideoItemViewModel)item);
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
            this.RefreshItemsList();
        }

        private void AllItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AllItems.SelectedIndex >= 0)
            {
                ItemViewModel item = (ItemViewModel)this.AllItems.SelectedItem;
                ItemViewModel.ItemSelected(item);
            }
        }

        private void RefreshSceneList()
        {
            this.scenes.Clear();
            foreach (SceneViewModel scene in App.Project.Scenes.OrderBy(i => i.Name))
            {
                this.scenes.Add(scene);
            }
        }

        private void RefreshItemsList()
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
    }
}
