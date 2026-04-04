namespace ChangeMind.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Infrastructure.Data;

public class TrainingProgramRepository(ChangeMindDbContext context) : ITrainingProgramRepository
{

    public async Task AddAsync(TrainingProgram program)
    {
        await context.TrainingPrograms.AddAsync(program);
    }  
}
