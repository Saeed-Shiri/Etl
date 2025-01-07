using FarasaEtl.Domain.Entities.Target;
using FarasaEtl.Domain.Repositories.Commands;
using FarasaEtl.Infrastructure.Data.Target;

namespace FarasaEtl.Infrastructure.Repositories.Commands
{
    public class CommandRepository<T> : ICommandRepository<T> where T : TargetBaseEntity
    {
        protected readonly MonitoringBaseInfoDbContext _monitoringBaseInfoContext;

        public CommandRepository(MonitoringBaseInfoDbContext monitoringBaseInfoContext)
        {
            _monitoringBaseInfoContext = monitoringBaseInfoContext;
        }

        public Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _monitoringBaseInfoContext
                .AddRangeAsync(entities, cancellationToken);

            await _monitoringBaseInfoContext
                .SaveChangesAsync(cancellationToken);
        }
    }
}
