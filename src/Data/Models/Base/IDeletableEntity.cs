using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Base
{
    interface IDeletableEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
