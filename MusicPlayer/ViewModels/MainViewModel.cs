namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string User => "John Doe";
    public string Greeting => $"Welcome, {User}";
}
