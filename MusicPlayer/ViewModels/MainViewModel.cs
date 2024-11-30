using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.WebUtilities;
using MusicPlayer.API;
using MusicPlayer.Authorization;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.Models.Config;
using MusicPlayer.Shared;
using MusicPlayer.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace MusicPlayer.ViewModels;

/// <summary>
/// This ViewModel contains all sub ViewModels, controls navigation between them and handles bindings from the menubar.
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    /* Spotify deprecated the recommendations end point 4 days before the deadline.
     * Using the https://open.spotify.com/get_access_token to get an access token the deprecated end point can be reached.
     * Authentication still works, but the deprecated end point will use the spotify app token.
     */
    private bool IsTokenOverride = true;
    private string TOKEN_OVERRIDE = "BQDl3Z5ye0pb2Yp_V84gFyBwhEiNSIfzGOw_o32yLzP-ZOaB2ZJBnkYfBxVRCsBvDbIdkldUrC4cQQEylzPJmD8b7sYylJAdGlVqtP9OS-iCoJRKTzce7Iz9t9G_EIjuI3_KAbl49e4Y2AUJlfvFR8pUnRLDB1-lZi8rcYYcnJgAwcRuvAJ2aTvNbkvMBcMDOV9jPGxHGglFbFTYBolq6jKX7c9pKB-zB2QrK10nQi0R-KpW8WJDkli0xFCQm2k5KuZ_diuKvh4nien6BO8NVruqu_U-ISau89toJg69aRUIC_YfICKkvExYjibPeWcGXj1ty5P0FfnpOdF0XFHA4prXnN-yl2poNEe6";

    [ObservableProperty]
    private ViewModelBase selectedViewModel;

    private readonly HttpClient Client;
    private static HttpListener listener = new HttpListener();


    [ObservableProperty]
    private HomeContentViewModel homeContentViewModel;

    private readonly PlaylistsViewModel PlaylistsViewModel;
    private readonly ArtistsViewModel ArtistsViewModel;
    private readonly AlbumsViewModel AlbumsViewModel;
    private readonly GenresViewModel GenresViewModel;
    private readonly SpotifyRecViewModel RecViewModel;
    public readonly ControlWidget Control;

    [ObservableProperty]
    public bool userAuthenticated = false;

    private readonly MainWindow mainWindow;

    [ObservableProperty]
    public MusicNavigationViewModel musicNavigation;

    [ObservableProperty]
    public CurrentSongViewModel currentSongViewModel;

    public MainViewModel() { }
    /// <summary>
    /// The constructor has all the required dependencies injected into it, the class stores these objects in the respective fields.
    /// It also runs the <c>Init()</c> method, and sets the location and visibility of the <c>ControlWidget</c>
    /// </summary>
    /// <param name="homeContent">HomeContentViewModel injected using DI</param>
    /// <param name="client">HttpClient injected using DI</param>
    /// <param name="playlistsView">PlaylistsViewModel injected using DI</param>
    /// <param name="artistsView">ArtistsViewModel injected using DI</param>
    /// <param name="albumsView">AlbumsViewModel injected using DI</param>
    /// <param name="genresView">GenresViewModel injected using DI</param>
    /// <param name="spotifyRecViewModel">SpotifyRecViewModel injected using DI</param>
    /// <param name="musicNavigationView">MusicNavigationViewModel injected using DI</param>
    /// <param name="currentSongViewModel">CurrentSongViewModel injected using DI</param>
    /// <param name="mainWindow">MainWindow injected using DI</param>
    /// <param name="controlWidget">ControlWidget injected using DI</param>
    /// <param name="sharedProperties">SharedProperties injected using DI</param>
    public MainViewModel(HomeContentViewModel homeContent,
                         HttpClient client,
                         PlaylistsViewModel playlistsView,
                         ArtistsViewModel artistsView,
                         AlbumsViewModel albumsView,
                         GenresViewModel genresView,
                         SpotifyRecViewModel spotifyRecViewModel,
                         MusicNavigationViewModel musicNavigationView,
                         CurrentSongViewModel currentSongViewModel,
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
        MusicNavigation = musicNavigationView;
        CurrentSongViewModel = currentSongViewModel;
        Control = controlWidget;
        this.mainWindow = mainWindow;

        PixelRect screen = this.mainWindow.Screens.Primary.WorkingArea;
        Control.Position = new PixelPoint((int)(screen.BottomRight.X + Control.Width), (int)(screen.BottomRight.Y + Control.Height));
        Control.Show();
    }
    /// <summary>
    /// Method that sets the <c>SelectedViewModel</c> to the <c>HomeContentViewModel</c>, and collects the music files from <c>Properties.SourceFolderPath</c>
    /// </summary>
    private void Init()
    {
        SelectedViewModel = HomeContentViewModel;
        //Possibly remove in the end, user can input the source folder themselves
        Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(Properties.SourceFolderPath);

    }

    /// <summary>
    /// Handles hiding and showing the <c>ControlWidget</c>
    /// </summary>
    public void ToggleWidget()
    {
        if (Control.IsVisible)
        {
            Control.Hide();
        }
        else
        {
            Control.Show();
        }

        mainWindow.Focus();
    }
    #region ViewModel Switching
    /// <summary>
    /// Changes the <c>SelectedViewModel</c> based on the supplied parameter.
    /// In case the parameter is incorrect, nothing happens.
    /// </summary>
    /// <param name="view">The name of the view, which the user requests</param>
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
    /// <summary>
    /// Modifies the source folder, both in <c>Properties.SourceFolderPath</c> and the config file.
    /// In case the new folder contains valid files, it also fills the app with song data.
    /// </summary>
    public async void SetInputFolderAsync()
    {
        IReadOnlyList<IStorageFolder> selectedFolder =
            await TopLevel
            .GetTopLevel(mainWindow)
            .StorageProvider
            .OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Source Folder" });
        Properties.SourceFolderPath = selectedFolder.First().Path.AbsolutePath;

        StoreSourceFolderInConfig(Properties.SourceFolderPath);

        if (selectedFolder.Count > 0)
        {
            var files = MusicFileCollector.CollectFilesFromFolder(selectedFolder.First().TryGetLocalPath());
            Properties.MusicFiles.Clear();
            foreach (var file in files)
            {
                Properties.MusicFiles.Add(file);
            }
        }
        SelectedViewModel.RefreshContent();
    }
    /// <summary>
    /// Modifies the <c>appsettings.json</c> based on the supplied parameter. 
    /// </summary>
    /// <param name="path">The absolute path of the selected folder</param>
    /// <returns></returns>
    private static bool StoreSourceFolderInConfig(string path)
    {
        try
        {
            string json = File.ReadAllText("appsettings.json");
            Config config = JsonConvert.DeserializeObject<Config>(json);
            if (OperatingSystem.IsWindows())
            {
                config.SOURCE_FOLDERS.WIN = path;
            }
            else
            {
                config.SOURCE_FOLDERS.LINUX = path;
            }
            json = JsonConvert.SerializeObject(config);
            File.WriteAllText("appsettings.json", json);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    #endregion
    #region Authorization
    /// <summary>
    /// Generates code verifier and code challenge.
    /// Opens the default browser with the authentication page, starts a callback listener,
    /// After getting authorization, stores the received data inside the application.
    /// </summary>
    public async void AuthorizeUserAsync()
    {
        string authUrl = "https://accounts.spotify.com/authorize";
        string codeVerifier = PKCEExtension.GenerateCodeVerifier(64);
        byte[] bytes = Encoding.UTF8.GetBytes(codeVerifier);

        AuthorizationObject authorization = new AuthorizationObject();

        authorization.CodeChallenge = PKCEExtension.GenerateCodeChallenge(bytes);

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
        if (context != null)
        {
            var codeToRetrieve = context.Request.QueryString["code"];
            if (codeToRetrieve != null)
            {
                Properties.AuthData = await APICallHandler.GetAccessTokenAsync(Client, authorization, codeToRetrieve, codeVerifier);
                if (Properties.AuthData != null)
                {
                    if (IsTokenOverride)
                    {
                        Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + TOKEN_OVERRIDE);
                    }
                    else
                    {
                        Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.AuthData.AccessToken);
                    }
                    //Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.AuthData.AccessToken);
                    //Automatically get the seeds, which the user can choose later
                    await RecViewModel.GetAvaliableGenreSeeds();
                    //Recommendations nav binds it's state to this variable
                    UserAuthenticated = true;
                }
            }
        }


    }
    /// <summary>
    /// Starts a callback listener based on the parameter, it stops once the callback happens.
    /// </summary>
    /// <param name="redirectUri">The pre-definde redirect URL</param>
    /// <returns>The received <c>HttpListenerContext</c></returns>
    private static async Task<HttpListenerContext> StartCallbackListener(string redirectUri)
    {
        try
        {
            if (listener.IsListening)
            {
                listener.Stop();
                listener.Prefixes.Remove(redirectUri + "/");
            }

            listener.Prefixes.Add(redirectUri + "/");
            listener.Start();
            HttpListenerContext context = await listener.GetContextAsync();
            listener.Stop();
            return context;
        }
        catch (Exception ex) 
        {
            return null;
        }


    }

    #endregion

}
