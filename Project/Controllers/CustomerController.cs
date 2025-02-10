using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        // dependency injection of service interfaces
        public CustomerController(ICustomerService CustomerService)
        {
            _customerService = CustomerService;
        }

        /// <summary>
        /// Returns a list of Customers
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{CustomerDto},{CustomerDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Customer/List -> [{CustomerDto},{CustomerDto},..]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> ListCategories()
        {
            // empty list of data transfer object CustomerDto
            IEnumerable<CustomerDto> CustomerDtos = await _customerService.ListCustomers();
            // return 200 OK with CustomerDtos
            return Ok(CustomerDtos);
        }

        /// <summary>
        /// Returns a single Customer specified by its {id}
        /// </summary>
        /// <param name="id">The Customer id</param>
        /// <returns>
        /// 200 OK
        /// {CustomerDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Customer/Find/1 -> {CustomerDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<CustomerDto>> FindCustomer(int id)
        {

            var Customer = await _customerService.FindCustomer(id);

            // if the customer could not be located, return 404 Not Found
            if (Customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Customer);
            }
        }
        /// <summary>
        /// Updates a Customer
        /// </summary>
        /// <param name="id">The ID of the customer to update</param>
        /// <param name="CustomerDto">The required information to update the Customer (CustomerName, CustomerColor)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Customer/Update/4
        /// Request Headers: Content-Type: application/json
        /// Request Body: {CustomerDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, CustomerDto CustomerDto)
        {
            // {id} in URL must match CustomerId in POST Body
            if (id != CustomerDto.Cid)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _customerService.UpdateCustomer(CustomerDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return NoContent();

        }
        /// <summary>
        /// Adds a Customer
        /// </summary>
        /// <param name="CustomerDto">The required information to add the customer (CustomerName, Email, phone)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Customer/Find/{CustomerId}
        /// {CustomerDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Customer/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {CustomerDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Customer/Find/{CustomerId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Customer>> AddCustomer(CustomerDto CustomerDto)
        {
            ServiceResponse response = await _customerService.AddCustomer(CustomerDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Customer/FindCustomer/{response.CreatedId}", CustomerDto);
        }

        /// <summary>
        /// Deletes the Customer
        /// </summary>
        /// <param name="id">The id of the customer to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Customer/Delete/7
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            ServiceResponse response = await _customerService.DeleteCustomer(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }
    }
}
