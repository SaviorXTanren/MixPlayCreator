using MixPlayCreator.Base.Model.Items;

namespace MixPlayCreator.Base.ViewModel.Items
{
    public class TextItemViewModel : ItemViewModel
    {
        public string Text
        {
            get { return this.Model.Text; }
            set
            {
                this.Model.Text = value;
                this.NotifyPropertyChanged("Text");
            }
        }

        public int Size
        {
            get { return this.Model.Size; }
            set
            {
                this.Model.Size = value;
                this.NotifyPropertyChanged("Size");
            }
        }

        public string Color
        {
            get { return this.Model.Color; }
            set
            {
                this.Model.Color = value;
                this.NotifyPropertyChanged("Color");
            }
        }

        public string Font
        {
            get { return this.Model.Font; }
            set
            {
                this.Model.Font = value;
                this.NotifyPropertyChanged("Font");
            }
        }

        public new TextItemModel Model { get; private set; }

        public TextItemViewModel() : this(new TextItemModel()) { }

        public TextItemViewModel(TextItemModel model) : base(model) { this.Model = model; }
    }
}
