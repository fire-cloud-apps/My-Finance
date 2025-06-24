using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;

public class JsSession
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }
    [JsonPropertyName("expires_at")]
    public long? ExpiresAt { get; set; }
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
    [JsonPropertyName("user")]
    public JsUser? User { get; set; }

    public DateTime? GetExpiresAtDateTime()
    {
        if (ExpiresAt.HasValue)
        {
            return DateTimeOffset.FromUnixTimeSeconds(ExpiresAt.Value).LocalDateTime;
        }
        return null;
    }

    public string? GetUserMetadataString(string key)
    {
        if (User != null && User.UserMetadata.ValueKind == JsonValueKind.Object && User.UserMetadata.TryGetProperty(key, out var prop) && prop.ValueKind == JsonValueKind.String)
        {
            return prop.GetString();
        }
        return null;
    }

    public string? GetAppMetadataString(string key)
    {
        if (User != null && User.AppMetadata.ValueKind == JsonValueKind.Object && User.AppMetadata.TryGetProperty(key, out var prop) && prop.ValueKind == JsonValueKind.String)
        {
            return prop.GetString();
        }
        return null;
    }
}
