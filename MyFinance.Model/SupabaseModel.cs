using System.Text.Json.Serialization;

namespace MyFinance.Model
{
    /// <summary>
    /// Represents the base class for Supabase table models.
    /// It's recommended to inherit from this and add your specific table columns,
    /// decorating them with [JsonPropertyName] attributes if needed.
    /// </summary>
    public abstract class SupabaseModel
    {
        // Supabase often uses 'id' and 'created_at' as default columns.
        // You can include them here or in your derived classes.
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("lastModified")]
        public DateTime? LastModified { get; set; } = DateTime.Now;
    }

    public class GoalEntryModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
