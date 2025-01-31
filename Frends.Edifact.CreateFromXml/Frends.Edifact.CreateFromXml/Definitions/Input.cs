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
    /// &lt;Edifact&gt;
    ///   &lt;TSPAXLST&gt;
    ///     &lt;UNH&gt;
    ///       &lt;MessageReferenceNumber_01&gt;PAXLST16&lt;/MessageReferenceNumber_01&gt;
    ///       &lt;MessageIdentifier_02&gt;
    ///         &lt;MessageType_01&gt;PAXLST&lt;/MessageType_01&gt;
    ///         &lt;MessageVersionNumber_02&gt;D&lt;/MessageVersionNumber_02&gt;
    ///         &lt;MessageReleaseNumber_03&gt;03B&lt;/MessageReleaseNumber_03&gt;
    ///         &lt;ControllingAgencyCoded_04&gt;UN&lt;/ControllingAgencyCoded_04&gt;
    ///       &lt;/MessageIdentifier_02&gt;
    ///     &lt;/UNH&gt;
    ///   &lt;/TSPAXLST&gt;
    /// &lt;/Edifact&gt;.
    /// </example>
    [DisplayFormat(DataFormatString = "Xml")]
    public string Xml { get; set; } = string.Empty;
}