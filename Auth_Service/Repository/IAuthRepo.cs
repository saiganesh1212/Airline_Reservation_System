using Airline_Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airline_Reservation.Repository
{
    public interface IAuthRepo
    {
        public User Login(User user); 
    }
}
