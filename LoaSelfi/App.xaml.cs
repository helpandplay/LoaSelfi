using System.Windows;
using LoaSelfi.ViewModel;

namespace LoaSelfi;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Main main = new Main()
        {
            DataContext = new MainViewModel(),
            WindowState = WindowState.Normal,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        main.Show();
        main.Activate();
        main.Focus();
    }
}
