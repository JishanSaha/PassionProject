using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    public class PackagePageController : Controller
    {
        private readonly IPackageService _packageService;
        

        // dependency injection of service interface
        public PackagePageController(IPackageService PackageService)
        {
            _packageService = PackageService;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
       
        // GET: PackagePage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<PackageDto?> PackageDtos = await _packageService.ListPackages();
            return View(PackageDtos);
        }
        // GET: PackagePage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            PackageDto? PackageDto = await _packageService.FindPackage(id);

            if (PackageDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Package"] });
            }
            else
            {
                // information which drives a package page
                PackageDetails PackageInfo = new PackageDetails()
                {
                    Package = PackageDto,
                    
                };
                return View(PackageInfo);
            }
        }
        // GET PackagePage/New
        public ActionResult New()
        {
            return View();
        }
        // POST PackagePage/Add
        [HttpPost]
        public async Task<IActionResult> Add(PackageDto PackageDto)
        {
            ServiceResponse response = await _packageService.AddPackage(PackageDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "PackagePage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET PackagePage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            PackageDto? PackageDto = await _packageService.FindPackage(id);
            if (PackageDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(PackageDto);
            }
        }
        //POST PackagePage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, PackageDto PackageDto)
        {
            ServiceResponse response = await _packageService.UpdatePackage(PackageDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List", "PackagePage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        //GET PackagePage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            PackageDto? PackageDto = await _packageService.FindPackage(id);
            if (PackageDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(PackageDto);
            }
        }
        //POST PackagePage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _packageService.DeletePackage(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "PackagePage");
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
