using Domain.Interfaces.Data;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Bases;

public abstract class BaseEntity<TKey> : IAuditInfo
{
    [Key]
    public TKey Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}