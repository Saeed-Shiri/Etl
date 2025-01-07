using FarasaEtl.Domain.Entities.Source;
using FarasaEtl.Domain.Entities.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Domain.Repositories.Queries
{
    public interface IQueryRepository<T> where T : SourceBaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    }
}
