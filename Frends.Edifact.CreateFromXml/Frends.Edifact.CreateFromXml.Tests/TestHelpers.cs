namespace Frends.Edifact.CreateFromXml.Tests;

using NUnit.Framework;
using System.IO;
using System.Threading;
using to = Frends.Edifact.ConvertToXml;

internal static class TestHelpers
{
    internal static string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }

    internal static string ConvertToXmlAndBack(string testData, bool createUnbHeader)
    {
        var xmlResult = to.Edifact.ConvertToXml(
            new to.Definitions.Input { InputEdifact = testData });

        var ediResult = Edifact.CreateFromXml(
            new Definitions.Input()
            {
                CreateUNBHeader = createUnbHeader,
                Xml = xmlResult.Xml,
            },
            CancellationToken.None);

        return ediResult.Edifact;
    }
}