using FluentAssertions;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Services;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Tests;

public class OpraviloServiceTests
{
    private readonly Mock<IOpraviloRepository> _mockRepository;
    private readonly IOpraviloService _opraviloService;

    public OpraviloServiceTests()
    {
        _mockRepository = new Mock<IOpraviloRepository>();
        _opraviloService = new OpraviloService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllOpravilaAsync_ShouldReturnAllopravila()
    {
        // Arrange
        var opravila = new List<Opravilo>
        {
            new() { Id = 1, Naslov = "Test 1", Opis = "Opis 1", Opravljeno = false },
            new() { Id = 2, Naslov = "Test 2", Opis = "Opis 2", Opravljeno = true }
        };
        _mockRepository.Setup(repo => repo.GetAllOpravilaAsync()).ReturnsAsync(opravila);

        // Act
        var result = await _opraviloService.GetAllOpravilaAsync();

        // Assert
        result.Should().BeEquivalentTo(opravila);
    }

    [Fact]
    public async Task GetOpraviloByIdAsync_ShouldReturnCorrectOpravilo()
    {
        // Arrange
        var opravilo = new Opravilo { Id = 1, Naslov = "Test", Opis = "Opis", Opravljeno = false };
        _mockRepository.Setup(repo => repo.GetOpraviloByIdAsync(1)).ReturnsAsync(opravilo);

        // Act
        var result = await _opraviloService.GetOpraviloByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(opravilo);
    }

    [Fact]
    public async Task AddOpraviloAsync_ShouldCallRepository()
    {
        // Arrange
        var opravilo = new Opravilo { Id = 3, Naslov = "Novo opravilo", Opis = "Opis" };

        // Act
        await _opraviloService.AddOpraviloAsync(opravilo);

        // Assert
        _mockRepository.Verify(repo => repo.AddOpraviloAsync(opravilo), Times.Once);
    }

    [Fact]
    public async Task UpdateOpraviloAsync_ShouldCallRepository()
    {
        // Arrange
        var opravilo = new Opravilo { Id = 1, Naslov = "Posodobljeno opravilo", Opis = "Posodobljen opis" };

        // Act
        await _opraviloService.UpdateOpraviloAsync(opravilo);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateOpraviloAsync(opravilo), Times.Once);
    }

    [Fact]
    public async Task DeleteOpraviloAsync_ShouldCallRepository()
    {
        // Act
        await _opraviloService.DeleteOpraviloAsync(1);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteOpraviloAsync(1), Times.Once);
    }

}