using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Moq;
using UnitTests.Fixtures;
using Xunit;

namespace UnitTests.Services.EmployeeProjectServiceTests;

public class GetAllEmployeePairsSortedTests : EmployeeBaseTests
{
    [Fact]
    public async Task GetTopWorkedTogetherPair_WhenExists_ThenReturnList()
    {
        // Arrange
        var data = new List<EmployeeProject>
        {
            new() { EmployeeId = 143, ProjectId = 12, DateFrom = new DateTime(2013, 11, 01), DateTo = new DateTime(2014, 01, 05) },
            new() { EmployeeId = 218, ProjectId = 10, DateFrom = new DateTime(2012, 05, 16), DateTo = DateTime.Now },
            new() { EmployeeId = 143, ProjectId = 10, DateFrom = new DateTime(2009, 01, 01), DateTo = new DateTime(2012, 05, 27) }
        };

        UnitOfWorkMock.Setup(
             x => x.EmployeeProjects.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        // Act
        var result = await EmplService.GetAllEmployeePairsSortedAsync(default);

        // Assert
        using (new AssertionScope())
        {
            result.IsFailed.Should().BeFalse();
            result.Value.Should().NotBeNullOrEmpty();
            result.Value.Count.Should().Be(1);
            UnitOfWorkMock.Verify(x => x.EmployeeProjects.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Fact]
    public async Task GetTopWorkedTogetherPair_WhenNotExists_ThenReturnEmptyList()
    {
        // Arrange
        var fixture = RecursionFixture.CreateFixtureWithOmitOnRecursion();
        var data = fixture.Create<IList<EmployeeProject>>();

        UnitOfWorkMock.Setup(
             x => x.EmployeeProjects.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data);

        // Act
        var result = await EmplService.GetAllEmployeePairsSortedAsync(default);

        // Assert
        using (new AssertionScope())
        {
            result.IsFailed.Should().BeFalse();
            result.Value.Should().BeNullOrEmpty();
            UnitOfWorkMock.Verify(x => x.EmployeeProjects.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
