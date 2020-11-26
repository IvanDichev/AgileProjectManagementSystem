using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Base
{
    public class BaseEntity<T> : IEditInfo, IBaseEntity<T>
    {
        [Key]
        public T Id { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
