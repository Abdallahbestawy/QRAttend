using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class AcademicYearRepo : IAcademicYearRepo
    {
        private readonly QRContext _context;

        public AcademicYearRepo(QRContext context)
        {
            _context = context;
        }

        public async Task<int> Create(AcademicYear year)
        {
            await _context.AcademicYears.AddAsync(year);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<AcademicYearDTO>>? GetAll()
        {
            var result = await _context.AcademicYears.Select(year => new AcademicYearDTO
            {
                Id = year.Id,
                IsCurrent = year.IsCurrent,
                Name = year.Name
            }).ToListAsync();
            if (result == null)
                return null;
            return result;
        }

        public async Task<AcademicYearDTO>? GetById(int id)
        {
            var result = await _context.AcademicYears.Select(year => new AcademicYearDTO
            {
                Id = year.Id,
                IsCurrent = year.IsCurrent,
                Name = year.Name
            }).FirstOrDefaultAsync(y => y.Id == id);
            if (result == null)
                return null;
            return result;
        }

        public async Task<AcademicYearDTO> GetCurrentOne()
        {
            var years = await _context.AcademicYears.ToListAsync();
            if (years.Count == 0)
                return null;
            var result = years.Select(y => new AcademicYearDTO
            {
                Id = y.Id,
                IsCurrent = y.IsCurrent,
                Name = y.Name
            }).FirstOrDefault(years => years.IsCurrent == true);
            if (result == null)
                return null;
            return result;
        }

        public async Task<bool> SetCurrent(AcademicYearDTO yearmodel)
        {
            var years = await _context.AcademicYears.ToListAsync();
            foreach (var year in years)
            {
                if (yearmodel.Id == year.Id)
                    year.IsCurrent = true;
                year.IsCurrent = false;
                _context.AcademicYears.Update(year);
            }
            var result = await _context.SaveChangesAsync();
            if(result == 0)
                return false;
            return true;
        }
    }
}
