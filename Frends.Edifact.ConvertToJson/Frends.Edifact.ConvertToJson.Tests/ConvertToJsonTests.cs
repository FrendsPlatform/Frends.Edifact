using System;
using Frends.Edifact.ConvertToJson.Definitions;
using NUnit.Framework;

namespace Frends.Edifact.ConvertToJson.Tests;

[TestFixture]
class ConvertToJsonTests
{
    [Test]
    [TestCase("D01B_IFCSUM.txt")]
    public void JsonAllowMissingUnb(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);

        // First test that it throws an exception when UNB is missing
        Assert.Throws<AggregateException>(() =>
        {
            Edifact.ConvertToJson(
                new Input { InputEdifact = testData, AllowMissingUNB = false });
        });

        // Now test that same content gets parsed if we allow missing UNB
        var result = Edifact.ConvertToJson(
                new Input { InputEdifact = testData, AllowMissingUNB = true });
        Assert.NotNull(result?.Json);
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
    public void NoHeaderTest(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);
        var jsonResult = TestHelpers.ConvertToJsonAndBack(testData, false);
        Assert.AreEqual(testData.Replace("\r", "").Replace("\n", ""), jsonResult);
    }

    [Test]
    [TestCase("D93A_UNOC.txt")]
    public void WithHeaderTest(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);
        var jsonResult = TestHelpers.ConvertToJsonAndBack(testData, true);
        Assert.AreEqual(testData.Replace("\r", "").Replace("\n", ""), jsonResult);
    }

    [Test]
    [TestCase("UnsupportedFormat.txt")]
    public void UnsupportedFormat(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);

        // First test that it throws an exception when UNB is missing
        var exception = Assert.Throws<AggregateException>(() =>
            {
                Edifact.ConvertToJson(
                    new Input { InputEdifact = testData, AllowMissingUNB = true });
            });
        Assert.NotNull(exception?.InnerException);
        // The file has Edifact version set to D13131B
        Assert.AreEqual(
            "Version D13131B is not supported. See inner exception for details.",
            exception?.InnerException?.Message);
    }
}