﻿using MixPlayCreator.Base.Model.Interactive;
using MixPlayCreator.Base.Model.Items;
using System;
using System.ComponentModel;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public static event EventHandler<ItemViewModel> ItemAdditionOccurred = delegate { };
        public static event EventHandler<ItemViewModel> ItemSelectionChanged = delegate { };
        public static event EventHandler<ItemViewModel> ItemDeletionOccurred = delegate { };
        public static event EventHandler<ItemViewModel> ItemZIndexChangeOccurred = delegate { };

        public static void ItemAdded(ItemViewModel item)
        {
            ItemViewModel.ItemAdditionOccurred(null, item);
            ItemViewModel.ItemSelectionChanged(null, item);
        }
        public static void ItemSelected(ItemViewModel item) { ItemViewModel.ItemSelectionChanged(null, item); }
        public static void ItemDeleted(ItemViewModel item)
        {
            ItemViewModel.ItemDeletionOccurred(null, item);
            ItemViewModel.ItemSelectionChanged(null, null);
        }
        public static void ItemZIndexChanged(ItemViewModel item) { ItemViewModel.ItemZIndexChangeOccurred(null, item); }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return this.Model.Name; }
            set
            {
                this.Model.Name = value;
                this.NotifyPropertyChanged("Name");
            }
        }

        public ItemTypeEnum Type
        {
            get { return this.Model.Type; }
            set
            {
                this.Model.Type = value;
                this.NotifyPropertyChanged("Type");
                this.NotifyPropertyChanged("TypeString");
                this.NotifyPropertyChanged("TypeImageSourcePath");
            }
        }

        public string TypeString { get { return this.Model.TypeString; } }

        public string TypeImageSourcePath { get { return this.Model.TypeImageSourcePath; } }

        public bool IsVisible
        {
            get { return this.Model.IsVisible; }
            set
            {
                this.Model.IsVisible = value;
                this.NotifyPropertyChanged("IsVisible");
                this.NotifyPropertyChanged("VisibleOpacity");
            }
        }

        public double XPosition
        {
            get { return this.Model.XPosition; }
            set
            {
                this.Model.XPosition = value;
                this.NotifyPropertyChanged("XPosition");
            }
        }

        public double YPosition
        {
            get { return this.Model.YPosition; }
            set
            {
                this.Model.YPosition = value;
                this.NotifyPropertyChanged("YPosition");
            }
        }

        public int ZIndex
        {
            get { return this.Model.ZIndex; }
            set
            {
                this.Model.ZIndex = value;
                this.NotifyPropertyChanged("ZIndex");
                ItemViewModel.ItemZIndexChanged(this);
            }
        }

        public InteractiveModelBase Interactive
        {
            get { return this.Model.Interactive; }
            set
            {
                this.Model.Interactive = value;
                this.NotifyPropertyChanged("Interactive");
            }
        }

        public bool IsInteractive { get { return (this.Model.Interactive != null && this.Model.Interactive is InteractiveButtonModel); } }

        public int SparkCost
        {
            get
            {
                if (this.Model.Interactive != null && this.Model.Interactive is InteractiveButtonModel)
                {
                    return ((InteractiveButtonModel)this.Model.Interactive).SparkCost;
                }
                return 0;
            }
            set
            {
                if (this.Model.Interactive != null && this.Model.Interactive is InteractiveButtonModel)
                {
                    ((InteractiveButtonModel)this.Model.Interactive).SparkCost = value;
                }
                this.NotifyPropertyChanged("SparkCost");
            }
        }

        public ItemModel Model { get; private set; }

        public ItemViewModel(ItemModel model) { this.Model = model; }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
                this.NotifyPropertyChanged("SelectedBorderBrush");
            }
        }

        public string SelectedBorderBrush { get { return (this.IsSelected) ? "DarkBlue" : "Transparent"; } }

        public double VisibleOpacity { get { return (this.IsVisible) ? 1.0 : 0.25; } }
    }
}
