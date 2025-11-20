using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PractTest.API.Controllers;
using PractTest.API.Shared;
using PractTest.Application.DTOs;
using PractTest.Application.Interfaces.Customers;

namespace PractTest.APITest
{
    public class CustomersControllerTest
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly Mock<ILogger<CustomersController>> _mockLoggerService;
        private readonly Mock<IValidator<CreateCustomerDto>> _mockValidator;
        private readonly CustomersController _controller;

        public CustomersControllerTest()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _mockLoggerService = new Mock<ILogger<CustomersController>>();
            _mockValidator = new Mock<IValidator<CreateCustomerDto>>();
            _controller = new CustomersController(_mockCustomerService.Object, _mockLoggerService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetAllCustomers_Returns200Ok_WhenCustomersExists()
        {
            // Arrange
            var customers = new List<CustomerDto>()
            {
                new CustomerDto() { Id = 1, Name = "Aaaa", Email = "a@a.com", LoyaltyPoints = 101 },
                new CustomerDto() { Id = 2, Name = "Bbbb", Email = "b@a.com", LoyaltyPoints = 102 }
            };
            _mockCustomerService
                .Setup(service => service.GetAllCustomerAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<APIResponse<IEnumerable<CustomerDto>>>(okResult.Value);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Customers retrieved successfully.", response.Message);
            Assert.NotNull(response.Data);
            Assert.Equal(2, ((List<CustomerDto>)response.Data).Count);
        }
    }
}
