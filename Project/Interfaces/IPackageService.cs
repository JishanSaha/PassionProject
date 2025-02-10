using Project.Models;

namespace Project.Interfaces
{
    public interface IPackageService
    {
        // base CRUD
        Task<IEnumerable<PackageDto>> ListPackages();


        Task<PackageDto?> FindPackage(int id);


        Task<ServiceResponse> UpdatePackage(PackageDto packageDto);

        Task<ServiceResponse> AddPackage(PackageDto packageDto);

        Task<ServiceResponse> DeletePackage(int id);

        Task<IEnumerable<PackageDto>> ListPackagesForBooking(int id);

        Task<ServiceResponse> LinkPackageToBooking(int packageID, int bookingID);

        Task<ServiceResponse> UnlinkPackageFromBooking(int packageId, int bookingId);
    }
}
