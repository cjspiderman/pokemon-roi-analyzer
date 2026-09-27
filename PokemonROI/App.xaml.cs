using System.Windows;
using PokemonROI.Services;
using PokemonROI.ViewModels;

namespace PokemonROI;
public partial class App : Application
{
    public static AppDatabase Database { get; private set; } = null!;
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Database = new AppDatabase();
        Database.Initialize();
        var window = new MainWindow { DataContext = new MainViewModel(Database) };
        window.Show();
    }
    protected override void OnExit(ExitEventArgs e) { Database?.Dispose(); base.OnExit(e); }
}
