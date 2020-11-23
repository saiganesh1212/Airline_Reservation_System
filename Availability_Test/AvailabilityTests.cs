using Availability_Service.Controllers;
using Availability_Service.DAL;
using Availability_Service.Models;
using Availability_Service.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Availability_Test
{
    public class Tests
    {
        List<Flight> flights = new List<Flight>();
        IQueryable<Flight> flightdata;
        Mock<DbSet<Flight>> mockSet;
        Mock<AvailableContext> availablecontextmock;
        Mock<IConfiguration> config;
        Mock<AvailabilityRepo> mock;
        AvailabilityRepo availablerepo;
        AvailabilityController controller;
        [SetUp]
        public void Setup()
        {
            flights = new List<Flight>()
            {
                new Flight{FlightId=1,Company_Name="Indigo",Starting_Location="Hyderabad",Destination="Pune",Available_Seats=24 },
                new Flight{FlightId=2,Company_Name="Sahara",Starting_Location="Pune",Destination="Bangalore",Available_Seats=12 }
            };
            flightdata = flights.AsQueryable();
            mockSet = new Mock<DbSet<Flight>>();
            mockSet.As<IQueryable<Flight>>().Setup(m => m.Provider).Returns(flightdata.Provider);
            mockSet.As<IQueryable<Flight>>().Setup(m => m.Expression).Returns(flightdata.Expression);
            mockSet.As<IQueryable<Flight>>().Setup(m => m.ElementType).Returns(flightdata.ElementType);
            mockSet.As<IQueryable<Flight>>().Setup(m => m.GetEnumerator()).Returns(flightdata.GetEnumerator());
            var p = new DbContextOptions<AvailableContext>();
            availablecontextmock = new Mock<AvailableContext>(p);
            availablecontextmock.Setup(x => x.Flights).Returns(mockSet.Object);
            config = new Mock<IConfiguration>();
            config.Setup(p => p["Jwt:Key"]).Returns("ThisismySecretKey");
            availablerepo = new AvailabilityRepo(availablecontextmock.Object);
            mock = new Mock<AvailabilityRepo>();

            controller = new AvailabilityController(availablerepo);
        }

        [Test]
        public void GetAllFlights_Success_Test()
        {
            
            var res = controller.Get_AllFlights() as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200,res.StatusCode);
            Assert.IsInstanceOf<List<Flight>>(res.Value);

        }
        [Test]
        public void GetAvailability_For_Flight_By_Id_Success()
        {
            
            var res = controller.Get_Availability_For_Flight(1) as OkObjectResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
            Assert.IsInstanceOf<int>(res.Value);

        }
        [Test]
        public void GetAvailability_For_Flight_By_Id_Failure()
        {
            
            var res = controller.Get_Availability_For_Flight(4) as StatusCodeResult;
            Assert.AreEqual(404, res.StatusCode);

        }
        [Test]
        public void Reduce_Availability_Success_Test()
        {
            var res = controller.Reduce_Availability(2) as StatusCodeResult;
            Assert.AreEqual(200, res.StatusCode);
        }
        [Test]
        public void Reduce_Availability_Failure_Test()
        {
            var res = controller.Reduce_Availability(4) as StatusCodeResult;
            Assert.AreEqual(404, res.StatusCode);
        }
        
    }
}