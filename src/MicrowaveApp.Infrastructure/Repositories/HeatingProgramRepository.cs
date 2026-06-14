using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MicrowaveApp.Infrastructure.Repositories;

public sealed class HeatingProgramRepository : IHeatingProgramRepository
{
    private readonly MicrowaveDbContext _dbContext;

    public HeatingProgramRepository(MicrowaveDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.HeatingPrograms
            .OrderByDescending(program => program.IsPresent)
            .ThenBy(program => program.Name)
            .ToArrayAsync(cancellationToken);
    }

    public Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.HeatingPrograms
            .FirstOrDefaultAsync(program => program.Id == id, cancellationToken);
    }

    public async Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        await _dbContext.HeatingPrograms.AddAsync(program, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
