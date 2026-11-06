using System.Net;
using System.Net.Http.Json;
using BIE.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bie.IntegrationTests;

[TestClass]
public class LearnerIntegrationTests
{
    [TestMethod]
    public async Task TouristAt30DaysIsSavedAsApproved()
    {
        using var app = new BieFactory();
        using var client = app.CreateClient();

        var entry = new Entry("Sam Test", "Orion", "Tourismus", 30, false, false, false, true);

        var response = await client.PostAsJsonAsync("/api/cases", entry);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var records = await client.GetFromJsonAsync<List<Case>>("/api/cases");
        Assert.IsNotNull(records);
        Assert.AreEqual(1, records.Count);

        var savedCase = records[0];
        Assert.AreEqual(entry, savedCase.Applicant);
        Assert.IsTrue(savedCase.Id > 0);
        Assert.AreEqual("approved", savedCase.Decision.Status);
    }
}
