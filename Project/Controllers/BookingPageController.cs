using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;
using Project.Models.ViewModels;
using Project.Services;

namespace Project.Controllers
{
    public class BookingPageController : Controller
    {
        private readonly IPackageService _packageService;
        private readonly IBookingService _bookingService;
        
        // dependency injection of service interface
        public BookingPageController(IPackageService PackageService, IBookingService BookingService)
        {

            _packageService = PackageService;
            _bookingService = BookingService;
            
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        // GET: BookingPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<BookingDto?> BookingDtos = await _bookingService.ListBookings();
            return View(BookingDtos);
        }
        // GET: BookingPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            BookingDto? BookingDto = await _bookingService.FindBooking(id);
            IEnumerable<PackageDto> AssociatedPackages = await _packageService.ListPackagesForBooking(id);

            IEnumerable<PackageDto> Packages = await _packageService.ListPackages();

            
            

            if (BookingDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Booking"] });
            }
            else
            {
                // information which drives a Booking page
                BookingDetails BookingInfo = new BookingDetails()
                {
                    Booking =BookingDto,
                    BookingPackages = AssociatedPackages,
                    AllPackages = Packages,
                    
                };
                return View(BookingInfo);
            }
        }
        //POST BookingPage/LinkToPackage
        //DATA: Pid={Pid}&Bid={Bid}
        [HttpPost]
        public async Task<IActionResult> LinkToPackage([FromForm] int Bid, [FromForm] int Pid)
        {
            await _packageService.LinkPackageToBooking(Pid, Bid);

            return RedirectToAction("Details", new { id = Bid });
        }

        //POST ProductPage/UnlinkFromPackage
        //DATA: Pid={Pid}&Bid={Bid}
        [HttpPost]
        public async Task<IActionResult> UnlinkFromPackage([FromForm] int Bid, [FromForm] int Pid)
        {
            await _packageService.UnlinkPackageFromBooking(Pid, Bid);

            return RedirectToAction("Details", new { id = Bid });
        }
        // GET BookingPage/New
        public ActionResult New()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Adds()
        {
            return View();
        }
        // POST BookingPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(BookingDto BookingDto)
        {
            ServiceResponse response = await _bookingService.AddBooking(BookingDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Adds", "BookingPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET BookingPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            BookingDto? BookingDto = await _bookingService.FindBooking(id);
            if (BookingDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(BookingDto);
            }
        }

        //POST BookingPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, BookingDto BookingDto)
        {
            ServiceResponse response = await _bookingService.UpdateBooking(BookingDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Adds", "BookingPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET BookingPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            BookingDto? BookingDto = await _bookingService.FindBooking(id);
            if (BookingDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(BookingDto);
            }
        }
       
        [HttpGet]
        public IActionResult Deletes()
        {
            return View();
        }

        //POST BookingPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _bookingService.DeleteBooking(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("Deletes", "BookingPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
