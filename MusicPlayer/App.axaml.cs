using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Models;
using MusicPlayer.ViewModels;
using MusicPlayer.Views;

namespace MusicPlayer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        // Register all the services needed for the application to run
        
        ServiceCollection collection = new ServiceCollection();
        collection.AddCommonServices();
        

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        ServiceProvider services = collection.BuildServiceProvider();

        MainViewModel viewModel = services.GetRequiredService<MainViewModel>();
        //Need for tray functionality, as it can not be binded to the window datacontext
        this.DataContext = viewModel;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = this.DataContext
            };
            
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = this.DataContext
            };
        }
       

        base.OnFrameworkInitializationCompleted();
    }
}
