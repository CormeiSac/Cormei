using System.Text.Json.Serialization;

namespace Cormei.Core.Models.Login
{
    public class LoginResult
    {
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_expires")]
        public DateTime RefreshExpires { get; set; }
    }
}