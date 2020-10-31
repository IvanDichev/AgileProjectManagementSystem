using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Base
{
    public class BaseDeletableModel<T> : BaseModel<T>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
