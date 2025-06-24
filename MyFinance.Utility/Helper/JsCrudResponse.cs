using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;

public class JsCrudResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}

