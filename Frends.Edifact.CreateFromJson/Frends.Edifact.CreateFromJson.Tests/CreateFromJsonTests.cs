using Frends.Edifact.CreateFromJson.Definitions;
using NUnit.Framework;
using System;
using System.Threading;

namespace Frends.Edifact.CreateFromJson.Tests;

[TestFixture]
class CreateFromJsonTests
{
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
    public void UnsupportedEdifactVersion()
    {
        string testData = @"{
""Edifact"":
{
    ""UNH"":
    {
        ""MessageIdentifier_02"":
        {
            ""MessageType_01"": ""IFCSUM"",
            ""MessageVersionNumber_02"": ""D"",
            ""MessageReleaseNumber_03"": ""13131B""
        }
    }
}";

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Edifact.CreateFromJson(
                new Input { Json = testData },
                CancellationToken.None);
        });
        Assert.NotNull(exception?.InnerException);

        // The file has Edifact version set to D13131B
        Assert.AreEqual(
            "Version D13131B is not supported. See inner exception for details.",
            exception?.Message);
    }

    [Test]
    public void UnsupportedMessageType()
    {
        string testData = @"{
""Edifact"":
{
    ""UNH"":
    {
        ""MessageIdentifier_02"":
        {
            ""MessageType_01"": ""FOOBAR"",
            ""MessageVersionNumber_02"": ""D"",
            ""MessageReleaseNumber_03"": ""96A""
        }
    }
}";

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            Edifact.CreateFromJson(
                new Input { Json = testData },
                CancellationToken.None);
        });

        // The file has Edifact message type was set to FOOBAR
        Assert.AreEqual(
            "Edifact message type TSFOOBAR was not found in Edifact version D96A",
            exception?.Message);
    }

    [Test]
    public void CreateUnbHeader()
    {
        string testData = TestHelpers.ReadTestFile("D93A_INVOIC - No UNB.json");
        var result = Edifact.CreateFromJson(
            new Input
            {
                Json = testData,
                CreateUNBHeader = true,
                HeaderData = new HeaderData { ControlNumber = "FIND_ME" }
            },
            CancellationToken.None);
        Assert.IsTrue(result.Edifact.Contains("FIND_ME"), "Could not verify that UNB header was created.");
    }
}