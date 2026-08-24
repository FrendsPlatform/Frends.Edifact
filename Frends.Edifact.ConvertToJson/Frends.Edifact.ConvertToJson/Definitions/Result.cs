namespace Frends.Edifact.ConvertToJson.Definitions;

/// <summary>
/// Result of converting Edifact to JSON.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the operation completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; internal set; }

    /// <summary>
    /// Error details. Null when Success is true.
    /// </summary>
    /// <example>null</example>
    public Error Error { get; internal set; }

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
