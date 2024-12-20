namespace Frends.Edifact.ConvertToXml.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Input
{
    /// <summary>
    /// Gets or sets EDIFACT input string to be converted to XML.
    /// </summary>
    /// <example>
    /// UNB+UNOC:3+5500000000:30+ACME:ZZ+100926:1743+SE1234567++++++1'
    /// UNH+000001+INVOIC:D:93A:UN:EDIT30'
    /// BGM+380+891206500'
    /// ...
    /// UNZ+1+SE1234567'
    /// </example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string InputEdifact { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether allow EDIFACT messages with missing UNB header.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool AllowMissingUNB { get; set; } = true;
}