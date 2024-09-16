using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp.API;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Tests
{
    public class OpraviloControllerTests
    {
        private readonly OpraviloController _controller;
        private readonly Mock<IOpraviloService> _mockService;

        public OpraviloControllerTests()
        {
            _mockService = new Mock<IOpraviloService>();
            _controller = new OpraviloController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithOpravila_WhenOpravilaExist()
        {
            // Arrange
            var opravila = new List<Opravilo>
            {
                new() { Id = 1, Naslov = "Test Naslov 1", Opis = "Test Opis 1" },
                new() { Id = 2, Naslov = "Test Naslov 2", Opis = "Test Opis 2" }
            };

            _mockService.Setup(service => service.GetAllOpravilaAsync())
                .ReturnsAsync(opravila);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var resultValue = Assert.IsType<List<Opravilo>>(result.Value);
            Assert.Equal(2, resultValue.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnNoContent_WhenNoOpravilaExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllOpravilaAsync())
                .ReturnsAsync(new List<Opravilo>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkWithOpravilo_WhenOpraviloExists()
        {
            // Arrange
            var opravilo = new Opravilo { Id = 1, Naslov = "Test Naslov", Opis = "Test Opis" };
            _mockService.Setup(service => service.GetOpraviloByIdAsync(1))
                .ReturnsAsync(opravilo);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(opravilo, result.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenOpraviloDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetOpraviloByIdAsync(1))
                .ReturnsAsync((Opravilo)null!);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenOpraviloIsCreated()
        {
            // Arrange
            var opravilo = new Opravilo { Naslov = "New Naslov", Opis = "New Opis" };
            var id = 1;
            _mockService.Setup(service => service.AddOpraviloAsync(opravilo))
                .ReturnsAsync(id);

            // Act
            var result = await _controller.Create(opravilo) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetById", result.ActionName);
            Assert.Equal(opravilo, result.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenOpraviloIsUpdated()
        {
            // Arrange
            var opravilo = new Opravilo { Id = 1, Naslov = "Updated Naslov", Opis = "Updated Opis" };

            _mockService.Setup(service => service.UpdateOpraviloAsync(opravilo))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(opravilo) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenOpraviloIdIsInvalid()
        {
            // Arrange
            var opravilo = new Opravilo { Id = 0, Naslov = "Invalid ID", Opis = "Invalid ID" };

            // Act
            var result = await _controller.Update(opravilo) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Opravilo mora imeti veljaven ID.", result.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenOpraviloIsDeleted()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteOpraviloAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}
