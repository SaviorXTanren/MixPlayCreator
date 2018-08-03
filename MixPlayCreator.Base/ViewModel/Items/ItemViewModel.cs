using MixPlayCreator.Base.Model.Items;
using System;
using System.ComponentModel;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public static event EventHandler<ItemViewModel> ItemSelectionChanged = delegate { };

        public static void ItemSelected(ItemViewModel item) { ItemViewModel.ItemSelectionChanged(null, item); }

        public event PropertyChangedEventHandler PropertyChanged;

        public ItemTypeEnum Type
        {
            get { return this.Model.Type; }
            set
            {
                this.Model.Type = value;
                this.NotifyPropertyChanged("Type");
            }
        }

        public int ZIndex
        {
            get { return this.Model.ZIndex; }
            set
            {
                this.Model.ZIndex = value;
                this.NotifyPropertyChanged("ZIndex");
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
    }
}
