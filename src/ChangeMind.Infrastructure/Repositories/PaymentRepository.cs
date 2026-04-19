namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class PaymentRepository(ChangeMindDbContext context) : IPaymentRepository
{
    public async Task<Payment?> GetByIdAsync(Guid id)
        => await context.Payments.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Payment?> GetByIdempotencyKeyAsync(Guid userId, string idempotencyKey)
        => await context.Payments.FirstOrDefaultAsync(p =>
            p.UserId == userId && p.IdempotencyKey == idempotencyKey);

    public async Task AddAsync(Payment payment)
        => await context.Payments.AddAsync(payment);
}
