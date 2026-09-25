namespace Frends.Edifact.ConvertToXml.Tests;

using System;
using System.Threading;
using Frends.Edifact.ConvertToXml.Definitions;
using NUnit.Framework;

/// <summary>
/// Test class for converting EDIFACT to XML.
/// </summary>
[TestFixture]
public class ConvertToXmlTests
{
    private static Options DefaultOptions() => new Options { ThrowErrorOnFailure = true };

    [Test]
    [TestCase("D01B_IFCSUM.txt")]
    public void XmlAllowMissingUnb(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);

        // First test that it throws an exception when UNB is missing
        Assert.Throws<AggregateException>(() =>
        {
            Edifact.ConvertToXml(
                new Input { InputEdifact = testData, AllowMissingUNB = false },
                DefaultOptions(),
                CancellationToken.None);
        });

        // Now test that same content gets parsed if we allow missing UNB
        var result = Edifact.ConvertToXml(
                new Input { InputEdifact = testData, AllowMissingUNB = true },
                DefaultOptions(),
                CancellationToken.None);
        Assert.NotNull(result?.Xml);
    }

    [Test]
    [TestCase("UnsupportedFormat.txt")]
    public void UnsupportedFormat(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);

        // First test that it throws an exception when format is unsupported
        var exception = Assert.Throws<AggregateException>(() =>
        {
            Edifact.ConvertToXml(
                new Input { InputEdifact = testData, AllowMissingUNB = true },
                DefaultOptions(),
                CancellationToken.None);
        });
        Assert.NotNull(exception);
        Assert.That(exception.Message, Contains.Substring("Version D13131B is not supported"));
    }

    [Test]
    [TestCase("D01B_INVOIC.txt")]
    [TestCase("D03B_PAXLST_PassengerList.txt")]
    [TestCase("D93A_INVOIC.txt")]
    [TestCase("D93A_UNOC.txt")]
    [TestCase("D96A_IFTMIN.txt")]
    [TestCase("D96A_IFTSTA.txt")]
    [TestCase("D96A_INVOIC.txt")]
    [TestCase("D96A_IFTMIN.txt")]
    [TestCase("D96A_INVOIC_2.txt")]
    [TestCase("D96A_ORDERS.txt")]
    [TestCase("D96A_ORDERS_2.txt")]
    [TestCase("D96A_ORDERS_DuplicateMessage.txt")]
    [TestCase("D96A_ORDERS_PurchaseOrder.txt")]
    [TestCase("D96A_ORDERS_PurchaseOrderCSV.txt")]
    [TestCase("D96A_ORDERS_PurchaseOrderInvalid.txt")]
    [TestCase("D96A_ORDERS_PurchaseOrders.txt")]
    [TestCase("D96A_ORDRSP.txt")]
    [TestCase("D96A_PRICAT.txt")]
    public void ValidConversionTest(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);

        var result = Edifact.ConvertToXml(
            new Input { InputEdifact = testData, AllowMissingUNB = true },
            DefaultOptions(),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.IsTrue(result.Success);
        Assert.IsTrue(result.Xml.Contains("<Edifact>"));
    }
}