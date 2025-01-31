namespace Frends.Edifact.CreateFromXml.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Data for generating a UNB header.
/// </summary>
public class HeaderData
{
    /// <summary>
    /// Interchange control reference
    /// </summary>
    /// <example>131</example>
    public string ControlNumber { get; set; } = string.Empty;

    /// <summary>
    /// Interchange sender identification
    /// </summary>
    /// <example>SENDER1</example>
    public string InterchangeSenderIdentification { get; set; } = string.Empty;

    /// <summary>
    /// Interchange sender internal identification
    /// </summary>
    /// <example>ABC</example>
    public string InterchangeSenderInternalIdentification { get; set; } = string.Empty;

    /// <summary>
    /// Interchange recipient identification
    /// </summary>
    /// <example>RECEPIENT1</example>
    public string InterchangeRecipientIdentification { get; set; } = string.Empty;

    /// <summary>
    /// Interchange recipient internal identification
    /// </summary>
    /// <example>ABC</example>
    public string InterchangeRecipientInternalIdentification { get; set; } = string.Empty;

    /// <summary>
    /// Date of preparation
    /// </summary>
    /// <example>991231</example>
    [DefaultValue("yyMMdd")]
    [DisplayFormat(DataFormatString = "Text")]
    public string DateOfPreparation { get; set; } = string.Empty;

    /// <summary>
    /// Time of preparation
    /// </summary>
    /// <example>0000</example>
    [DefaultValue("hhmm")]
    [DisplayFormat(DataFormatString = "Text")]
    public string TimeOfPreparation { get; set; } = string.Empty;
}