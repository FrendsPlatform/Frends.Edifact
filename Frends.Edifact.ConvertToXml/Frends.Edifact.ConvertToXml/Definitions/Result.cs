namespace Frends.Edifact.ConvertToXml.Definitions;

/// <summary>
/// Result of converting Edifact to XML.
/// </summary>
public class Result
{
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
