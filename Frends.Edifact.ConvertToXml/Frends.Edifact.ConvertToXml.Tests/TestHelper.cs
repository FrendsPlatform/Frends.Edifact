namespace Frends.Edifact.ConvertToXml.Tests;

using Frends.Edifact.ConvertToXml.Definitions;
using NUnit.Framework;
using System.IO;

/// <summary>
/// Helper Test class for converting EDIFACT to XML.
/// </summary>
internal static class TestHelpers
{
    /// <summary>
    /// Helper Test class for converting EDIFACT to XML.
    /// </summary>
    internal static string ReadTestFile(string fileName)
    {
        return File.ReadAllText(string.Concat(TestContext.CurrentContext.TestDirectory, @"..\..\..\..\..\..\TestFiles\", fileName));
    }
}