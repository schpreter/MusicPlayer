using Avalonia;
using Avalonia.Headless;
using Avalonia.Markup.Xaml;
using MusicPlayerTest;


[assembly: AvaloniaTestApplication(typeof(App))]

namespace MusicPlayerTest;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}