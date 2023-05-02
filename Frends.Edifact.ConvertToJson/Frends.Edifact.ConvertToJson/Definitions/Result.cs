namespace Frends.Edifact.ConvertToJson.Definitions;

/// <summary>
/// Result of converting Edifact to JSON.
/// </summary>
public class Result
{
    /// <summary>
    /// Result of converting Edifact to JSON.
    /// </summary>
    /// <example>
    /// { 
    ///     "Edifact": {
    ///         "UNB": { ... }
    ///         "TSINVOIC": { ... }
    ///         "UNZ": { ... }
    ///     }
    /// }
    /// </example>
    public string Json { get; internal set; } = "";
}
