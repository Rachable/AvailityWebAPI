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
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
        {
            _logger = logger;
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
            var customer = _service.GetById(id);

            if (customer != null)
                return Ok(customer);

            return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCustomer = _service.Add(customer);

            return CreatedAtAction("Get", new {id = newCustomer.Id}, customer);
        }

        [HttpPut]
         public IActionResult Put([FromBody] Customer customer)
         {
             if (!ModelState.IsValid)
                 return BadRequest(ModelState);

             var newCustomer = _service.Update(customer);

             if (newCustomer == null)
                return NotFound();

             return Ok(newCustomer);
         }

         [HttpDelete("{id}")]
         public IActionResult Delete(int id)
         {
            if (_service.Remove(id))
                return NoContent();

            return NotFound();
         }
    }
}
