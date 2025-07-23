using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility.Helper;
public class MetaDetails
{
    public string Version { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public InternalURLs InternalURLs { get; set; } = new InternalURLs();
    public DefaultSettings DefaultSettings { get; set; } = new DefaultSettings();

}

public class InternalURLs
{
    public string Home { get; set; } = string.Empty;
    public string GoalList { get; set; } = string.Empty;
    public string GoalEntryList { get; set; } = string.Empty;
    public string GoalEntryAdd { get; set; } = string.Empty;
}

public class DefaultSettings
{
    public string View { get; set; } = "List";
    public int PageSize { get; set; } = 8;
}

