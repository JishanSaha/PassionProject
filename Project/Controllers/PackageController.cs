using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        // dependency injection of service interfaces
        public PackageController(IPackageService PackageService)
        {
            _packageService = PackageService;
        }


        /// <summary>
        /// Returns a list of Packages
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{PackageDto},{PackageDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Package/List -> [{PackageDto},{PackageDto},..]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> ListPackages()
        {
            // empty list of data transfer object PackageDto
            IEnumerable<PackageDto> PackageDtos = await _packageService.ListPackages();
            // return 200 OK with PackageDtos
            return Ok(PackageDtos);
        }
        /// <summary>
        /// Returns a single Package specified by its {id}
        /// </summary>
        /// <param name="id">The Package id</param>
        /// <returns>
        /// 200 OK
        /// {ProductDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Package/Find/1 -> {PackageDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<PackageDto>> FindPackage(int id)
        {

            var Package = await _packageService.FindPackage(id);

            // if the Package could not be located, return 404 Not Found
            if (Package == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Package);
            }
        }

        /// <summary>
        /// Updates a Package
        /// </summary>
        /// <param name="id">The ID of the Package to update</param>
        /// <param name="PackageDto">The required information to update the Package (PackageName, PackageDescription, PackagePrice)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Package/Update/5
        /// Request Headers: Content-Type: application/json
        /// Request Body: {PackageDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdatePackage(int id, PackageDto PackageDto)
        {
            // {id} in URL must match PackageId in POST Body
            if (id != PackageDto.Pid)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _packageService.UpdatePackage(PackageDto);

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
        /// Adds a Package
        /// </summary>
        /// <param name="PackageDto">The required information to add the Package (PackageName, PackageDescription, PackagePrice)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Package/Find/{PackageId}
        /// {PackageDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Package/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {PackageDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Package/Find/{PackageId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Package>> AddPackage(PackageDto PackageDto)
        {
            ServiceResponse response = await _packageService.AddPackage(PackageDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Package/FindPackage/{response.CreatedId}", PackageDto);
        }
        /// <summary>
        /// Deletes the Package
        /// </summary>
        /// <param name="id">The id of the Package to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Package/Delete/7
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeletePackage(int id)
        {
            ServiceResponse response = await _packageService.DeletePackage(id);

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
        /// <summary>
        /// Returns a list of packages for a specific booking by its {id}
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{PackageDto},{PackageDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Package/ListForBooking/3 -> [{PackageDto},{PackageDto},..]
        /// </example>
        [HttpGet(template: "ListForBooking/{id}")]
        public async Task<IActionResult> ListPackagesForBooking(int id)
        {
            // empty list of data transfer object PackageDto
            IEnumerable<PackageDto> PackageDtos = await _packageService.ListPackagesForBooking(id);
            // return 200 OK with BookingDtos
            return Ok(PackageDtos);
        }
        /// <summary>
        /// Links a package from a booking
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="id">The id of the booking</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// Post: api/Package/Link?PackageId=4&Bookingid=1
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPost("Link")]
        public async Task<ActionResult> Link(int packageID, int bookingID)
        {
            ServiceResponse response = await _packageService.LinkPackageToBooking(packageID, bookingID);

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
        /// <summary>
        /// Unlinks a package from a booking
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="id">The id of the booking</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// Delete: api/Package/Unlink?Pid=4&Bid=1
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Unlink")]
        public async Task<ActionResult> Unlink(int PackageId, int BookingId)
        {
            ServiceResponse response = await _packageService.UnlinkPackageFromBooking(PackageId, BookingId);

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
