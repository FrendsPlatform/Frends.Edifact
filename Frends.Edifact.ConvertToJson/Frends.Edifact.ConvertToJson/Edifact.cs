using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework;
using EdiFabric.Framework.Readers;
using Frends.Edifact.ConvertToJson.Definitions;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Frends.Edifact.ConvertToJson;

/// <summary>
/// Task for converting Edifact to JSON.
/// </summary>
public static class Edifact
{
    /// <summary>
    /// Converts an EDIFACT document to XML. Supports the following formats:
    /// D00A, D00B, D01A, D01B, D01C, D02A, D02B, D03A, D03B, D04A, D04B, D05A,
    /// D05B, D06A, D06B, D07A, D07B, D08A, D08B, D09A, D09B, D10A, D10B, D11A,
    /// D11B, D12A, D13A, D14A, D15A, D16A, D17A, D18A, D19A, D93A, D94A, D94B,
    /// D95A, D95B, D96A, D96B, D97A, D97B, D98A, D98B, D99A, D99B.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Edifact.ConvertToJson)
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <returns>object { string Json }</returns>
    public static Result ConvertToJson(
        [PropertyTab] Input input)
    {
        var xmlResult = ConvertEdifactToXml(input);
        XmlDocument doc = new();
        doc.LoadXml(xmlResult);
        var json = JsonConvert.SerializeXmlNode(doc);
        return new Result { Json = json };
    }

    private static Assembly AssemblyFactory(MessageContext messageContext)
    {
        try
        {
            var returnAssembly = Assembly.Load($"Frends.Edifabric.Templates.Edifact.{messageContext.Version}");
            return returnAssembly;
        }
        catch (Exception ex)
        {
            throw new ArgumentOutOfRangeException($"Version {messageContext.Version} is not supported. See inner exception for details.", ex);
        }


    }

    private static string ConvertEdifactToXml(
        [PropertyTab] Input input)
    {
        Edifabric.Activation.Activation.Activate();

        var edifactDocument = input.InputEdifact;

        EdifactReaderSettings edifactReaderSettings = new() { NoEnvelope = input.AllowMissingUNB };
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(edifactDocument));
        using var ediReader = new EdifactReader(stream, AssemblyFactory, edifactReaderSettings);
        var ediItems = ediReader.ReadToEnd().ToList();

        if (ediItems.OfType<ReaderErrorContext>().Any())
        {
            var ex = ediItems.OfType<ReaderErrorContext>().Select(x => x.Exception);
            throw new AggregateException("Error reading edi file", ex);
        }

        var returnValue = ConvertToXml(ediItems);
        return returnValue;
    }

    private static string ConvertToXml(List<IEdiItem> ediItems)
    {
        var xDocument = new XDocument();
        var root = new XElement("Edifact");
        xDocument.Add(root);
        // Serialize EdiItems and add to document
        var ediItemsXml = ediItems.Select(x => Serialize(x));
        root.Add(ediItemsXml.Select(x => x.Elements()));
        return xDocument.ToString();
    }

    private static XDocument Serialize(IEdiItem ediItem)
    {
        if (ediItem == null)
            throw new ArgumentNullException(nameof(ediItem));

        var serializer = new XmlSerializer(ediItem.GetType());
        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
        //add empty namespace to exclude namespaces from output
        namespaces.Add("", "");
        using var ms = new MemoryStream();
        serializer.Serialize(ms, ediItem, namespaces);
        ms.Position = 0;
        return XDocument.Load(ms, LoadOptions.PreserveWhitespace);
    }
}
