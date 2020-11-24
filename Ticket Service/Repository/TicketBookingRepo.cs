using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ticket_Service.DAL;
using Ticket_Service.Models;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Ticket_Service.Repository
{
    public class TicketBookingRepo : ITicketBookingRepo
    {
        private readonly TicketContext _context;
        
        public TicketBookingRepo(TicketContext context)
        {
            _context = context;

        }
        
        public bool BookTicket(Ticket ticket)
        {
            try
            {
                _context.Tickets.Add(ticket);
                 _context.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }





            /*
            int availability = await _repo.AvailableCount(ticket.FlightId);
                if ( availability> 0)
                {
                    _context.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();
                await _repo.Reduce(ticket.FlightId);
                    
                    return true;
                }
                else
                {
                    return false;
                }
                
             */

        }

       
        public IEnumerable<Ticket> Get_All_Tickets()
        {
            
                var result =  _context.Tickets.ToList();
                return result;
           
        }
    }
}
