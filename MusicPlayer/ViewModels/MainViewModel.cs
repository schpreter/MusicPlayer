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

    static readonly HttpClient client = new HttpClient();

    [ObservableProperty]
    private HomeContentViewModel homeContentViewModel;
    private readonly PlaylistsViewModel playlistsViewModel;
    private readonly ArtistsViewModel artistsViewModel;
    private readonly AlbumsViewModel albumsViewModel;
    private readonly GenresViewModel genresViewModel;
    private readonly ControlWidget control;
    private AuthorizationObject authorization;

    private readonly MainWindow mainWindow;

    [ObservableProperty]
    public MusicNavigationViewModel musicNavigation;

    public MainViewModel() { }
    public MainViewModel(HomeContentViewModel homeContent,
        AuthorizationObject authorizationObject,
        PlaylistsViewModel playlistsView,
        ArtistsViewModel artistsView,
        AlbumsViewModel albumsView,
        GenresViewModel genresView,
        MusicNavigationViewModel musicNavigationView,
        MainWindow mainWindow,
        ControlWidget controlWidget,
        SharedProperties sharedProperties)
    {
        Properties = sharedProperties;
        HomeContentViewModel = homeContent;
        authorization = authorizationObject;
        Init();

        playlistsViewModel = playlistsView;
        artistsViewModel = artistsView;
        albumsViewModel = albumsView;
        genresViewModel = genresView;
        musicNavigation = musicNavigationView;
        control = controlWidget;
        this.mainWindow = mainWindow;

        PixelRect screen = this.mainWindow.Screens.Primary.WorkingArea;

        control.Position = new PixelPoint((int)(screen.BottomRight.X), (int)(screen.BottomRight.Y));
        control.Show();
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
                SelectedViewModel = playlistsViewModel;
                break;
            case "ARTISTS":
                SelectedViewModel = artistsViewModel;
                break;
            case "ALBUMS":
                SelectedViewModel = albumsViewModel;
                break;
            case "GENRES":
                SelectedViewModel = genresViewModel;
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
        var selectedFolder = await TopLevel.GetTopLevel(mainWindow).StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false, Title = "Select Input Folder" });
        if (selectedFolder.Count > 0)
        {
            Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(selectedFolder.First().TryGetLocalPath());
        }
    }

    public async void AuthorizeUserAsync()
    {
        var authUrl = "https://accounts.spotify.com/authorize";
        var codeVerifier = PKCEExtension.GenerateCodeVerifier(64);
        byte[] bytes = Encoding.UTF8.GetBytes(codeVerifier);

        var codeChallenge = PKCEExtension.GenerateCodeChallenge(bytes);
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
        Process.Start(new ProcessStartInfo
        {
            FileName = authUrl,
            UseShellExecute = true
        });

        var context = await StartCallbackListener(codeVerifier);

        // This is the code from Spotify API
        var codeToRetrieve = context.Request.QueryString["code"];
        if (codeToRetrieve != null)
        {
            string accessToken = await GetAccessToken(codeToRetrieve, codeVerifier);
            string profile = await FetchProfile(accessToken);
        }

        //try
        //{
        //    //using HttpResponseMessage response = await client.GetAsync(authUrl);
        //    //response.EnsureSuccessStatusCode();
        //    //string responseBody = await response.Content.ReadAsStringAsync();
        //    // Above three lines can be replaced with new helper method below
        //    string responseBody = await client.GetStringAsync(authUrl);

        //    Console.WriteLine(responseBody);
        //}
        //catch (HttpRequestException e)
        //{
        //    Console.WriteLine("\nException Caught!");
        //    Console.WriteLine("Message :{0} ", e.Message);
        //}


    }



    private async Task<HttpListenerContext> StartCallbackListener(string verifier)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(authorization.RedirectUri + "/");
        listener.Start();
        var context = await listener.GetContextAsync();
        listener.Stop();
        return context;

    }
    private async Task<string> GetAccessToken(string code, string verifier)
    {
        string url = "https://accounts.spotify.com/api/token";
        string query = "";
        //query = QueryHelpers.AddQueryString(query, new Dictionary<string, string>()
        //{
        //    {"client_id" , authorization.ClientID },
        //    {"grant_type" , "authorization_code" },
        //    {"code", code },
        //    {"redirect_uri" , authorization.RedirectUri },
        //    {"code_verifier" , verifier },
        //});

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
        var response = await client.PostAsync(url, content);

        AuthorizationResponseObject parsedResponse = JsonConvert.DeserializeObject<AuthorizationResponseObject>(response.Content.ReadAsStringAsync().Result);
        //try
        //{
        //    //using HttpResponseMessage response = await client.GetAsync(authUrl);
        //    //response.EnsureSuccessStatusCode();
        //    //string responseBody = await response.Content.ReadAsStringAsync();
        //    // Above three lines can be replaced with new helper method below
        //    string responseBody = await client.GetStringAsync(url);

        //    Console.WriteLine(responseBody);
        //}
        //catch (HttpRequestException e)
        //{
        //    Console.WriteLine("\nException Caught!");
        //    Console.WriteLine("Message :{0} ", e.Message);
        //}
        return null;
    }

    private async Task<string> FetchProfile(string token)
    {
        return null;

    }

    #endregion

    //public void OpenBrowser()
    //{
    //    Process.Start(new ProcessStartInfo
    //    {
    //        FileName = "http://www.google.com",
    //        UseShellExecute = true
    //    });
    //}
}
