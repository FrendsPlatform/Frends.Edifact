namespace Frends.Edifact.ConvertToXml.Definitions;

/// <summary>
/// Result of converting Edifact to XML.
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
    public Error? Error { get; internal set; }

    /// <summary>
    /// Gets result of converting Edifact to XML.
    /// </summary>
    /// <example>
    /// <Edifact>
    ///     <UNB>
    ///         ...
    ///     </UNB>
    /// </Edifact>
    /// </example>
    public string Xml { get; internal set; } = string.Empty;
}
