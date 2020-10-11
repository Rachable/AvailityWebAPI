using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
        public IActionResult Get([FromQuery] CustomerParameters customerParameters)
        {
            try
            {
                var customers = _service.GetAllCustomers(customerParameters);

                var metadata = new
                {
                    customers.TotalCount,
                    customers.PageSize,
                    customers.CurrentPage,
                    customers.TotalPages,
                    customers.HasNext,
                    customers.HasPrevious
                };

                if (Response != null)
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return customers.Count() > 0 ? (IActionResult)Ok(customers) : (IActionResult)NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception on Get All");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var customer = _service.GetById(id);
                return customer != null ? (IActionResult)Ok(customer) : (IActionResult)NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception on Get By Id");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newId = _service.Add(customer);
                var newCustomer = _service.GetById(newId);

                return CreatedAtAction("Get", new { id = newCustomer.Id }, newCustomer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception on Post");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return _service.Update(customer) ? (IActionResult)NoContent() : (IActionResult)NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception on Put");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return _service.Remove(id) ? (IActionResult)NoContent() : (IActionResult)NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception on Delete");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
