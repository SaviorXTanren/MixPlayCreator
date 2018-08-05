﻿using System.Windows.Controls;

namespace MixPlayerCreator.WPF.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageDialogControl.xaml
    /// </summary>
    public partial class MessageDialogControl : UserControl
    {
        public MessageDialogControl(string message)
        {
            this.DataContext = message;

            InitializeComponent();
        }
    }
}
