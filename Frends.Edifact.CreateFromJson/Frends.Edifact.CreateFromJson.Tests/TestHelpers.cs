using NUnit.Framework;
using System.IO;
using System.Threading;
using to = Frends.Edifact.ConvertToJson;

namespace Frends.Edifact.CreateFromJson.Tests;
internal static class TestHelpers
{
    internal static string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }

    internal static string ConvertToJsonAndBack(string testData, bool createUnbHeader)
    {
        var jsonResult = to.Edifact.ConvertToJson(
            new to.Definitions.Input { InputEdifact = testData });

        var ediResult = Edifact.CreateFromJson(
            new Definitions.Input()
            {
                CreateUNBHeader = createUnbHeader,
                Json = jsonResult.Json
            },
            CancellationToken.None
        );

        return ediResult.Edifact;
    }
}