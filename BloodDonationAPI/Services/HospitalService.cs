using BloodDonationAPI.DataAccess;
using BloodDonationAPI.DTOs.PlacesDto;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Extensions;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services
{
    

    public class HospitalService:IHospitalService
    {
        private readonly ApplicationDbContext context;
        
        
        public HospitalService(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task<HospitalsDto> CreatePlacesAsync(HospitalsDto dto)
        {
            var places= dto.ToEntity();
            await context.Hospitals.AddAsync(places);
            await context.SaveChangesAsync();
            return places.ToDto();
        }

        public async Task<bool> DeletePlacesAsync(int id)
        {
            var place = FindById(id);
            if(place==null)
            {
                throw new ArgumentNullException(nameof(id)," cant find");
            }
            context.Hospitals.Remove(place);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<HospitalsDto>> GetAllPlacesAsync()
        {
            var place = await context.Hospitals.ToListAsync();
            var placesDtos = place.Select(p => p.ToDto());
            return placesDtos;
        }

        public async Task<HospitalsDto?> GetPlacesByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), " does not contain");
            }
            var place = await context.Hospitals.FindAsync(id);
            if (place == null)
            {
                throw new ArgumentNullException(nameof(id), " does not match any place!!");
            }
            var placesDtos = place.ToDto();
            return placesDtos;
        }

        public async Task<bool> UpdatePlacesAsync(int id, HospitalsDto dto)
        {
            if (dto == null)
            {
                return false;
            }
            var oldPlace = FindById(id);
            context.Entry(oldPlace).CurrentValues.SetValues(dto);
            await context.SaveChangesAsync();
            return true;
        }

        public Hospitals FindById(int id)
        {
            var place = context.Hospitals.Find(id);
            if (place == null)
            {
                throw new ArgumentNullException(nameof(id), " this id is not avaible");
            }

            return place;  
        }
    }
}