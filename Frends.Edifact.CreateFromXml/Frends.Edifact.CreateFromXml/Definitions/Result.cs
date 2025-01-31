namespace Frends.Edifact.CreateFromXml.Definitions;

/// <summary>
/// Result of creating Edifact from XML.
/// </summary>
public class Result
{
    /// <summary>
    /// Result of creating Edifact from XML.
    /// </summary>
    /// <example>
    /// UNB+UNOB:1+:01+:16++FIND_ME'UNH+PAXLST16+PAXLST:D:03B:UN'BGM+10+LOCKKH04103101+4'UNT+3+PAXLST16'UNZ+1+FIND_ME'
    /// </example>
    public string Edifact { get; internal set; } = string.Empty;
}
