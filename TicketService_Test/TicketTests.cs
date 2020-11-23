using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using Ticket_Service.Controllers;
using Ticket_Service.DAL;
using Ticket_Service.Models;
using Ticket_Service.Repository;

namespace TicketService_Test
{
    public class Tests
    {
        List<Ticket> tickets = new List<Ticket>();
        IQueryable<Ticket> ticketdata;
        Mock<DbSet<Ticket>> mockSet;
        Mock<TicketContext> ticketcontextmock;
        Mock<IConfiguration> config;
        
        TicketBookingRepo ticketrepo;
        TicketController controller;

        [SetUp]
        public void Setup()
        {
            tickets = new List<Ticket>()
            {
                new Ticket{TicketId=1,Age=22,PassengerName="Ganesh",FlightId=1,StartingLocation="Pune",Destination="Agra",DateOfJourney=new System.DateTime(2020,11,24) },
                new Ticket{TicketId=1,Age=22,PassengerName="Suresh",FlightId=2,StartingLocation="Mumbai",Destination="Delhi",DateOfJourney=new System.DateTime(2020,11,22)
                }
            };

            ticketdata = tickets.AsQueryable();
            mockSet = new Mock<DbSet<Ticket>>();
            mockSet.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(ticketdata.Provider);
            mockSet.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(ticketdata.Expression);
            mockSet.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(ticketdata.ElementType);
            mockSet.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(ticketdata.GetEnumerator());
            var p = new DbContextOptions<TicketContext>();
            ticketcontextmock = new Mock<TicketContext>(p);
           ticketcontextmock.Setup(x => x.Tickets).Returns(mockSet.Object);
            
            config = new Mock<IConfiguration>();
            config.Setup(p => p["Jwt:Key"]).Returns("ThisismySecretKey");
            ticketrepo = new TicketBookingRepo(ticketcontextmock.Object);
            
            
        }

        [Test]
        public void Get_All_Tickets_Test()
        {
            controller = new TicketController(ticketrepo);
            var res = controller.Get_All_Tickets() as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
            Assert.IsInstanceOf<List<Ticket>>(res.Value);
        }
        
        [Test]
        public void Book_Ticket_Test()
        {
            Ticket ticket = new Ticket() { TicketId = 1, Age = 22, PassengerName = "Ram", FlightId = 1, StartingLocation = "Pune", Destination = "Agra", DateOfJourney = new System.DateTime(2020, 11, 24) };
            controller = new TicketController(ticketrepo);
            var res1=controller.Book_Ticket(ticket) as StatusCodeResult;
            int t=res1.StatusCode;
            Assert.AreEqual(201, t);
            
        }
        
        
    }
}