using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MusicPlayer.ViewModels;

namespace MusicPlayer.Views;

public partial class MusicNavigationView : UserControl
{
    public MusicNavigationView()
    {
        InitializeComponent();
        slider.AddHandler(InputElement.PointerReleasedEvent, Slider_PointerReleased, RoutingStrategies.Tunnel);
        slider.AddHandler(InputElement.PointerPressedEvent, Slider_PointerPressed, RoutingStrategies.Tunnel);

    }
    private void Slider_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (DataContext is MusicNavigationViewModel viewModel)
        {
            viewModel.SliderDragging((long)((Slider)sender).Value);
        }
    }
    private void Slider_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        if (DataContext is MusicNavigationViewModel viewModel)
        {
            viewModel.SliderUserChanged((long)((Slider)sender).Value);
        }
    }
}