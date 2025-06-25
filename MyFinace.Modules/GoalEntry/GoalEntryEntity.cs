using MyFinace.Modules.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFinace.Modules.GoalEntry;


internal class GoalEntry
{
    /// <summary>
    /// Auto Generated Id for the GoalEntity.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid(); // Ensure a default GUID is set for new entities

    /// <summary>
    /// Should be selected from the auto complete of GoalEntity.Id is considered as ParentId.
    /// </summary>
    [JsonPropertyName("parent_GoalId")]
    public Guid ParentId { get; set; }


    /// <summary>
    /// Mandatory Name for the GoalEntity, when selected from the GoalEntity the GoalEntity.Name to be auto populated.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Auto Generated DateTime for the GoalEntity.
    /// </summary>

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Multiline text field for the description of the goal.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }


    /// <summary>
    /// Should be from the GoalEntryMetaData string array, which should be selected from the UI.
    /// </summary>
    [JsonPropertyName("platform")] // Explicitly map to lowercase "platform" in JSON
    public string Platform { get; set; } = GoalEntryMetaData.Platform[0];

    [JsonPropertyName("isActive")] // Explicitly map for consistency
    public bool IsActive { get; set; } = true;

    [JsonPropertyName("fundName")] // Explicitly map for consistency
    public string FundName { get; set; } = string.Empty;

    /// <summary>
    /// SIPAmount is the amount that is invested regularly (e.g., monthly) towards the goal.
    /// </summary>
    [JsonPropertyName("sipAmount")]
    public decimal SIPAmount { get; set; }

    /// <summary>
    /// Mandatory TargetAmount for the GoalEntity.
    /// </summary>
    [JsonPropertyName("targetAmount")] // Explicitly map for consistency
    public decimal TargetAmount { get; set; }
    /// <summary>
    /// Mandatory InvestedAmount for the GoalEntity.
    /// </summary>
    [JsonPropertyName("investedAmount")] // Explicitly map for consistency
    public decimal InvestedAmount { get; set; } = 10000;
    /// <summary>
    /// Multi Text filed, where use can add the image in SVG format.
    /// </summary>
    [JsonPropertyName("imageSVG")] // Explicitly map for consistency
    public string ImageSVG { get; set; }

    /// <summary>
    /// A comma separated string of tags for the GoalEntity.
    /// </summary>
    [JsonPropertyName("tag")] // Explicitly map for consistency
    public string Tag { get; set; }

    /// <summary>
    /// OwenedBy is the user who owns the GoalEntity.
    /// </summary>
    [JsonPropertyName("ownedBy")] // Explicitly map for consistency
    public string OwnedBy { get; set; } = string.Empty;

}


public class GoalEntryMetaData
{
    public static string[] Platform = new string[] { "ET Money SRG", "ET Money Alam", "PayTm SRG", "PayTm Alam", "Jypyter", "NPS", "EPF", "VPF", "NPS Pritish", "NPS Jyoshmitha", "HDFC Bank", "Axis Bank", "ICICI Bank" };
}
