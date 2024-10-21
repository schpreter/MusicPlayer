using CommunityToolkit.Mvvm.ComponentModel;
using MusicPlayer.Shared;

namespace MusicPlayer.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    protected SharedProperties properties;
    public virtual void RefreshContent() { }
    public virtual void SetCategoryFilters(string category) { }

}
