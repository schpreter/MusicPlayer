using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MusicPlayer.Models
{
    public class AuthorizationObject
    {
        public string ClientID { get; set; }
        public string ResponseType { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string CodeChallenge { get; set; }

        
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
