using MixPlayCreator.Base.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace MixPlayCreator.WPF.Controls.Editors
{
    /// <summary>
    /// Interaction logic for TextItemEditorControl.xaml
    /// </summary>
    public partial class TextItemEditorControl : UserControl
    {
        private static readonly List<int> SampleFontSize = new List<int>() { 12, 24, 36, 48, 60, 72, 84, 96, 108, 120 };

        private TextItemViewModel item;

        private ObservableCollection<string> colors = new ObservableCollection<string>();
        private ObservableCollection<string> fonts = new ObservableCollection<string>();

        public TextItemEditorControl(TextItemViewModel item)
        {
            this.item = item;

            InitializeComponent();

            this.DataContext = this.item;
            this.GeneralItemEditor.SetItem(item);

            this.SizeComboBox.ItemsSource = SampleFontSize;

            this.ColorComboBox.ItemsSource = this.colors;
            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                if (28 <= (int)knownColor && (int)knownColor <= 167)
                {
                    string colorName = System.Drawing.Color.FromKnownColor(knownColor).ToString();
                    colorName = colorName.Replace("Color [", "");
                    colorName = colorName.Replace("]", "");
                    this.colors.Add(colorName);
                }
            }
            this.ColorComboBox.Text = this.item.Color;

            this.FontComboBox.ItemsSource = this.fonts;
            foreach (System.Windows.Media.FontFamily font in Fonts.SystemFontFamilies.OrderBy(f => f.Source))
            {
                this.fonts.Add(font.Source);
            }
        }
    }
}
