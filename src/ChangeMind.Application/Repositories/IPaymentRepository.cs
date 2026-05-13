namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id);
    Task<Payment?> GetByIdempotencyKeyAsync(Guid userId, string idempotencyKey);
    Task<IReadOnlyList<Payment>> GetByUserIdAsync(Guid userId);
    Task<Payment?> GetActivePackageByUserIdAsync(Guid userId);
    Task AddAsync(Payment payment);
}
