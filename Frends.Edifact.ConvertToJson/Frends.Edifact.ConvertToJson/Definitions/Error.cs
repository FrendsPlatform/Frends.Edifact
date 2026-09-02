using System;

namespace Frends.Edifact.ConvertToJson.Definitions;

/// <summary>
/// Error details for a failed operation.
/// </summary>
public class Error
{
    /// <summary>
    /// Error message.
    /// </summary>
    /// <example>Version D13131B is not supported. See inner exception for details.</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional info, typically the original exception.
    /// </summary>
    /// <example>null</example>
    public Exception AdditionalInfo { get; set; }
}
