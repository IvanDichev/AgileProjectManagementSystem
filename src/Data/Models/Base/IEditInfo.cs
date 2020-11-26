using System;

namespace Data.Models.Base
{
    public interface IEditInfo
    {
        DateTime AddedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
