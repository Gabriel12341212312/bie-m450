using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BIE.E2ETests;

[TestClass]
public class LearnerE2ETests : BrowserTestBase
{
    [TestMethod]
    public void TouristAt30DaysRemainsApprovedAfterReload()
    {
        Fill("Sam Browser", "Tourismus", "30");
        Driver.FindElement(By.Id("submit")).Click();

        Wait.Until(d => d.FindElement(By.Id("result")).Displayed);
        var result = Driver.FindElement(By.Id("result"));
        StringAssert.Contains(result.Text, "GENEHMIGT");

        var caseId = result.GetDomAttribute("data-application-id");
        Assert.IsFalse(string.IsNullOrWhiteSpace(caseId), "Das Ergebnis muss eine Antrags-ID enthalten.");

        Driver.Navigate().Refresh();

        var row = Wait.Until(d => d.FindElements(By.CssSelector("#application-list tr"))
            .FirstOrDefault(r => r.GetDomAttribute("data-id") == caseId));

        var cells = row.FindElements(By.TagName("td"));
        Assert.AreEqual("Sam Browser", cells[1].Text);
        Assert.AreEqual("Orion", cells[2].Text);
        Assert.AreEqual("30 Tage", cells[3].Text);
        Assert.AreEqual("GENEHMIGT", cells[4].Text);
    }
}
