using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace findox.Domain.Models.Database
{
    public abstract class Entity 
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}