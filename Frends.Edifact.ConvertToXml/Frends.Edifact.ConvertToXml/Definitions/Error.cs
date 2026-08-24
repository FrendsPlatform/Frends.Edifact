namespace Frends.Edifact.ConvertToXml.Definitions;

using System;

/// <summary>
/// Error details for a failed operation.
/// </summary>
public class Error
{
    /// <summary>
    /// Error message.
    /// </summary>
    /// <example>Version D13131B is not supported.</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional error information, typically the exception.
    /// </summary>
    /// <example>null</example>
    public Exception? AdditionalInfo { get; set; }
}
