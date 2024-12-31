using NUnit.Framework;
using System.IO;
using System.Threading;

namespace Frends.Edifact.CreateFromXml.Tests;
internal static class TestHelpers
{
    internal static string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }
}