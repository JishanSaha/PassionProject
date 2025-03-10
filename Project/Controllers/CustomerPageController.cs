using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;
using Project.Models.ViewModels;


namespace Project.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;

        //dependency injection of the service inerface
        public CustomerPageController(ICustomerService CustomerService, IBookingService BookingService, IPaymentService paymentService)
        {
            _customerService = CustomerService;
            _bookingService = BookingService;
            _paymentService = paymentService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        // GET: CustomerPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<CustomerDto?> CustomerDtos = await _customerService.ListCustomers();
            return View(CustomerDtos);
        }
        // GET: CustomerPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            CustomerDto? CustomerDto = await _customerService.FindCustomer(id);
            IEnumerable<BookingDto> AssociatedBooking = await _bookingService.ListBookingsForCustomer(id);
            IEnumerable<PaymentDto> AssociatedPayment = await _paymentService.ListPaymentsForCustomer(id);

            if (CustomerDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find customer"] });
            }
            else
            {
                // information which drives a customer page
                CustomerDetails CustomerInfo = new CustomerDetails()
                {
                    Customer = CustomerDto,
                    CustomerBookings = AssociatedBooking,
                    CustomerPayments = AssociatedPayment
                };
                return View(CustomerInfo);
            }
        }
        [HttpGet]
        public IActionResult Adds()
        {
            return View();
        }
        [HttpGet]
        // GET CustomerPage/New
        public ActionResult New()
        {
            return View();
        }
        // POST CustomerPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(CustomerDto CustomerDto)
        {
            ServiceResponse response = await _customerService.AddCustomer(CustomerDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Adds", "CustomerPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET CustomerPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CustomerDto? CustomerDto = await _customerService.FindCustomer(id);
            if (CustomerDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(CustomerDto);
            }
        }

        //POST CustomerPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, CustomerDto CustomerDto)
        {
            ServiceResponse response = await _customerService.UpdateCustomer(CustomerDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Adds", "CustomerPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET CustomerPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            CustomerDto? CustomerDto = await _customerService.FindCustomer(id);
            if (CustomerDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(CustomerDto);
            }
        }
        // GET: CuatomerPage/Details/{id}
        [HttpGet]
        public IActionResult Deletes()
        {
            return View();
        }

        //POST CustomerPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _customerService.DeleteCustomer(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("Deletes", "CustomerPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
