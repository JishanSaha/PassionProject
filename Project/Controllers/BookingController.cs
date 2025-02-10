using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Interfaces;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        // dependency injection of service interfaces
        public BookingController(IBookingService BookingService)
        {
            _bookingService = BookingService;
        }


        /// <summary>
        /// Returns a list of Bookings
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{BookingDto},{BookingDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Booking/List -> [{BookingDto},{BookingDto},..]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> ListBookings()
        {
            // empty list of data transfer object BookingDto
            IEnumerable<BookingDto> BookingDtos = await _bookingService.ListBookings();
            // return 200 OK with BookingDtos
            return Ok(BookingDtos);
        }
        /// <summary>
        /// Returns a single Booking specified by its {id}
        /// </summary>
        /// <param name="id">The Booking id</param>
        /// <returns>
        /// 200 OK
        /// {BookingDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Booking/Find/1 -> {BookingDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<BookingDto>> FindBooking(int id)
        {

            var Booking = await _bookingService.FindBooking(id);

            // if the Booking could not be located, return 404 Not Found
            if (Booking == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Booking);
            }
        }

        /// <summary>
        /// Updates a Booking
        /// </summary>
        /// <param name="id">The ID of the Booking to update</param>
        /// <param name="BookingDto">The required information to update the Booking (Cid,paymentmethod, status...)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Booking/Update/5
        /// Request Headers: Content-Type: application/json
        /// Request Body: {BookingDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateBooking(int id, BookingDto BookingDto)
        {
            // {id} in URL must match BookingId in POST Body
            if (id !=BookingDto.Bid)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _bookingService.UpdateBooking(BookingDto);

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
        /// Adds a Booking
        /// </summary>
        /// <param name="BookingDto">The required information to add the Booking (bid,cid,date, methood , status)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Booking/Find/{BookingId}
        /// {BookingDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Booking/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {BookingDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Booking/Find/{BookingId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Package>> AddBooking(BookingDto BookingDto)
        {
            ServiceResponse response = await _bookingService.AddBooking(BookingDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Booking/FindBooking/{response.CreatedId}", BookingDto);
        }
        /// <summary>
        /// Deletes the Booking
        /// </summary>
        /// <param name="id">The id of the Booking to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Booking/Delete/7
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            ServiceResponse response = await _bookingService.DeleteBooking(id);

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
        //ListBookingForCustomer
        [HttpGet(template: "ListForCustomer/{id}")]
        public async Task<IActionResult> ListBookingsForCustomer(int id)
        {
            // empty list of data transfer object OrderItemDto
            IEnumerable<BookingDto> bookingDtos = await _bookingService.ListBookingsForCustomer(id);
            // return 200 OK with OrderItemDtos
            return Ok(bookingDtos);
        }

    }
}
