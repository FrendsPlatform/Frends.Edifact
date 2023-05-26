using Frends.Edifact.ConvertToJson.Definitions;
using NUnit.Framework;
using System.IO;
using System.Threading;
using from = Frends.Edifact.CreateFromJson;

namespace Frends.Edifact.ConvertToJson.Tests;
internal static class TestHelpers
{
    internal static string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }

    internal static string ConvertToJsonAndBack(string testData, bool createUnbHeader)
    {
        var jsonResult = Edifact.ConvertToJson(
            new Input { InputEdifact = testData });

        var ediResult = from.Edifact.CreateFromJson(
            new from.Definitions.Input()
            {
                CreateUNBHeader = createUnbHeader,
                Json = jsonResult.Json
            },
            CancellationToken.None
        );

        return ediResult.Edifact;
    }
}