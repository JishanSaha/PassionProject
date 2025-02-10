using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Interfaces;
using Project.Models;

namespace Project.Services
{
    public class PackageService  : IPackageService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public PackageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PackageDto>> ListPackages()
        {
            // all Packages
            List<Package> Packages = await _context.Packages
                .ToListAsync();
            // empty list of data transfer object PackageDto
            List<PackageDto> PackageDtos = new List<PackageDto>();
            foreach (Package Package in Packages)
            {
                // create new instance of PackageDto, add to list
                PackageDtos.Add(new PackageDto()
                {
                    Pid = Package.Pid,
                    Name = Package.Name,
                    Description = Package.Description,
                    Price = Package.Price
                });
            }
            // return PackageDtos
            return PackageDtos;

        }
        public async Task<PackageDto?> FindPackage(int id)
        {

            var Package = await _context.Packages
                .FirstOrDefaultAsync(p => p.Pid == id);


            if (Package == null)
            {
                return null;
            }
            // create an instance of PackageDto
            PackageDto PackageDto = new PackageDto()
            {
                Pid = Package.Pid,
                Name = Package.Name,
                Description = Package.Description,
                Price = Package.Price
            };
            return PackageDto;

        }
        public async Task<ServiceResponse> UpdatePackage(PackageDto PackageDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Package
            Package Package = new Package()
            {
                Pid = PackageDto.Pid,
                Name = PackageDto.Name,
                Description = PackageDto.Description,
                Price = PackageDto.Price
            };
            // flags that the object has changed
            _context.Entry(Package).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Packages set ... where PackageId={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record");

                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;

        }
        public async Task<ServiceResponse> AddPackage(PackageDto PackageDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Product
            Package Package = new Package()
            {
                Name = PackageDto.Name,
                Description = PackageDto.Description,
                Price = PackageDto.Price
            };
            // SQL Equivalent: Insert into Products (..) values (..)

            try
            {
                _context.Packages.Add(Package);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Package.");
                serviceResponse.Messages.Add(ex.Message);

            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = Package.Pid;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeletePackage(int id)
        {
            ServiceResponse response = new();

            var Package = await _context.Packages.FindAsync(id);
            if (Package == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Package cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Packages.Remove(Package);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Package");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }
        public async Task<IEnumerable<PackageDto>> ListPackagesForBooking(int id)
        {
            // join BookingPackage on Packages.Pid = BookingPackage.Packageid WHERE BookingPackage.Bookingid = {id}
            List<Package> Packages = await _context.Packages
                .Where(p => p.Bookings.Any(b => b.Bid == id))
                .ToListAsync();

            // empty list of data transfer object PackageDto
            List<PackageDto> PackageDtos = new List<PackageDto>();
            // foreach Order Item record in database
            foreach (Package Package in Packages)
            {
                // create new instance of PackageDto, add to list
                PackageDtos.Add(new PackageDto()
                {
                    Pid = Package.Pid,
                    Name = Package.Name,
                    Description = Package.Description,
                    Price = Package.Price
                });
            }
            // return ProductDtos
            return PackageDtos;

        }
        public async Task<ServiceResponse> LinkPackageToBooking(int packageID, int bookingID)
        {
            ServiceResponse serviceResponse = new();

            Package? package = await _context.Packages
                .Include(p => p.Bookings)
                .Where(p => p.Pid == packageID)
                .FirstOrDefaultAsync();
            Booking? booking = await _context.Bookings.FindAsync(bookingID);

            // Data must link to a valid entity
            if (booking == null || package == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (booking == null)
                {
                    serviceResponse.Messages.Add("booking was not found. ");
                }
                if (package == null)
                {
                    serviceResponse.Messages.Add("package was not found.");
                }
                return serviceResponse;
            }
            try
            {
                package.Bookings.Add(booking);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue linking the booking to the package");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UnlinkPackageFromBooking(int packageId, int bookingId)
        {
            ServiceResponse serviceResponse = new();

            Package? Package = await _context.Packages
                .Include(p => p.Bookings)
                .Where(p => p.Pid == packageId)
                .FirstOrDefaultAsync();
            Booking? Booking = await _context.Bookings.FindAsync(bookingId);

            // Data must link to a valid entity
            if (Package == null || Booking == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (Booking == null)
                {
                    serviceResponse.Messages.Add("Booking was not found. ");
                }
                if (Package == null)
                {
                    serviceResponse.Messages.Add("Package was not found.");
                }
                return serviceResponse;
            }
            try
            {
                Package.Bookings.Remove(Booking);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue unlinking the Booking to the Package");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }
    }
}
