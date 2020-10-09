using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Services;

namespace CustomerWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        Customer[] customers = new Customer[]
        {
            new Customer {Id = 1, FirstName = "Bob", LastName = "Burger", DateBecameCustomer = new DateTime(2019, 1, 20)},
            new Customer {Id = 2, FirstName = "Tony", LastName = "Stark", DateBecameCustomer = new DateTime()},
            new Customer {Id = 3, FirstName = "Bat", LastName = "Man", DateBecameCustomer = new DateTime(2020, 3, 15)},
            new Customer {Id = 4, FirstName = "Chicken", LastName = "Little", DateBecameCustomer = new DateTime(2019, 5, 7)},
            new Customer {Id = 5, FirstName = "Mary", LastName = "Poppins", DateBecameCustomer = new DateTime(2018, 1, 1)}
        };

        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
            _service = new TestCustomerService();
        }

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            Customer customer = _service.GetById(id);

            if (customer != null)
                return Ok(customer);
            else
                return NotFound();

        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer newCustomer = _service.Add(customer);

            return CreatedAtAction("Get", new {id = newCustomer.Id}, customer);
        }
    }
}
