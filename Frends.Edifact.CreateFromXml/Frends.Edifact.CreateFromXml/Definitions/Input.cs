namespace Frends.Edifact.CreateFromXml.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    /// Edifact in XML format. The XML format should be the same
    /// as produced by Frends when converting Edifact documents to
    /// XML.
    /// </summary>
    /// <example>
    ///TODO
    /// }</example>
    [DisplayFormat(DataFormatString = "Xml")]
    public string Xml { get; set; } = string.Empty;
}