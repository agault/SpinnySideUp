using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
    public class PilotDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(IEnumerable<PilotDto>))]

        public IHttpActionResult GetPilots()//GETPilotS Pilot contoller used to generate list view
        {
            List<Pilot> Pilots = db.Pilots.Include(pilot => pilot.User).ToList();
            List<PilotDto> PilotDtos = new List<PilotDto> { };
            //api/PilotData/GetPilots

            //We Want to send the API data so it can infact get the list and present it.
            //The for each loops over all the Pilots to get the details exposed.
            foreach (var Pilot in Pilots)
            {
                PilotDto NewPilot = new PilotDto
                {
                    //We are not going to display the creator Id to the public just yet. we want it so only registered users as admin can creat.
                    PilotId = Pilot.PilotId,
                    FirstName = Pilot.FirstName,
                    LastName = Pilot.LastName,
                    Rank = Pilot.Rank
                   

                };
                PilotDtos.Add(NewPilot);
            }

            return Ok(PilotDtos);

        }

        /// <summary>
        /// DETAILS OF A SINGLE Pilot
        // GET: api/PilotData/5
        //FIND Pilot BY ITS ID retreives details:
        /// </summary>
        /// <param name="id">Pilot ID</param>
        /// <returns>RETURNS A Pilot DETAIL WITH A URL LINK COMES BACK AS XML OBJ</returns>

        [HttpGet]
        [ResponseType(typeof(PilotDto))]
        public IHttpActionResult FindPilot(int id)
        {
            //Find Pilot Data
            Pilot Pilot = db.Pilots.Find(id);
            //return 404 status code(not found)
            if (Pilot == null)
            {
                return NotFound();
            }

            //In frreindly object format
            PilotDto PilotDto = new PilotDto
            {
                PilotId = Pilot.PilotId,
                FirstName = Pilot.FirstName,
                LastName = Pilot.LastName,
                DateBirth = Pilot.DateBirth,
                Rank =Pilot.Rank,
                Pc = Pilot.Pc,
                Pi = Pilot.Pi,
                Sp = Pilot.Sp,
                Ip = Pilot.Ip,
                Fac = Pilot.Fac,
                UserId = Pilot.UserId
               // Flight = Pilot.Flight.FlightId <<<Collection of Flights
            };

            //Sends 200 code
            return Ok(PilotDto);
        }
        /// <summary>
        /// CREATE Pilot,
        /// Sends to the DB
        /// POST: api/PilotData/AddPilot
        /// </summary>
        /// <param name="Pilot">The creation of a Pilot obj</param>
        /// <returns></returns>

        [ResponseType(typeof(Pilot))]
        [HttpPost]
        public IHttpActionResult AddPilot([FromBody] Pilot Pilot)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pilots.Add(Pilot);
            db.SaveChanges();

            return Ok(Pilot.PilotId);
        }
        //UPDATE A PilotS INFO
        /// <summary>
        /// //UPDATE A PilotS INFO
        /// </summary>
        /// <param name="id">PilotId</param>
        /// <param name="Pilot">Pilot Data Received as a POST request</param>
        /// <returns>Should Return Updated Pilot info </returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePilot(int id, [FromBody] Pilot Pilot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != Pilot.PilotId)
            {
                return BadRequest();
            }
            db.Entry(Pilot).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PilotExists(id))//Frefered to by other method to check if it in fact exsists
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
        ///  DELETE: api/PilotData/DeletePilot/2
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Deletes a Pilot using the ID of a Pilot</returns>

        [HttpPost]
        public IHttpActionResult DeletePilot(int id)
        {
            Pilot pilot = db.Pilots.Find(id);
            if (pilot == null)
            {
                return NotFound();
            }

            db.Pilots.Remove(pilot);
            db.SaveChanges();

            return Ok(pilot);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool PilotExists(int id)
        {
            return db.Pilots.Count(e => e.PilotId == id) > 0;
        }

    }
}