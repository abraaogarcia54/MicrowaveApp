using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Exceptions;

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

    [Fact]
    public async Task CreateAsync_ShouldRejectDefaultHeatingCharacter()
    {
        var repository = new FakeHeatingProgramRepository();
        var service = new HeatingProgramService(repository);

        var act = () => service.CreateAsync(new CreateHeatingProgramRequest(
            "Arroz",
            "Arroz cozido",
            90,
            8,
            '.',
            null));

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectDuplicatedHeatingCharacter()
    {
        var repository = new FakeHeatingProgramRepository();
        repository.Programs.Add(HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar estouros."));
        var service = new HeatingProgramService(repository);

        var act = () => service.CreateAsync(new CreateHeatingProgramRequest(
            "Arroz",
            "Arroz cozido",
            90,
            8,
            '*',
            null));

        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage("*já está em uso*");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPresetAndCustomPrograms()
    {
        var repository = new FakeHeatingProgramRepository();
        repository.Programs.Add(HeatingProgram.CreatePreset(
            1,
            "Pipoca",
            "Pipoca (de micro-ondas)",
            180,
            7,
            '*',
            "Observar estouros."));
        var service = new HeatingProgramService(repository);
        await service.CreateAsync(new CreateHeatingProgramRequest("Arroz", "Arroz cozido", 90, 8, '#', null));

        var programs = await service.GetAllAsync();

        programs.Should().HaveCount(2);
        programs.Should().Contain(program => program.IsPresent);
        programs.Should().Contain(program => !program.IsPresent);
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
