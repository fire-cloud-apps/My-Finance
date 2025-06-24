using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MyFinace.Modules.Goals;
/// <summary>
/// Finance Goals Entity represents a financial goal with various properties such as name, description, status, investment category, amounts, target years, and ownership.
/// </summary>
public class GoalEntity
{
    /// <summary>
    /// Auto Generated Id for the GoalEntity.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

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

    /// <summary>
    /// Comes form the GoalStatus enum, which indicates the status of the goal.
    /// </summary>
    public GoalStatus Status { get; set; }


    /// <summary>
    /// Some enum 'Investment Category' for the ctegory of the GoalEntity.
    /// </summary>
    public InvestmentCategory InvestmentCategory { get; set; }
    /// <summary>
    /// Mandatory TotalAmountInvested for the GoalEntity.
    /// </summary>
    public decimal TotalAmountInvested { get; set; }
    /// <summary>
    /// Mandatory TargetAmount for the GoalEntity.
    /// </summary>
    public decimal TargetAmount { get; set; }
    /// <summary>
    /// SIPAmount is the amount that is invested regularly (e.g., monthly) towards the goal.
    /// </summary>
    public decimal TotalSIPAmount { get; set; }
    /// <summary>
    /// Mandatory TargetYear for the GoalEntity.
    /// </summary>
    public DateTime? TargetYear { get; set; }
    /// <summary>
    /// Mandatory StartedYear for the GoalEntity.
    /// </summary>
    public DateTime? StartedYear { get; set; }

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

public enum GoalStatus
{
    /// <summary>
    /// Goal is in progress.
    /// </summary>
    YTB = 0,
    /// <summary>
    /// Goal is in progress.
    /// </summary>
    InProgress = 1,
    /// <summary>
    /// 
    /// Goal is completed.
    /// </summary>
    Completed = 2,
    /// <summary>
    /// Goal is cancelled.
    /// </summary>
    Cancelled = 3,
    /// <summary>
    /// Hold is a state where the goal is temporarily paused or on hold.
    /// </summary>
    Hold = 4
}

public enum InvestmentCategory
{
    /// <summary>
    /// Represents a category for stocks.
    /// </summary>
    Stocks = 0,
    /// <summary>
    /// Represents a category for bonds.
    /// </summary>
    Bonds = 1,
    /// <summary>
    /// Represents a category for mutual funds.
    /// </summary>
    MutualFunds = 2,
    /// <summary>
    /// Represents a category for real estate.
    /// </summary>
    RealEstate = 3,
    /// <summary>
    /// Represents a category for cryptocurrencies.
    /// </summary>
    Cryptocurrency = 4,

    /// <summary>
    /// Gold represents a category for investments in gold.
    /// </summary>
    Gold = 5,

    /// <summary>
    /// Silver represents a category for investments in silver.
    /// </summary>
    Silver = 6,

    /// <summary>
    /// FixedDeposit represents a category for fixed deposit investments.
    /// </summary>
    FixedDeposit = 7,

}

