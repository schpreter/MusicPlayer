using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    protected SharedProperties properties;
    /// <summary>
    /// Refreshes the data for the given ViewModel, and triggers a UI refresh.
    /// </summary>
    public virtual void RefreshContent() { }

}
