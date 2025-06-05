using Domain.Entities.Bases;

namespace Domain.Entities;

public class EmployeeProject : BaseDeletableEntity<int>
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
