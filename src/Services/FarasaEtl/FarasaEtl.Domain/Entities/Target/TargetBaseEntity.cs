using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Domain.Entities.Target
{
    public abstract class TargetBaseEntity
    {
        public long Id { get; protected set; }
        public DateTime? CreatedDate { get; set; }
    }
}
