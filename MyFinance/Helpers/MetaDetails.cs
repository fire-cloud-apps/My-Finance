namespace MyFinance.Helpers;
public class MetaDetails
{
    public string Version { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public InternalURLs InternalURLs { get; set; } = new InternalURLs();

}

public class InternalURLs
{
    public string Home { get; set; } = string.Empty;
    public string GoalList { get; set; } = string.Empty;
    public string GoalEntryList { get; set; } = string.Empty;
    public string GoalEntryAdd { get; set; } = string.Empty;
}

