using System;

namespace Data.Models.Base
{
    public class BaseDeletableModel<T> : BaseEntity<T>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
