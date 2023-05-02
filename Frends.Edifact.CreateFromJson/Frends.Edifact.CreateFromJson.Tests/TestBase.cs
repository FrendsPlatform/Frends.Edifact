using NUnit.Framework;
using System.IO;
using to = Frends.Edifact.ConvertToJson;

namespace Frends.Edifact.CreateFromJson.Tests;
internal class TestBase
{
    protected string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }

    protected string ConvertToJsonAndBack(string testData, bool createUnbHeader)
    {
        var jsonResult = to.Edifact.ConvertToJson(
            new to.Definitions.Input { InputEdifact = testData });

        var ediResult = Edifact.CreateFromJson(
            new Definitions.Input()
            {
                CreateUNBHeader = createUnbHeader,
                Json = jsonResult.Json
            }
        );

        return ediResult.Edifact;
    }
}