using System.Net.Http.Json;
using System.Net;
using Xunit;
using FluentAssertions;
using Application.DTOs.Response;
using Domain.Entities;

namespace IntegrationTests.Controllers;

public class TopPairTests(IntegrationTestsWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    private const string BaseUrl = $"/api/employees/top-pair";

    [Fact]
    public async Task GetTopWorkedTogetherPair_WhenExists_ThenReturnOk()
    {
        // Arrange
        var empl1 = new Employee { Id = 143 };
        var empl2 = new Employee { Id = 218 };
        var proj1 = new Project { Id = 10 };
        var proj2 = new Project { Id = 12 };

        DbContext.Employees.Add(empl1);
        DbContext.Employees.Add(empl2);
        DbContext.Projects.Add(proj1);
        DbContext.Projects.Add(proj2);
        DbContext.EmployeeProjects.Add(new EmployeeProject { EmployeeId = empl1.Id, ProjectId = proj2.Id, DateFrom = new DateTime(2013, 11, 01), DateTo = new DateTime(2014, 01, 05) });
        DbContext.EmployeeProjects.Add(new EmployeeProject { EmployeeId = empl2.Id, ProjectId = proj1.Id, DateFrom = new DateTime(2012, 05, 16), DateTo = DateTime.Now });
        DbContext.EmployeeProjects.Add(new EmployeeProject { EmployeeId = empl1.Id, ProjectId = proj1.Id, DateFrom = new DateTime(2009, 01, 01), DateTo = new DateTime(2012, 05, 27) });
        await UnitOfWork.CompleteAsync(default);

        var uri = new Uri($"{BaseUrl}", UriKind.Relative);

        // Act
        var response = await Client.GetAsync(uri);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employeePair = await response.Content.ReadFromJsonAsync<List<EmployeePairDto>>();
        employeePair.Should().NotBeNullOrEmpty();
        employeePair.Count.Should().Be(1);
        employeePair[0].EmployeeId1.Should().Be(empl1.Id);
        employeePair[0].EmployeeId2.Should().Be(empl2.Id);
        employeePair[0].TotalDaysWorked.Should().Be(12);
    }

    [Fact]
    public async Task GetTopWorkedTogetherPair_WhenNotExists_ThenReturnOkWittEmptyList()
    {
        // Arrange
        var uri = new Uri($"{BaseUrl}", UriKind.Relative);

        // Act
        var response = await Client.GetAsync(uri);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var employeePair = await response.Content.ReadFromJsonAsync<List<EmployeePairDto>>();
        employeePair.Should().BeEmpty();
    }
}
