﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Airline_Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Airline_Client.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TicketController));

        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Info("token not found");

                return RedirectToAction("Login");

            }
            else
            {
                _log4net.Info("Flightslist getting Displayed");

                List<Flight> itemList = new List<Flight>();
                using (var client = new HttpClient())
                {

                     
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    
                    client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                    using (var response = await client.GetAsync("https://localhost:44359/api/availability"))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        itemList = JsonConvert.DeserializeObject<List<Flight>>(apiResponse);
                    }
                }
                _log4net.Info(HttpContext.Session.GetString("Username")+" Successfully got all flights details ");
                return View(itemList);
            }

        }



        // GET: Ticket/Book
        public ActionResult Book()
        {
            return View();
        }

        // POST: Ticket/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Book(Ticket ticket)
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Info("token not found");

                return RedirectToAction("Login");

            }
            else
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    
                    client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri("https://localhost:44359/api/");
                    var responseTask = await client.GetAsync("availability/" + ticket.FlightId.ToString());

                    if (responseTask.IsSuccessStatusCode)
                    {
                        var response = responseTask.Content.ReadAsStringAsync();

                        int currentAvailability = int.Parse(response.Result);
                        if (currentAvailability > 0)
                        {
                            
                            using (var res = await client.PostAsync("https://localhost:44377/api/ticket/book", content))
                            {

                                if ((int)res.StatusCode == 201)
                                {
                                    var reduceTask = await client.DeleteAsync("availability/reduce/" + ticket.FlightId.ToString());

                                    _log4net.Info("Successfully booked for passenger " + ticket.PassengerName);
                                }
                            }
                        }
                    }
                    
                    
                }
                
                return RedirectToAction("Get_All_Bookings");

            }


        }

        [HttpGet("AllBookings")]
        public async Task<IActionResult> Get_All_Bookings()
        {
            _log4net.Info("All Booked tickets");
            List<Ticket> bookings = new List<Ticket>();
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                using (var response = await client.GetAsync("https://localhost:44377/api/ticket/"))
                {
                    var res = await response.Content.ReadAsStringAsync();
                    bookings= JsonConvert.DeserializeObject<List<Ticket>>(res);

                }
            }
            _log4net.Info(HttpContext.Session.GetString("Username") + " Successfully got all bookings details ");
            return View(bookings);
        }
    }
}