using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using SpinnySideUp.Models.ViewModels;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SpinnySideUp.Models;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace SpinnySideUp.Controllers
{
    public class FlightDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// // GET: api/FlightData/GetFlights
        /// </summary>
        /// <returns>A list of all the flights</returns>

        [ResponseType(typeof(IEnumerable<FlightDto>))]
        
        public IHttpActionResult GetFlights()//GETFlightS Flight contoller used to generate list view
        {
            List<Flight> Flights = db.Flights.Include(flight => flight.User).ToList();
            List<FlightDto> FlightDtos = new List<FlightDto> { };

            //We Want to send the API data so it can infact get the list and present it.
            //The for each loops over all the Flights to get the details exposed.
            foreach (var Flight in Flights)
            {
                FlightDto NewFlight = new FlightDto
                {
                    //We are not going to display the creator Id to the public just yet. we want it so only registered users as admin can creat.
                    FlightId = Flight.FlightId,
                    Date = Flight.Date,
                    Duty = Flight.Duty,
                    Seat = Flight.Seat,
                    Mission = Flight.Mission,
                    AircraftId = Flight.AircraftId,
                    UserId = Flight.UserId
                };
                FlightDtos.Add(NewFlight);
            }

            return Ok(FlightDtos);

        }
       /// <summary>
        /// DETAILS OF A SINGLE FLIGHT
        // GET: api/FlightData/5
        //FIND FLIGHT BY ITS ID retreives details:
        /// </summary>
        /// <param name="id">Flight ID</param>
        /// <returns>RETURNS A FLIGHT DETAIL WITH A URL LINK COMES BACK AS XML OBJ</returns>

        [HttpGet]
        [ResponseType(typeof(FlightDto))]
        public IHttpActionResult FindFlight(int id)
        {
            //Find Flight Data
            Flight Flight = db.Flights.Find(id);
            //return 404 status code(not found)
            if (Flight == null)
            {
                return NotFound();
            }

            //In frreindly object format
            FlightDto FlightDto = new FlightDto
            {
                FlightId = Flight.FlightId,
                Date = Flight.Date,
                Duty = Flight.Duty,
                Seat = Flight.Seat,
                Mission = Flight.Mission,
                Day = Flight.Day,
                Night = Flight.Night,
                NightGoggles = Flight.NightGoggles,
                NightSystems = Flight.NightSystems,
                Weather = Flight.Weather,
                AircraftId = Flight.AircraftId,
                Notes = Flight.Notes,
                UserId = Flight.UserId
            };

            //Sends 200 code
            return Ok(FlightDto);
        }
       
        /// <summary>
        /// CREATE FLIGHT,
        /// Sends to the DB
        /// POST: api/flightData/Addflight
        /// </summary>
        /// <param name="Flight">The creation of a flight obj</param>
        /// <returns></returns>

        [ResponseType(typeof(Flight))]
        [HttpPost]
        public IHttpActionResult AddFlight([FromBody] Flight Flight)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Flights.Add(Flight);
            db.SaveChanges();

            return Ok(Flight.FlightId);
        }
        
       //UPDATE A FLIGHTS INFO
       /// <summary>
       /// //UPDATE A FLIGHTS INFO
       /// </summary>
       /// <param name="id">FlightId</param>
       /// <param name="Flight">Flight Data Received as a POST request</param>
       /// <returns>Should Return Updated Flight info </returns>
       [ResponseType(typeof(void))]
       [HttpPost]
       public IHttpActionResult UpdateFlight(int id, [FromBody] Flight Flight)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }
           if (id != Flight.FlightId)
           {
               return BadRequest();
           }
           db.Entry(Flight).State = EntityState.Modified;
           try
           {
               db.SaveChanges();
           }
           catch (DbUpdateConcurrencyException)
           {
               if (!FlightExists(id))//Frefered to by other method to check if it in fact exsists
               {
                   return NotFound();
               }
               else
               {
                   throw;
               }
           }
           return StatusCode(HttpStatusCode.NoContent);
       }
       
       /// <summary>
       ///  DELETE: api/FlightData/DeleteFlight/9
       /// </summary>
       /// <param name="id">ID</param>
       /// <returns>Deletes a flight using the ID of a flight</returns>

       [HttpPost]
       public IHttpActionResult DeleteFlight(int id)
       {
           Flight flight = db.Flights.Find(id);
           if (flight == null)
           {
               return NotFound();
           }

           db.Flights.Remove(flight);
           db.SaveChanges();

           return Ok(flight);
       }

       protected override void Dispose(bool disposing)
       {
           if (disposing)
           {
               db.Dispose();
           }
           base.Dispose(disposing);
       }
    
        /// <summary>
        /// Finds a Flight in the system. Internal use only.
        /// </summary>
        /// <param name="id">Flight id</param>
        /// <returns>TRUE if the flight exists, false otherwise.</returns>
        private bool FlightExists(int id)
       {
           return db.Flights.Count(e => e.FlightId == id) > 0;
       }
    }
}