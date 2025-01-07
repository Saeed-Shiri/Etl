using FarasaEtl.Domain.Entities.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Domain.Repositories.Commands
{
    public interface ICommandRepository<T> where T : TargetBaseEntity
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
