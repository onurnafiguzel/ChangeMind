namespace ChangeMind.Application.Repositories;

using ChangeMind.Domain.Entities;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id);
    Task AddAsync(Payment payment);
}
