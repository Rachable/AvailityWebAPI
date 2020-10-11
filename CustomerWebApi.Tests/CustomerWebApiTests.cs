using Xunit;
using CustomerWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace CustomerWebApi.Tests
{
    public class CustomerWebApiTests
    {

        CustomerController _controller;

        public CustomerWebApiTests()
        {
            _controller = new CustomerController(new NullLogger<CustomerController>(), new MockCustomerService());
        }

        [Fact]
        public void GetAllCustomersReturnsOkResult()
        {
            var okResult = _controller.Get(new CustomerParameters());
            Assert.IsType<OkObjectResult>(okResult);

            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<PagedList<Customer>>(objectResult.Value);
            Assert.Equal(5, items.Count);
        }

        [Theory]
        [InlineData(1, "Bob", "Burger")]
        [InlineData(2, "Tony", "Stark")]
        [InlineData(3, "Bat", "Man")]
        public void GetCustomerIDValidID(int id, string expectedFirstName, string expectedLastName)
        {
            var okResult = _controller.Get(id);
            Assert.IsType<OkObjectResult>(okResult);

            var objectResult = okResult as ObjectResult;
            var testCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.Equal(expectedFirstName, testCustomer.FirstName);
            Assert.Equal(expectedLastName, testCustomer.LastName);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetCustomerIDInvalidID(int id)
        {
            var okResult = _controller.Get(id);
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Theory]
        [InlineData("Wonder", "Woman", 6)]
        [InlineData("Green", "Lantern", 6)]
        [InlineData("Dare", "Devil", 6)]
        public void AddCustomerSuccessful(string firstName, string lastName, int expectedCount)
        {
            var testCustomer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName
            };

            var okResult = _controller.Post(testCustomer);
            Assert.IsType<CreatedAtActionResult>(okResult);

            var objectResult = okResult as ObjectResult;
            Assert.IsType<Customer>(objectResult.Value);
            var customer = objectResult.Value as Customer;
            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);

            okResult = _controller.Get(new CustomerParameters());
            objectResult = okResult as ObjectResult;
            var items = Assert.IsType<PagedList<Customer>>(objectResult.Value);
            Assert.Equal(expectedCount, items.Count);
        }

        [Theory]
        [InlineData(1, "Bob", "Burgers")]
        [InlineData(2, "Tony", "Bark")]
        [InlineData(3, "Cat", "Woman")]
        public void UpdateValidCustomer(int id, string firstName, string lastName)
        {
            var testCustomer = new Customer()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };

            var okResult = _controller.Put(testCustomer);
            Assert.IsType<NoContentResult>(okResult);
        }

        [Theory]
        [InlineData(17, "Bob", "Burgers")]
        [InlineData(43, "Tony", "Bark")]
        [InlineData(192, "Cat", "Woman")]
        public void UpdateInvalidCustomer(int id, string firstName, string lastName)
        {
            var testCustomer = new Customer()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };

            var okResult = _controller.Put(testCustomer);
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Theory]
        [InlineData(3, 4)]
        [InlineData(1, 4)]
        [InlineData(5, 4)]
        public void DeleteValidCustomer(int id, int expectedCount)
        {
            var okResult = _controller.Delete(id);
            Assert.IsType<NoContentResult>(okResult);

            okResult = _controller.Get(new CustomerParameters());
            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<PagedList<Customer>>(objectResult.Value);
            Assert.Equal(expectedCount, items.Count);
        }

        [Theory]
        [InlineData(38, 5)]
        [InlineData(72, 5)]
        [InlineData(67, 5)]
        public void DeleteInvalidCustomer(int id, int expectedCount)
        {
            var okResult = _controller.Delete(id);
            Assert.IsType<NotFoundResult>(okResult);

            okResult = _controller.Get(new CustomerParameters());
            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<PagedList<Customer>>(objectResult.Value);
            Assert.Equal(expectedCount, items.Count);
        }
    }
}
