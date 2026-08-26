using Frends.Edifact.ConvertToJson.Definitions;
using Frends.Edifact.CreateFromJson.Definitions;
using NUnit.Framework;
using System.IO;
using System.Threading;
using From = Frends.Edifact.CreateFromJson;

namespace Frends.Edifact.ConvertToJson.Tests;
internal static class TestHelpers
{
    internal static string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }

    internal static Definitions.Options  DefaultOptions()
    {
        return new Definitions.Options { ThrowErrorOnFailure = true };
    }

    internal static string ConvertToJsonAndBack(string testData, bool createUnbHeader)
    {
        var jsonResult = Edifact.ConvertToJson(
            new Definitions.Input { InputEdifact = testData }, DefaultOptions(), CancellationToken.None);

        var ediResult = From.Edifact.CreateFromJson(
            new From.Definitions.Input()
            {   
                CreateUNBHeader = createUnbHeader,
                Json = jsonResult.Json
            },
            new From.Definitions.Options(),
            CancellationToken.None
        );

        return ediResult.Edifact;
    }
}