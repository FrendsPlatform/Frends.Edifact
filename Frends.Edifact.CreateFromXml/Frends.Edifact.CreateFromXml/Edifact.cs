namespace Frends.Edifact.CreateFromXml;

using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.Edifact;
using EdiFabric.Framework.Writers;
using Frends.Edifact.CreateFromXml.Definitions;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;


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
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Edifact.CreateFromXml)
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <param name="cancellationToken">Frends cancellation token.</param>
    /// <returns>object { string Edifact }</returns>
    public static Result CreateFromXml(
        [PropertyTab] Input input,
        CancellationToken cancellationToken)
    {
        var xml = XElement.Parse(input.Xml);
        var result = CreateEdifactFromXml(xml, input, cancellationToken);
        return new Result { Edifact = result };
    }

    private static string CreateEdifactFromXml(
        XElement xml, Input input, CancellationToken cancellationToken)
    {
        Edifabric.Activation.Activation.Activate();

        using var stream = new MemoryStream();

        // Why we cannot use shorthand using here and must use a using block? Here:
        // EdifactWriter is Disposable and we *must* call dispose to make sure that
        // it gets properly cleaned up AND to make sure that it flushes all writes
        // to the output MemoryStream. So we need to call Dispose before trying to
        // read data from the stream.
        // PS. Writer also has a Flush method, but it is marked as deprecated.
        using (var writer = new EdifactWriter(stream))
        {
            Type documentType = DetermineDocumentType(xml);
            WriteUnaIfAny(xml, writer);
            WriteOrCreateUnb(xml, writer, input);
            WriteUngIfAny(xml, writer);
            WriteEdiMessages(xml, documentType, writer, cancellationToken);

            // UNZ handling - we could not find a directly write UNZ with Edifabric.
            // The INTERCHANGE CONTROL COUNT will just default to 1 and
            // INTERCHANGE CONTROL REFERENCE will be taken from UNB.
        }

        return ReadStreamToEnd(stream);
    }

    private static string ReadStreamToEnd(MemoryStream stream)
    {
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.Default);
        return reader.ReadToEnd();
    }

    private static Type TypeFactory(XElement unh)
    {
        if (unh == null)
        {
            throw new ArgumentException("EDIFACT message does not contain UNH header");
        }

        var unhHeader = DeserializePart<UNH>(unh);
        var edifactVersion =
            $"{unhHeader.MessageIdentifier_02.MessageVersionNumber_02}" +
            $"{unhHeader.MessageIdentifier_02.MessageReleaseNumber_03}";
        var typeName = $"TS{unhHeader.MessageIdentifier_02.MessageType_01}";
        var assembly = LoadEdifactVersion(edifactVersion);
        var returnType = FindEdifactMessageType(edifactVersion, typeName, assembly);
        return returnType;
    }

    private static Type FindEdifactMessageType(string edifactVersion, string typeName, Assembly assembly)
    {
        var returnType = assembly.ExportedTypes.FirstOrDefault(x => x.Name == typeName);
        if (returnType == null)
        {
            throw new ArgumentException(
                $"Edifact message type {typeName} was not found in " +
                $"Edifact version {edifactVersion}");
        }

        return returnType;
    }

    private static Assembly LoadEdifactVersion(string edifactVersion)
    {
        var assemblyName = $"Frends.Edifabric.Templates.Edifact.{edifactVersion}";
        try
        {
            return Assembly.Load(assemblyName);
        }
        catch (Exception ex)
        {
            throw new ArgumentOutOfRangeException(
                $"Version {edifactVersion} is not supported. " +
                $"See inner exception for details.", ex);
        }
    }

    private static Type DetermineDocumentType(XElement ediXml)
    {
        var partUNH = ediXml.DescendantsAndSelf("UNH").First();
        var documentType = TypeFactory(partUNH);
        return documentType;
    }

    private static IEdiItem? DeserializeXElement(Type type, XElement xElement)
    {
        var serializer = new XmlSerializer(type);
        using var reader = xElement.CreateReader();
        var deserialized = (IEdiItem?)serializer.Deserialize(reader);
        return deserialized;
    }

    private static void WriteEdiMessages(
        XElement ediXml, Type documentType, EdifactWriter writer, CancellationToken cancellationToken)
    {
        var ediMessages = ediXml.DescendantsAndSelf(documentType.Name.ToString());
        foreach (var xElement in ediMessages)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var ediMessage = (EdiMessage?)DeserializeXElement(documentType, xElement);
            if (ediMessage != null) writer.Write(ediMessage);
        }
    }

    private static void WriteUngIfAny(XElement ediXml, EdifactWriter writer)
    {
        var partUNG = ediXml.DescendantsAndSelf("UNG");
        if (partUNG.Any())
        {
            var ung = DeserializePart<UNG>(partUNG.First());
            writer.Write(ung);
        }
    }

    private static void WriteOrCreateUnb(XElement ediXml, EdifactWriter writer, Input input)
    {
        var partUNB = ediXml.DescendantsAndSelf("UNB");
        if (partUNB.Any())
        {
            var unb = DeserializePart<UNB>(partUNB.First());
            writer.Write(unb);
        }
        else if (input.CreateUNBHeader)
        {
            writer.Write(SegmentBuilders.BuildUnb(input.HeaderData));
        }
    }

    private static void WriteUnaIfAny(XElement ediXml, EdifactWriter writer)
    {
        var partUNA = ediXml.DescendantsAndSelf("UNA");
        if (partUNA.Any())
        {
            var una = DeserializePart<UNA>(partUNA.First());
            writer.Write(una);
        }
    }

    private static T DeserializePart<T>(XElement xElement)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = xElement.CreateReader();
        var part = (T?)serializer.Deserialize(reader);
        if (part != null) return part;
        else throw new FormatException($"Could not deserialize {typeof(UNG).Name}");
    }
}
