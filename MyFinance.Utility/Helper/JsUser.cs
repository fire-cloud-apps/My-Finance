using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;
public class JsUser
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("last_sign_in_at")]
    public DateTime? LastSignInAt { get; set; }
    [JsonPropertyName("user_metadata")]
    public JsonElement UserMetadata { get; set; }
    [JsonPropertyName("app_metadata")]
    public JsonElement AppMetadata { get; set; }
}
//public class JsTasksResponse
//{
//    [JsonPropertyName("data")]
//    public List<TaskItem>? Data { get; set; }
//    [JsonPropertyName("count")]
//    public int Count { get; set; }
//    [JsonPropertyName("error")]
//    public JsError? Error { get; set; }
//}