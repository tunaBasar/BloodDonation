using BloodDonationAPI.DataAccess;
using BloodDonationAPI.DTOs.PlacesDto;
using BloodDonationAPI.Entities;
using BloodDonationAPI.Extensions;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services
{
    

    public class PlacesService:IPlacesService
    {
        private readonly ApplicationDbContext context;
        
        
        public PlacesService(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task<PlacesDto> CreatePlacesAsync(PlacesDto dto)
        {
            var places= dto.ToEntity();
            await context.Places.AddAsync(places);
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
            context.Places.Remove(place);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PlacesDto>> GetAllPlacesAsync()
        {
            var place = await context.Places.ToListAsync();
            var placesDtos = place.Select(p => p.ToDto());
            return placesDtos;
        }

        public async Task<PlacesDto?> GetPlacesByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id), " does not contain");
            }
            var place = await context.Places.FindAsync(id);
            if (place == null)
            {
                throw new ArgumentNullException(nameof(id), " does not match any place!!");
            }
            var placesDtos = place.ToDto();
            return placesDtos;
        }

        public async Task<bool> UpdatePlacesAsync(int id, PlacesDto dto)
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

        public Places FindById(int id)
        {
            var place = context.Places.Find(id);
            if (place == null)
            {
                throw new ArgumentNullException(nameof(id), " this id is not avaible");
            }

            return place;  
        }
    }
}