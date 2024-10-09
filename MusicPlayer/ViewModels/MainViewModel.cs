using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.WebUtilities;
using MusicPlayer.Authorization;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace MusicPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    private readonly HttpClient Client;

    [ObservableProperty]
    private HomeContentViewModel homeContentViewModel;

    private readonly PlaylistsViewModel PlaylistsViewModel;
    private readonly ArtistsViewModel ArtistsViewModel;
    private readonly AlbumsViewModel AlbumsViewModel;
    private readonly GenresViewModel GenresViewModel;
    private readonly SpotifyRecViewModel RecViewModel;
    private readonly ControlWidget Control;
    private AuthorizationTokenData AuthData;

    [ObservableProperty]
    public bool userAuthenticated = false;

    private readonly MainWindow mainWindow;

    [ObservableProperty]
    public MusicNavigationViewModel musicNavigation;

    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent,
                         HttpClient client,
                         PlaylistsViewModel playlistsView,
                         ArtistsViewModel artistsView,
                         AlbumsViewModel albumsView,
                         GenresViewModel genresView,
                         SpotifyRecViewModel spotifyRecViewModel,
                         MusicNavigationViewModel musicNavigationView,
                         MainWindow mainWindow,
                         ControlWidget controlWidget,
                         SharedProperties sharedProperties)
    {
        Properties = sharedProperties;
        HomeContentViewModel = homeContent;

        Init();

        PlaylistsViewModel = playlistsView;
        ArtistsViewModel = artistsView;
        Client = client;
        AlbumsViewModel = albumsView;
        GenresViewModel = genresView;
        RecViewModel = spotifyRecViewModel;
        musicNavigation = musicNavigationView;
        Control = controlWidget;
        this.mainWindow = mainWindow;

        PixelRect screen = this.mainWindow.Screens.Primary.WorkingArea;

        Control.Position = new PixelPoint((int)(screen.BottomRight.X), (int)(screen.BottomRight.Y));
        Control.Show();
    }
    private void Init()
    {
        SelectedViewModel = HomeContentViewModel;
        //Possibly remove in the end, user can input the source folder themselves
        Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(Properties.SourceFolderPath);

    }
    #region ViewModel Switching
    public void ShowContent(string view)
    {
        switch (view)
        {
            case "HOME":
                SelectedViewModel = HomeContentViewModel;
                break;
            case "PLAYLISTS":
                SelectedViewModel = PlaylistsViewModel;
                break;
            case "ARTISTS":
                SelectedViewModel = ArtistsViewModel;
                break;
            case "ALBUMS":
                SelectedViewModel = AlbumsViewModel;
                break;
            case "GENRES":
                SelectedViewModel = GenresViewModel;
                break;
            case "RECOMMENDATIONS":
                SelectedViewModel = RecViewModel;
                break;
            default:
                break;
        }
        SelectedViewModel.RefreshContent();

    }
    #endregion

    #region Menu Methods
    public async void SetInputFolderAsync()
    {
        IReadOnlyList<IStorageFolder> selectedFolder = await TopLevel.GetTopLevel(mainWindow).StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Input Folder" });
        if (selectedFolder.Count > 0)
        {
            Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(selectedFolder.First().TryGetLocalPath());
        }
    }

    public async void AuthorizeUserAsync()
    {
        string authUrl = "https://accounts.spotify.com/authorize";
        string codeVerifier = PKCEExtension.GenerateCodeVerifier(64);
        byte[] bytes = Encoding.UTF8.GetBytes(codeVerifier);

        AuthorizationObject authorization = new AuthorizationObject();

        string codeChallenge = PKCEExtension.GenerateCodeChallenge(bytes);
        authorization.CodeChallenge = codeChallenge;

        authUrl = QueryHelpers.AddQueryString(authUrl, new Dictionary<string, string>()
        {
            {"response_type" , authorization.ResponseType },
            {"client_id" , authorization.ClientID },
            {"scope" , authorization.Scope },
            {"code_challenge_method" , authorization.CodeChallengeMethod },
            {"code_challenge" , authorization.CodeChallenge },
            {"redirect_uri" , authorization.RedirectUri },
        });
        //Open users default browser, with the Spotify authorization url and querydata
        Process.Start(new ProcessStartInfo
        {
            FileName = authUrl,
            UseShellExecute = true
        });

        HttpListenerContext context = await StartCallbackListener(authorization.RedirectUri);

        // This is the code from Spotify API
        var codeToRetrieve = context.Request.QueryString["code"];
        if (codeToRetrieve != null)
        {
            AuthData = await GetAccessToken(authorization, codeToRetrieve, codeVerifier);
            //string profile = await FetchProfile(accessToken);
        }


    }
    #endregion
    #region Authorization

    private async Task<HttpListenerContext> StartCallbackListener(string redirectUri)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(redirectUri + "/");
        listener.Start();
        HttpListenerContext context = await listener.GetContextAsync();
        listener.Stop();
        return context;

    }
    private async Task<AuthorizationTokenData> GetAccessToken(AuthorizationObject authorization, string code, string verifier)
    {
        string url = "https://accounts.spotify.com/api/token";


        using FormUrlEncodedContent content = new FormUrlEncodedContent(
            new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string> ( "client_id", authorization.ClientID ),
                new KeyValuePair<string, string> ( "grant_type", "authorization_code" ),
                new KeyValuePair<string, string> ( "code", code ),
                new KeyValuePair<string, string> ( "redirect_uri" , authorization.RedirectUri ),
                new KeyValuePair<string, string> ( "code_verifier" , verifier ),
            }
            );

        //Setting the header's content type   
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        //Post the content to the API    
        HttpResponseMessage response = await Client.PostAsync(url, content);
        UserAuthenticated = response.IsSuccessStatusCode;
        return JsonConvert.DeserializeObject<AuthorizationTokenData>(response.Content.ReadAsStringAsync().Result);
    }

    private async Task<string> FetchProfile(string token)
    {
        //TODO
        return null;

    }
    #endregion

}
