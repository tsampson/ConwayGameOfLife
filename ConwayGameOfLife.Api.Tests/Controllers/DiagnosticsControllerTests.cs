using ConwayGameOfLife.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ConwayGameOfLife.Api.Tests.Controllers;

public class DiagnosticsControllerTests
{
    [Test]
    public void Heartbeat_Get()
    {
        var diagnosticsController = new DiagnosticsController();
        var result = diagnosticsController.Get() as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
    }
}
