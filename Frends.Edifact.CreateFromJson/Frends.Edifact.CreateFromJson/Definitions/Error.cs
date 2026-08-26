namespace Frends.Edifact.CreateFromJson.Definitions;

/// <summary>
/// Error details returned when the task fails.
/// </summary>
public class Error
{
    /// <summary>
    /// The error message.
    /// </summary>
    /// <example>Could not deserialize input JSON.</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional information about the error, typically the original exception.
    /// </summary>
    /// <example>null</example>
    public object? AdditionalInfo { get; set; }
}
