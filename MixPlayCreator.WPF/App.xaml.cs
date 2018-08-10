using MixPlayCreator.Base.ViewModel;
using System.Windows;

namespace MixPlayCreator.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CDKProjectViewModel Project { get; set; }
        public static SceneViewModel CurrentScene { get; set; }
    }
}
