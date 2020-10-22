using System;

namespace Data.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
