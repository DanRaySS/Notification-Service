﻿namespace API_Service.Core.Domain.SharedKernel.Storage
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
