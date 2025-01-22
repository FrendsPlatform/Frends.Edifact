namespace Frends.Edifact.ConvertToXml;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Framework;
using EdiFabric.Framework.Readers;
using Frends.Edifact.ConvertToXml.Definitions;

/// <summary>
/// Task for converting Edifact to XML.
/// </summary>
public static class Edifact
{
    /// <summary>
    /// Converts an EDIFACT document to XML. Supports the following formats:
    /// D00A, D00B, D01A, D01B, D01C, D02A, D02B, D03A, D03B, D04A, D04B, D05A,
    /// D05B, D06A, D06B, D07A, D07B, D08A, D08B, D09A, D09B, D10A, D10B, D11A,
    /// D11B, D12A, D13A, D14A, D15A, D16A, D17A, D18A, D19A, D93A, D94A, D94B,
    /// D95A, D95B, D96A, D96B, D97A, D97B, D98A, D98B, D99A, D99B.
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <returns>string containing the XML representation of the EDIFACT document.</returns>
    public static Result ConvertToXml(
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
            throw new AggregateException("Error reading EDIFACT file", ex);
        }

        var returnValue = ConvertToXmlInternal(ediItems);
        return new Result { Xml = returnValue };
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

    private static string ConvertToXmlInternal(List<IEdiItem> ediItems)
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
        {
            throw new ArgumentNullException(nameof(ediItem));
        }

        var serializer = new XmlSerializer(ediItem.GetType());
        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

        // Add empty namespace to exclude namespaces from output
        namespaces.Add(string.Empty, string.Empty);

        using var ms = new MemoryStream();
        serializer.Serialize(ms, ediItem, namespaces);
        ms.Position = 0;
        return XDocument.Load(ms, LoadOptions.PreserveWhitespace);
    }
}
