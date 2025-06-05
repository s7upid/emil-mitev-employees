using Domain.Entities.Bases;

namespace Domain.Entities;

public class Project : BaseDeletableEntity<int>
{
    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
}
