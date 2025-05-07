namespace Frends.Edifact.CreateFromXml.Tests;

using Frends.Edifact.CreateFromXml.Definitions;
using NUnit.Framework;
using System;
using System.Threading;

[TestFixture]
class CreateFromXmlTests
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
        var xmlResult = TestHelpers.ConvertToXmlAndBack(testData, false);
        Assert.AreEqual(testData.Replace("\r", string.Empty).Replace("\n", string.Empty), xmlResult);
    }

    [Test]
    [TestCase("D93A_UNOC.txt")]
    public void WithHeaderTest(string fileName)
    {
        string testData = TestHelpers.ReadTestFile(fileName);
        var xmlResult = TestHelpers.ConvertToXmlAndBack(testData, true);
        Assert.AreEqual(testData.Replace("\r", string.Empty).Replace("\n", string.Empty), xmlResult);
    }

    [Test]
    public void UnsupportedEdifactVersion()
    {
        string testData = @"<Edifact>
        <UNH>
            <MessageIdentifier_02>
                <MessageType_01>IFCSUM</MessageType_01>
                <MessageVersionNumber_02>D</MessageVersionNumber_02>
                <MessageReleaseNumber_03>13131B</MessageReleaseNumber_03>
            </MessageIdentifier_02>
        </UNH>
        </Edifact>";

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Edifact.CreateFromXml(
                    new Input { Xml = testData },
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
        string testData = @"<Edifact>
        <UNH>
            <MessageIdentifier_02>
                <MessageType_01>FOOBAR</MessageType_01>
                <MessageVersionNumber_02>D</MessageVersionNumber_02>
                <MessageReleaseNumber_03>96A</MessageReleaseNumber_03>
            </MessageIdentifier_02>
        </UNH>
    </Edifact>";

        var exception = Assert.Throws<ArgumentException>(() =>
        {
            Edifact.CreateFromXml(
                new Input { Xml = testData },
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
        string testData = @"<Edifact>
    <TSPAXLST>
      <UNH>
        <MessageReferenceNumber_01>PAXLST16</MessageReferenceNumber_01>
        <MessageIdentifier_02>
          <MessageType_01>PAXLST</MessageType_01>
          <MessageVersionNumber_02>D</MessageVersionNumber_02>
          <MessageReleaseNumber_03>03B</MessageReleaseNumber_03>
          <ControllingAgencyCoded_04>UN</ControllingAgencyCoded_04>
        </MessageIdentifier_02>
      </UNH>
      <BGM>
        <DOCUMENTMESSAGENAME_01>
          <Documentnamecode_01>10</Documentnamecode_01>
        </DOCUMENTMESSAGENAME_01>
        <DOCUMENTMESSAGEIDENTIFICATION_02>
          <Documentidentifier_01>LOCKKH04103101</Documentidentifier_01>
        </DOCUMENTMESSAGEIDENTIFICATION_02>
        <Messagefunctioncode_03>4</Messagefunctioncode_03>
      </BGM>
    </TSPAXLST>
    </Edifact>";
        var result = Edifact.CreateFromXml(
            new Input
            {
                Xml = testData,
                CreateUNBHeader = true,
                HeaderData = new HeaderData { ControlNumber = "FIND_ME" },
            },
            CancellationToken.None);
        Assert.IsTrue(result.Edifact.Contains("FIND_ME"), "Could not verify that UNB header was created.");
    }
}