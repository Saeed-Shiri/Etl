using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Application.Interfaces
{
    public interface IRecurringJob
    {
        string CronExpression { get; }
        string JobId { get; }
        Task Execute(CancellationToken cancellationToken = default);

    }
}
