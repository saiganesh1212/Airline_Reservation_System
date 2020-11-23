using Airline_Reservation.DAL;
using Airline_Reservation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airline_Reservation.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly UserDbContext _context;
        public AuthRepo(UserDbContext context)
        {
            _context = context;
        }
        public User Login(User user)
        {
            return _context.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
        }
    }
}
