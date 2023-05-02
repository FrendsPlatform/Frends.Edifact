namespace Frends.Edifact.CreateFromJson.Definitions;

/// <summary>
/// Result of creating Edifact from JSON.
/// </summary>
public class Result
{
    /// <summary>
    /// Result of creating Edifact from JSON.
    /// </summary>
    /// <example>
    /// UNB+UNOC:3+5500000000:30+ACME:ZZ+100926:1743+SE1234567++++++1'
    /// UNH+000001+INVOIC:D:93A:UN:EDIT30'
    /// BGM+380+891206500'
    /// ...
    /// UNZ+1+SE1234567'
    /// </example>
    public string Edifact { get; internal set; } = "";
}
