using Domain.Interfaces;
using Domain.Interfaces.Services;
using Infrastructure.Data.Services;
using Moq;

namespace UnitTests.Services.EmployeeProjectServiceTests;

public class EmployeeBaseTests
{
    protected Mock<IUnitOfWork> UnitOfWorkMock { get; } = new();
    protected IEmployeeProjectService EmplService;

    public EmployeeBaseTests()
    {
        EmplService = new EmployeeProjectService(UnitOfWorkMock.Object);
    }
}
