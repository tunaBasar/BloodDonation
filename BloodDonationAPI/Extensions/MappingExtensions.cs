using BloodDonationAPI.DTOs.RequestDto;
using BloodDonationAPI.DTOs.UserDto;
using BloodDonationAPI.DTOs.DonationDto;
using BloodDonationAPI.Entities;
using System.Linq;
using BloodDonationAPI.DTOs.PlacesDto;

namespace BloodDonationAPI.Extensions
{
    public static class MappingExtensions
    {

        public static UserResponseDto ToResponseDto(this User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                SurName = user.SurName,
                Mail = user.Mail,
                PhoneNumber = user.PhoneNumber,
                BloodType = user.BloodType,
            };
        }

        public static RequestResponseDto ToResponseDto(this Request request)
        {
            return new RequestResponseDto
            {
                Id = request.Id,
                BloodType = request.BloodType,
                UserId = request.UserId,
                UrgencyLevel = request.UrgencyLevel,
                Description = request.Description,
                IsActive = request.IsActive,
            };
        }

        public static DonationResponseDto ToResponseDto(this Donation donation)
        {
            return new DonationResponseDto
            {
                Id = donation.Id,
                UserId = donation.UserId,
                RequestId = donation.RequestId,
            };
        }
        public static HospitalsDto ToDto(this Hospitals hospitals)
        {
            return new HospitalsDto
            {
                Address = hospitals.Address,
                IsMobile = hospitals.IsMobile,
                Plate = hospitals.Plate
            };
        }
        public static DonationCreateDto ToDonationCreateDto(this DeactiveRequestDto dto)
        {
            return new DonationCreateDto
            {
                RequestId = dto.requestId,
                UserId = dto.userId,
                IsActive = dto.IsActive
            };
        }



        public static User ToEntity(this UserCreateDto dto)
        {
            return new User
            {
                Name = dto.Name,
                SurName = dto.SurName,
                Mail = dto.Mail,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Tc = dto.Tc,
                BloodType = dto.BloodType
            };
        }

        public static Request ToEntity(this RequestCreateDto dto)
        {
            return new Request
            {
                BloodType = dto.BloodType,
                UserId = dto.UserId,
                UrgencyLevel = dto.UrgencyLevel,
                Description = dto.Description,
                IsActive = true
            };
        }

        public static Donation ToEntity(this DonationCreateDto dto)
        {
            return new Donation
            {
                UserId = dto.UserId,
                RequestId = dto.RequestId
            };
        }

        public static Hospitals ToEntity(this HospitalsDto dto)
        {
            return new Hospitals
            {
                Address = dto.Address,
                IsMobile = dto.IsMobile,
                Plate = dto.Plate
            };
        }
    }
}