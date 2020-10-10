using System;
using System.Net;
using Xunit;
using CustomerWebApi;
using CustomerWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
using Services;
using System.Collections.Generic;
using Models;
using System.Security.Principal;

namespace CustomerWebApi.Tests
{
    public class TestCustomerApi
    {

        CustomerController _controller;

        public TestCustomerApi()
        {  
            _controller = new CustomerController(null, new TestCustomerService());
        }

        [Fact]
        public void GetAllCustomersReturnsOkResult()
        {

            var okResult = _controller.Get();
            Assert.IsType<OkObjectResult>(okResult);

            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<List<Customer>>(objectResult.Value);
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
            Assert.Equal(testCustomer.FirstName, expectedFirstName);
            Assert.Equal(testCustomer.LastName, expectedLastName);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(26)]
        [InlineData(37)]
        public void GetCustomerIDInvalidID(int id)
        {
            var okResult = _controller.Get(id);
            Assert.IsType<NotFoundResult>(okResult);
        }

        [Theory]
        [InlineData(6,"Wonder","Woman",6)]
        [InlineData(7,"Green","Lantern",6)]
        [InlineData(8,"Dare","Devil",6)]
        public void AddCustomerSuccessful(int id, string firstName, string lastName, int expectedCount)
        {
            var testCustomer = new Customer()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };

            var okResult = _controller.Post(testCustomer);
            Assert.IsType<CreatedAtActionResult>(okResult);

            var objectResult = okResult as ObjectResult;
            Assert.IsType<Customer>(objectResult.Value);

            okResult = _controller.Get();
            objectResult = okResult as ObjectResult;
            var items = Assert.IsType<List<Customer>>(objectResult.Value);
            Assert.Equal(expectedCount, items.Count);
        }

        [Theory]
        [InlineData(1,"Bob","Burgers")]
        [InlineData(2,"Tony","Bark")]
        [InlineData(3,"Cat","Woman")]
        public void UpdateValidCustomer(int id, string firstName, string lastName)
        {
            var testCustomer = new Customer()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };

            var okResult = _controller.Put(testCustomer);
            Assert.IsType<OkObjectResult>(okResult);


            var objectResult = okResult as ObjectResult;
            Assert.IsType<Customer>(objectResult.Value);

            okResult = _controller.Get(id);
            objectResult = okResult as ObjectResult;
            var returnCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.Equal(returnCustomer.FirstName, firstName);
            Assert.Equal(returnCustomer.LastName, lastName);
        }

        [Theory]
        [InlineData(17,"Bob","Burgers")]
        [InlineData(43,"Tony","Bark")]
        [InlineData(192,"Cat","Woman")]
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
        [InlineData(3,4)]
        [InlineData(1,4)]
        [InlineData(5,4)]
        public void DeleteValidCustomer(int id, int expectedCount)
        {

            var okResult = _controller.Delete(id);
            Assert.IsType<NoContentResult>(okResult);

            okResult = _controller.Get();
            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<List<Customer>>(objectResult.Value);
            Assert.Equal(expectedCount, items.Count);
        }

        [Theory]
        [InlineData(38,5)]
        [InlineData(72,5)]
        [InlineData(67,5)]
        public void DeleteInvalidCustomer(int id, int expectedCount)
        {

            var okResult = _controller.Delete(id);
            Assert.IsType<NotFoundResult>(okResult);

            okResult = _controller.Get();
            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<List<Customer>>(objectResult.Value);
            Assert.Equal(expectedCount, items.Count);
        }
    }
}
