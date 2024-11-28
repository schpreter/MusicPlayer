namespace MusicPlayer.Models
{
    /// <summary>
    /// Class structure storing the authorization data sent to the authorize endpoint.
    /// </summary>
    public class AuthorizationObject
    {
        public string ClientID { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string CodeChallengeMethod { get; set; } = string.Empty;
        public string CodeChallenge { get; set; } = string.Empty;   


        public AuthorizationObject()
        {
            ClientID = "a14239410839464680e8d0eb8a016e6b";
            Scope = "user-read-private user-read-email";
            ResponseType = "code";
            RedirectUri = "http://localhost:8888/callback";
            CodeChallengeMethod = "S256";
        }
    }
}
