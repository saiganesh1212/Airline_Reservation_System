using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket_Service.Models;

namespace Ticket_Service.DAL
{
    public class TicketContext:DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {

        }
        public virtual DbSet<Ticket> Tickets { get; set; }
    }
}
