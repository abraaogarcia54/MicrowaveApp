using MicrowaveApp.Application.DTOs;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Application.Services;
using MicrowaveApp.Domain.Entities;
using MicrowaveApp.Domain.Enums;

namespace MicrowaveApp.Application.Tests;

public class MicrowaveServiceTests
{
    [Fact]
    public async Task QuickStartAsync_ShouldCreateDefaultHeatingSession()
    {
        var service = CreateService();

        var session = await service.QuickStartAsync();

        session.TotalTimeInSeconds.Should().Be(30);
        session.Power.Should().Be(10);
        session.HeatingChar.Should().Be('.');
        session.Status.Should().Be(HeatingStatus.Heating);
    }

    [Fact]
    public async Task StartAsync_WithoutPower_ShouldUseDefaultPower()
    {
        var service = CreateService();

        var session = await service.StartAsync(new StartHeatingRequest(45));

        session.TotalTimeInSeconds.Should().Be(45);
        session.Power.Should().Be(10);
    }

    [Fact]
    public async Task StartAsync_WithoutTimeAndPower_ShouldQuickStart()
    {
        var service = CreateService();

        var session = await service.StartAsync(new StartHeatingRequest());

        session.TotalTimeInSeconds.Should().Be(30);
        session.Power.Should().Be(10);
    }

    [Fact]
    public async Task StartAsync_WithTimeBetweenSixtyAndOneHundred_ShouldReturnMinuteDisplay()
    {
        var service = CreateService();

        var session = await service.StartAsync(new StartHeatingRequest(90, 5));

        session.TotalTimeDisplay.Should().Be("1:30");
        session.RemainingTimeDisplay.Should().Be("1:30");
    }

    [Fact]
    public async Task StartAsync_WhenHeating_ShouldAddThirtySeconds()
    {
        var service = CreateService();
        await service.StartAsync(new StartHeatingRequest(20, 5));

        var session = await service.StartAsync(new StartHeatingRequest(10, 2));

        session.TotalTimeInSeconds.Should().Be(50);
        session.Power.Should().Be(5);
    }

    [Fact]
    public async Task StartAsync_WhenPaused_ShouldResume()
    {
        var service = CreateService();
        await service.StartAsync(new StartHeatingRequest(20, 5));
        await service.PauseOrCancelAsync();

        var session = await service.StartAsync(new StartHeatingRequest(10, 2));

        session.Status.Should().Be(HeatingStatus.Heating);
        session.TotalTimeInSeconds.Should().Be(20);
        session.Power.Should().Be(5);
    }

    [Fact]
    public async Task PauseOrCancelAsync_WhenPaused_ShouldCancelAndClearSession()
    {
        var service = CreateService();
        await service.StartAsync(new StartHeatingRequest(20, 5));
        var paused = await service.PauseOrCancelAsync();

        var cancelled = await service.PauseOrCancelAsync();

        paused!.Status.Should().Be(HeatingStatus.Paused);
        cancelled.Should().BeNull();
    }

    [Fact]
    public async Task AdvanceAsync_ShouldAppendHeatingPulseAndCompleteSession()
    {
        var service = CreateService();
        await service.StartAsync(new StartHeatingRequest(2, 3));

        var firstTick = await service.AdvanceAsync();
        var secondTick = await service.AdvanceAsync();

        firstTick.HeatingString.Should().Be("...");
        secondTick.HeatingString.Should().Be("... ... Aquecimento concluído");
        secondTick.Status.Should().Be(HeatingStatus.Completed);
    }

    [Fact]
    public async Task StartProgramAsync_ShouldUsePresetProgramData()
    {
        var service = CreateService();

        var session = await service.StartProgramAsync(1);

        session.TotalTimeInSeconds.Should().Be(180);
        session.Power.Should().Be(7);
        session.HeatingChar.Should().Be('*');
        session.IsPresentProgram.Should().BeTrue();
    }

    [Fact]
    public async Task StartAsync_WhenPresetProgramIsHeating_ShouldRejectAddTime()
    {
        var service = CreateService();
        await service.StartProgramAsync(1);

        var act = () => service.StartAsync(new StartHeatingRequest());

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*pré-definidos não permitem acréscimo*");
    }

    private static MicrowaveService CreateService()
    {
        return new MicrowaveService(
            new Microwave(),
            new FakeHeatingProgramRepository());
    }

    private sealed class FakeHeatingProgramRepository : IHeatingProgramRepository
    {
        private readonly List<HeatingProgram> _programs =
        [
            HeatingProgram.CreatePreset(
                1,
                "Pipoca",
                "Pipoca (de micro-ondas)",
                180,
                7,
                '*',
                "Observar estouros.")
        ];

        public Task<IReadOnlyCollection<HeatingProgram>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<HeatingProgram>>(_programs);
        }

        public Task<HeatingProgram?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_programs.FirstOrDefault(program => program.Id == id));
        }

        public Task AddAsync(HeatingProgram program, CancellationToken cancellationToken = default)
        {
            _programs.Add(program);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(HeatingProgram program, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
