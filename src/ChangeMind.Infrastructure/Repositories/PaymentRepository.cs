namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Infrastructure.Data;

public class PaymentRepository(ChangeMindDbContext context) : IPaymentRepository
{
    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await context.Payments.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Payment payment)
    {
        await context.Payments.AddAsync(payment);
    }
}
