using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Application.Tests;

public class HeatingProgramServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldPersistProgramAndReturnResponse()
    {
        var repository = new FakeHeatingProgramRepository();
        var service = new HeatingProgramService(repository);

        var response = await service.CreateAsync(new CreateHeatingProgramRequest(
            "Arroz",
            "Arroz cozido",
            90,
            8,
            '#',
            "Mexer na metade do tempo."));

        response.Name.Should().Be("Arroz");
        response.Power.Should().Be(8);
        repository.Programs.Should().HaveCount(1);
    }

    private sealed class FakeHeatingProgramRepository : IHeatingProgramRepository
    {
        public List<HeatingProgram> Programs { get; } = [];

        public Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<HeatingProgram>>(Programs);
        }

        public Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Programs.FirstOrDefault(program => program.Id == id));
        }

        public Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
        {
            Programs.Add(program);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
