using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket_Service.Models;

namespace Ticket_Service.Repository
{
    public interface ITicketBookingRepo
    {
        public IEnumerable<Ticket> Get_All_Tickets();
        public bool BookTicket(Ticket ticket);
    }
}
