using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    protected SharedProperties properties;

}
