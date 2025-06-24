using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;


public class JsUserMetadata
{
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
}

public class JsAppMetadata
{
    [JsonPropertyName("provider")]
    public string? Provider { get; set; }
}

