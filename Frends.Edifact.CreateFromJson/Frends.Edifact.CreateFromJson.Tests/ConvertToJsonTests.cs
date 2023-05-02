using NUnit.Framework;

namespace Frends.Edifact.CreateFromJson.Tests;

[TestFixture]
class ConvertToJsonTests : TestBase
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
        string testData = ReadTestFile(fileName);
        var jsonResult = ConvertToJsonAndBack(testData, false);
        Assert.AreEqual(testData.Replace("\r", "").Replace("\n", ""), jsonResult);
    }

    [Test]
    [TestCase("D93A_UNOC.txt")]
    public void WithHeaderTest(string fileName)
    {
        string testData = ReadTestFile(fileName);
        var jsonResult = ConvertToJsonAndBack(testData, true);
        Assert.AreEqual(testData.Replace("\r", "").Replace("\n", ""), jsonResult);
    }
}