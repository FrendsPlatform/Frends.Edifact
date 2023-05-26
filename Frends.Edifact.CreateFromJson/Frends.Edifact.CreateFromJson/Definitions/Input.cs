using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Edifact.CreateFromJson.Definitions;

/// <summary>
/// Input for converting Edifact native document to other formats.
/// </summary>
public class Input
{
    /// <summary>
    /// Should the UNB header be created, if not included in input document.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(false)]
    public bool CreateUNBHeader { get; set; }

    /// <summary>
    /// UNB header data. Used if CreateUnbHeader is set to true.
    /// </summary>
    [UIHint(nameof(CreateUNBHeader), "", true)]
    public HeaderData HeaderData { get; set; } = new HeaderData();

    /// <summary>
    /// Edifact in JSON format. The JSON format should be the same
    /// as produced by Frends when converting Edifact documents to
    /// JSON.
    /// </summary>
    /// <example>
    /// { 
    ///     "Edifact": {
    ///         "UNB": { ... }
    ///         "TSINVOIC": { ... }
    ///         "UNZ": { ... }
    ///     }
    /// }</example>
    [DisplayFormat(DataFormatString = "Json")]
    public string Json { get; set; } = "";
}