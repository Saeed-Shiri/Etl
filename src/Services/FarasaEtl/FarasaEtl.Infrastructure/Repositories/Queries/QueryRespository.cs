

using Dapper;
using FarasaEtl.Domain.Entities.Source;
using FarasaEtl.Domain.Entities.Target;
using FarasaEtl.Domain.Repositories.Queries;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FarasaEtl.Infrastructure.Repositories.Queries
{
    public class QueryRespository<T> : IQueryRepository<T> where T : SourceBaseEntity
    {
        protected readonly IDbConnection _sourceConnection;

        public QueryRespository(IDbConnection sourceConnection)
        {
            _sourceConnection = sourceConnection;
        }

        public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            
        }
    }
}
