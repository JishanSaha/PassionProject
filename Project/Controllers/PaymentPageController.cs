using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models.ViewModels;
using Project.Models;

namespace Project.Controllers
{
    public class PaymentPageController : Controller
    {
        private readonly IPaymentService _paymentService;


        // dependency injection of service interface
        public PaymentPageController(IPaymentService PaymentService)
        {
            _paymentService = PaymentService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: PaymentPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<PaymentDto?> PaymentDtos = await _paymentService.ListPayments();
            return View(PaymentDtos);
        }
        // GET: PaymentPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            PaymentDto? PaymentDto = await _paymentService.FindPayment(id);

            if (PaymentDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Payment"] });
            }
            else
            {
                // information which drives a payment page
                PaymentDetails PaymentInfo = new PaymentDetails()
                {
                    Payment = PaymentDto,

                };
                return View(PaymentInfo);
            }
        }
        // GET PaymentPage/New
        public ActionResult New()
        {
            return View();
        }
        // POST PaymentPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(PaymentDto PaymentDto)
        {
            ServiceResponse response = await _paymentService.AddPayment(PaymentDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "PaymentPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET PaymentPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            PaymentDto? PaymentDto = await _paymentService.FindPayment(id);
            if (PaymentDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(PaymentDto);
            }
        }
        //POST PaymentPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, PaymentDto PaymentDto)
        {
            ServiceResponse response = await _paymentService.UpdatePayment(PaymentDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List", "PaymentPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET PaymentPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            PaymentDto? PaymentDto = await _paymentService.FindPayment(id);
            if (PaymentDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(PaymentDto);
            }
        }
        //POST PaymentPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _paymentService.DeletePayment(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "PaymentPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        [HttpGet]
        public IActionResult Adds()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Deletes()
        {
            return View();
        }
    }
}
