using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsManagement.Core.Interfaces;
using ContactsManagement.Core.Models;
using ContatcsManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace ContactsManagement.API.Tests.Controller
{
    public class ContactControllerTests
    {
        private readonly Mock<IContactsManagementService> _mockService;
        private readonly Mock<ILogger<ContactController>> _mockLogger;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _mockService = new Mock<IContactsManagementService>();
            _mockLogger = new Mock<ILogger<ContactController>>();
            _controller = new ContactController(_mockLogger.Object, _mockService.Object);
        }
        [Fact]
        public async Task GetContacts_ReturnsOkResult_WithContacts()
        {
            // Arrange
            var contacts = new ResponseModel()
            {
                TotalNoOfContacts = 1,
                Contacts = new List<ContactsModel>
                {
                    new ContactsModel { Id  = 1, FirstName = "rsy", LastName = "tech", Email = "rsystem@example.com" }
                }
            };

            _mockService.Setup(s => s.GetAsync()).ReturnsAsync(contacts);

            // Act
            var result = await _controller.GetContacts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.Equal(1, returnValue.TotalNoOfContacts);
        }
        [Fact]
        public async Task GetContacts_ReturnsNotFound_WhenNoContacts()
        {
            // Arrange
            _mockService.Setup(s => s.GetAsync());

            // Act
            var result = await _controller.GetContacts();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAsync_ReturnsOkResult_WhenInsertSuccessful()
        {
            // Arrange
            var contact = new ContactsModel() { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };
            _mockService.Setup(s => s.CreateAsync(contact)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAsync(contact);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }
        [Fact]
        public async Task CreateAsync_ReturnsBadRequest_WhenModelIsNull()
        {
            // Act
            var result = await _controller.CreateAsync(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public async Task UpdateAsync_ReturnsOkResult_WhenUpdateSuccessful()
        {
            // Arrange
            var contact = new ContactsModel() { Id = 1, FirstName = "Tech", LastName = "Net", Email = "Core.doe@example.com" };
            _mockService.Setup(s => s.UpdateAsync(contact)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateAsync(contact);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }
        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenModelIsNull()
        {
            // Act
            var result = await _controller.UpdateAsync(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public async Task DeleteAsync_ReturnsOkResult_WhenDeleteSuccessful()
        {
            // Arrange
            var id = 1;
            _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAsync(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.DeleteAsync(0);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

    }
}
