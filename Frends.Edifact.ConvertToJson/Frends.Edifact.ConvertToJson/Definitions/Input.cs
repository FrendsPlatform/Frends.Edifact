using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Edifact.ConvertToJson.Definitions;

/// <summary>
/// Input for converting Edifact native document to other formats.
/// </summary>
public class Input
{
    /// <summary>
    /// Input edifact.
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
    public string InputEdifact { get; set; } = "";

    /// <summary>
    /// Allow Edifact messages with missing UNB header.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool AllowMissingUNB { get; set; } = true;
}