using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface IAcademicYearRepo
    {
        Task<int> Create(AcademicYear year);
        Task<List<AcademicYearDTO>>? GetAll();
        Task<AcademicYearDTO>? GetCurrentOne();
        Task<AcademicYearDTO>? GetById(int id);
        Task<bool> SetCurrent(AcademicYearDTO yearmodel);
    }
}
