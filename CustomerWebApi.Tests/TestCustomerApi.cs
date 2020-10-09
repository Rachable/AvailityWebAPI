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

namespace CustomerWebApi.Tests
{
    public class TestCustomerApi
    {

        CustomerController _controller;

        public TestCustomerApi()
        {  
            _controller = new CustomerController(new TestCustomerService());
        }

        [Fact]
        public void GetAllCustomersReturnsOkResult()
        {

            var okResult = _controller.Get();
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsNotType<NotFoundResult>(okResult);

            var objectResult = okResult as ObjectResult;
            var items = Assert.IsType<List<Customer>>(objectResult.Value);
            Assert.Equal(5, items.Count);
            
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetCustomerIDValidID(int id)
        {
            var okResult = _controller.Get(id);
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsNotType<NotFoundResult>(okResult);

            var objectResult = okResult as objectResult;
            Assert.IsType<CustomerWebApi>(objectResult.Value);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(26)]
        [InlineData(37)]
        public void GetCustomerIDInvalidID(int id)
        {
            var okResult = _controller.Get(id);
            Assert.IsNotType<OkObjectResult>(okResult);
            Assert.IsType<NotFoundResult>(okResult);
        }
    }
}
