using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoService.Api.Controllers;
using TodoService.Core.Exceptions;
using TodoService.Core.Interfaces;
using TodoService.Core.Models;
using Xunit;

namespace TodoService.Api.UnitTests.Controllers
{
    public class TodoItemsControllersTests
    {
        private readonly Mock<ITodoItemRepository> _mockRepository;
        private readonly TodoItemsController _controller;
        private readonly string _toDoId;
        private TodoItem _savedTodoItem;

        public TodoItemsControllersTests()
        {
            _toDoId = Guid.NewGuid().ToString();
            _savedTodoItem = null;
            _mockRepository = new Mock<ITodoItemRepository>();
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                .Returns(Task.CompletedTask)
                .Callback<TodoItem>(x => _savedTodoItem = x);
            _controller = new TodoItemsController(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateToDo_WhenValid_ReturnsGuid()
        {
            // Arrange
            var newToDo = new TodoItem {Id = _toDoId, Name = "fake item Id"};

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync(newToDo);

            // Act
            var result = await _controller.CreateItem(newToDo);

            // Assert
            var createdAtActionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<TodoItem>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task CreateToDo_WhenValid_AddsCorrectToDo()
        {
            // Arrange
            var newToDo = new TodoItem {Id = _toDoId, Name = "fake item Id"};

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync((TodoItem x) => { return x; })
                .Callback<TodoItem>(x => _savedTodoItem = x);

            // Act
            var result = await _controller.CreateItem(newToDo);

            // Assert
            Assert.Equal(newToDo.Id, _savedTodoItem.Id);
            Assert.Equal(newToDo.Name, _savedTodoItem.Name);
        }

        [Fact]
        public async Task CreateToDo_WhenUserIdIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "Missing Id");
            var newToDoDto = new TodoItem {Id = "", Name = "fake item Id"};

            // Act
            var result = await _controller.CreateItem(newToDoDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetToDo_WithNonExistingToDoId_ShouldReturnNotFound()
        {
            // Arrange         
            _mockRepository.Setup(repo => repo.GetByIdAsync(_toDoId))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var result = await _controller.GetItem(_toDoId);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(_toDoId, notFoundObjectResult.Value);
        }

        [Fact]
        public async Task GetToDo_WithValidId_ReturnsOk()
        {
            // Arrange
            var toDo = new TodoItem {Id = _toDoId};
            _mockRepository.Setup(repo => repo.GetByIdAsync(_toDoId))
                .Returns(Task.FromResult(toDo));

            // Act
            var result = await _controller.GetItem(_toDoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(okResult.Value, toDo);
            _mockRepository.Verify(repo => repo.GetByIdAsync(_toDoId), Times.Once);
        }

        [Fact]
        public async Task UpdateToDo_WhenReplacingName_ReturnOK()
        {
            // Arrange
            var toDo = new TodoItem
            {
                Id =  _toDoId,
                Name = "Original Name"
            };

            var updatedToDo = new TodoItem
            {
                Id = _toDoId,
                Name = "New Name"
            };


            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                .Returns(Task.FromResult(updatedToDo));


            // Act
            var result = await _controller.UpdateItem(toDo.Id, updatedToDo);
            var okResult = Assert.IsType<OkResult>(result);
            // Assert
            Assert.Equal(200,okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateToDo_WhenToDoIdDoesNotMatch_ReturnsBadRequest()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var result = await _controller.UpdateItem(_toDoId, new TodoItem());

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Null(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task UpdateToDo_WhenToDoIdNotPresent_ReturnsBadRequest()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                .ThrowsAsync(new EntityNotFoundException());

            // Act
            var result = await _controller.UpdateItem(null, new TodoItem());

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Null(notFoundObjectResult.Value);
        }
    



        [Fact]
        public async Task RemoveItem_WithItemId_RemovesItem()
        {
            // Arrange

            var existingToDo = new TodoItem {Id = _toDoId};

            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(existingToDo));

            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<TodoItem>()))
                .Returns(Task.FromResult(new TodoItem()));

            // Act
            var result = await _controller.RemoveItem(existingToDo.Id);

            //  Assert
            Assert.IsType<NoContentResult>(result);
        }

        
        [Fact]
        public async Task RemoveItem_WithWrongItemId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingToDoId = Guid.NewGuid().ToString();

            _mockRepository.Setup(repo => repo.GetByIdAsync(nonExistingToDoId))
                .Returns(Task.FromResult(_savedTodoItem));

   
            // Act
            var result = await _controller.RemoveItem(nonExistingToDoId);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(nonExistingToDoId, notFoundObjectResult.Value);
        }

    }
}
