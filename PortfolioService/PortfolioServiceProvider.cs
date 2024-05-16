using Common;
using Common.Helpers;
using Common.Models;
using PortfolioService.Web.Models;
using System;
using System.IO;

namespace PortfolioService
{
    public class PortfolioServiceProvider : IPortfolioService
    {
        public UserDto Login(string email, string password)
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.Get(email);
            var x = userRepository.GetAll();
            if (user is null)
                return null;
            BlobHelper blob = new BlobHelper();
            if (user.Password == password)
                return new UserDto
                {
                    City = user.City,
                    Street = user.Address.Street,
                    Number = user.Address.Number,
                    Country = user.Country,
                    Email = email,
                    FirstName = user.FirstName,
                    Image = blob.Download("images", user.ImageUrl),
                    LastName = user.LastName,
                    Phone = user.Phone
                };
            return null;
        }

        public bool Register(UserDto userDto)
        {
            User user = new User(userDto.Email)
            {
                Address = new Address { Number = userDto.Number, Street = userDto.Street },
                City = userDto.City,
                Country = userDto.Country,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                ImageUrl = userDto.Image,
                LastName = userDto.LastName,
                Password = userDto.Password,
                Phone = userDto.Phone,

            };

            UserRepository userRepository = new UserRepository();
            var existingUser = userRepository.Get(user.Email);
            if (existingUser is null)
            {
                userRepository.Add(user);
                return true;
            }
            return false;

        }

        public bool Update(UserDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            var existingUser = userRepository.Get(userDto.Email);
            if (existingUser is null)
                return false;
            existingUser.Address.Number = userDto.Number;
            existingUser.Address.Street = userDto.Street;
            existingUser.City = userDto.City;
            existingUser.Country = userDto.Country;
            existingUser.Email = userDto.Email;
            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Password = userDto.Password;
            existingUser.Phone = userDto.Phone;
            BlobHelper blob = new BlobHelper();
            blob.Upload(userDto.Image, "images", existingUser.ImageUrl);

            userRepository.Update(existingUser);
            return true;
        }
    }
}
