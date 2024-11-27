﻿using Avalonia;
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
    public readonly ControlWidget Control;

    [ObservableProperty]
    public bool userAuthenticated = false;

    private readonly MainWindow mainWindow;

    [ObservableProperty]
    public MusicNavigationViewModel musicNavigation;

    [ObservableProperty]
    public CurrentSongViewModel currentSongViewModel;

    public MainViewModel() { }
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
    private void Init()
    {
        SelectedViewModel = HomeContentViewModel;
        //Possibly remove in the end, user can input the source folder themselves
        Properties.MusicFiles = MusicFileCollector.CollectFilesFromFolder(Properties.SourceFolderPath);

    }

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
        Properties.SourceFolderPath = selectedFolder.First().Path.AbsolutePath;

        ModifyConfigFile(Properties.SourceFolderPath);

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

    private bool ModifyConfigFile(string path)
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
        var codeToRetrieve = context.Request.QueryString["code"];
        if (codeToRetrieve != null)
        {
            Properties.AuthData = await APICallHandler.GetAccessTokenAsync(Client, authorization, codeToRetrieve, codeVerifier);
            //This will not be reached if the previous line throws an exception
            if (Properties.AuthData != null)
            {
                Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.AuthData.AccessToken);
                //Recommendations nav binds it's state to this variable
                await RecViewModel.GetAvaliableGenreSeeds();
                UserAuthenticated = true;
            }
            else
            {
                //Maybe show a popup in the app that the auth failed or something
            }

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

    #endregion

}
