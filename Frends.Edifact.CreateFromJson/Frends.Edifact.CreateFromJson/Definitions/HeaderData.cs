using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Edifact.CreateFromJson.Definitions;

/// <summary>
/// Data for generating a UNB header.
/// </summary>
public class HeaderData
{
    /// <summary>
    /// Interchange control reference
    /// </summary>
    /// <example>131</example>
    public string ControlNumber { get; set; } = "";

    /// <summary>
    /// Interchange sender identification
    /// </summary>
    /// <example>SENDER1</example>
    public string InterchangeSenderIdentification { get; set; } = "";

    /// <summary>
    /// Interchange sender internal identification
    /// </summary>
    /// <example>ABC</example>
    public string InterchangeSenderInternalIdentification { get; set; } = "";

    /// <summary>
    /// Interchange recipient identification
    /// </summary>
    /// <example>RECEPIENT1</example>
    public string InterchangeRecipientIdentification { get; set; } = "";

    /// <summary>
    /// Interchange recipient internal identification
    /// </summary>
    /// <example>ABC</example>
    public string InterchangeRecipientInternalIdentification { get; set; } = "";

    /// <summary>
    /// Date of preparation
    /// </summary>
    /// <example>991231</example>
    [DefaultValue("yyMMdd")]
    [DisplayFormat(DataFormatString = "Text")]
    public string DateOfPreparation { get; set; } = "";

    /// <summary>
    /// Time of preparation
    /// </summary>
    /// <example>0000</example>
    [DefaultValue("hhmm")]
    [DisplayFormat(DataFormatString = "Text")]
    public string TimeOfPreparation { get; set; } = "";
}