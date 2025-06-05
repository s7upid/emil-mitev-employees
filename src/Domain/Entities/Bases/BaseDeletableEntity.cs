
using Domain.Interfaces.Data;

namespace Domain.Entities.Bases;

public abstract class BaseDeletableEntity<TKey> : BaseEntity<TKey>, IDeletableEntity
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}