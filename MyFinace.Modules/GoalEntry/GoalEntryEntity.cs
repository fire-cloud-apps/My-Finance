using MyFinace.Modules.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyFinace.Modules.GoalEntry;
internal class GoalEntryEntity
{
    /// <summary>
    /// Auto Generated Id for the GoalEntity.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonPropertyName("parent_GoalId")]
    public Guid ParentId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Auto Generated DateTime for the GoalEntity.
    /// </summary>

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Mandatory Name for the GoalEntity.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Multiline text field for the description of the goal.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    
    public Platform Platform { get; set; }

    public bool IsActive { get; set; } = true;

    public string FundName { get; set; } = string.Empty;

    /// <summary>
    /// SIPAmount is the amount that is invested regularly (e.g., monthly) towards the goal.
    /// </summary>
    [JsonPropertyName("sipAmount")]
    public decimal SIPAmount { get; set; }

    /// <summary>
    /// Mandatory TargetAmount for the GoalEntity.
    /// </summary>
    public decimal TargetAmount { get; set; }
    /// <summary>
    /// Multi Text filed, where use can add the image in SVG format.
    /// </summary>
    public string ImageSVG { get; set; }

    /// <summary>
    /// A comma separated string of tags for the GoalEntity.
    /// </summary>
    public string Tag { get; set; }
       
    /// <summary>
    /// OwenedBy is the user who owns the GoalEntity.
    /// </summary>
    public string OwnedBy { get; set; }

}

public enum Platform 
{
    ETMoneySRG,
    ETMoneyAlam,
    PaytmMoneySRG,
    PaytmMoneyAlam,

    Jupyter,
    NPS,
    EPF,
    VPF, 
    NPSPritish,
    NPSJyoshmitha,

    HDFCBank,
    AxisBank,
    ICICIBank
}

