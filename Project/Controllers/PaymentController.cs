using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        // dependency injection of service interfaces
        public PaymentController(IPaymentService PaymentService)
        {
            _paymentService = PaymentService;
        }
        /// <summary>
        /// Returns a list of Payments
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{PaymentDto},{PaymentDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Payment/List -> [{PaymentDto},{PaymentDto},..]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> ListPayments()
        {
            // empty list of data transfer object PaymentDto
            IEnumerable<PaymentDto> PaymentDtos = await _paymentService.ListPayments();
            // return 200 OK with PaymentDtos
            return Ok(PaymentDtos);
        }
        /// <summary>
        /// Returns a single Payment specified by its {id}
        /// </summary>
        /// <param name="id">The Payment id</param>
        /// <returns>
        /// 200 OK
        /// {PaymentDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Payment/Find/1 -> {PaymentDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<PaymentDto>> FindPayment(int id)
        {

            var payment = await _paymentService.FindPayment(id);

            // if the Payment could not be located, return 404 Not Found
            if(payment == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(payment);
            }
        }
        /// <summary>
        /// Updates a Payment
        /// </summary>
        /// <param name="id">The ID of the Payment to update</param>
        /// <param name="PaymentDto">The required information to update the Payment (Pid,Bid,Date,status...)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Payment/Update/5
        /// Request Headers: Content-Type: application/json
        /// Request Body: {PaymentDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdatePayment(int id, PaymentDto PaymentDto)
        {
            // {id} in URL must match PaymentId in POST Body
            if (id != PaymentDto.Pid)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _paymentService.UpdatePayment(PaymentDto);

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
        /// Adds a Payment
        /// </summary>
        /// <param name="PaymentDto">The required information to add the Payment (bid,cid,date, methood , status)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Payment/Find/{PaymentId}
        /// {PaymentDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Payment/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {PaymentDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Payment/Find/{PaymentId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Package>> AddPayment(PaymentDto PaymentDto)
        {
            ServiceResponse response = await _paymentService.AddPayment(PaymentDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Payment/FindPayment/{response.CreatedId}", PaymentDto);
        }
        /// <summary>
        /// Deletes the Payment
        /// </summary>
        /// <param name="id">The id of the Payment to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Payment/Delete/6
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            ServiceResponse response = await _paymentService.DeletePayment(id);

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
        //ListPaymentForCustomer
        [HttpGet(template: "ListForCustomer/{id}")]
        public async Task<IActionResult> ListPaymentsForCustomer(int id)
        {
            // empty list of data transfer object OrderItemDto
            IEnumerable<PaymentDto> paymentDtos = await _paymentService.ListPaymentsForCustomer(id);
            // return 200 OK with OrderItemDtos
            return Ok(paymentDtos);
        }
    }
}
