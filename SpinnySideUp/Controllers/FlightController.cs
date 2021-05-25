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
using System.Diagnostics;
using System.Web.Script.Serialization;
using SpinnySideUp.Models;
using System.Net;
using Microsoft.AspNet.Identity;

namespace SpinnySideUp.Controllers
{
    public class FlightController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static FlightController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //My Port # is 44314
            client.BaseAddress = new Uri("https://localhost:44314/api/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //warning function ignoring:
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        // GET: Flight
        //Flight/List
        // LIST ALL THE FLIGHTS IN VIEW:
        public ActionResult List()
        {
            string url = "FlightData/GetFlights";// from data controller GetFlights

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FlightDto> SelectedFlight = response.Content.ReadAsAsync<IEnumerable<FlightDto>>().Result;
                return View(SelectedFlight);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //GET Flight/Details/3
        public ActionResult Details(int id)
        {
            ShowFlight ViewModels = new ShowFlight();
            string FlightDetailurl = "Flightdata/findFlight/" + id;
            HttpResponseMessage findFlightresponse = client.GetAsync(FlightDetailurl).Result;

            if (findFlightresponse.IsSuccessStatusCode)
            {

                FlightDto SelectedFlight = findFlightresponse.Content.ReadAsAsync<FlightDto>().Result;
                ViewModels.Flight = SelectedFlight;

                return View(ViewModels);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        //ADD/CREATE new Flight
        // GET:Flight/Create 
        [HttpGet]
        public ActionResult Create()//FORM Imputs from view
        {
            return View();
        }

        //POST Flight/Create
        //Posts to DB and creats new Flight if form entries are correct
        //send request to Data controller. 

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Flight FlightInfo)
        {
            string AddFlightUrl = "FlightData/AddFlight"; //AddFlight model created in data controller
            FlightInfo.UserId = User.Identity.GetUserId();//USER ID WHO CREATES FLIGHT

            HttpContent content = new StringContent(jss.Serialize(FlightInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage AddFlightResponse = client.PostAsync(AddFlightUrl, content).Result;

            if (AddFlightResponse.IsSuccessStatusCode)
            {

                int FlightId = AddFlightResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = FlightId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //UPDATE FLIGHTS
        //EDIT Flight callls on update 
        //GET: Flight/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateFlight ViewModels = new UpdateFlight();

            string GetUpdateFlightUrl = "FlightData/findFlight/" + id;//locate Flight by id
            HttpResponseMessage FindFlightResponse = client.GetAsync(GetUpdateFlightUrl).Result;

            if (FindFlightResponse.IsSuccessStatusCode)
            {
                //Put data into DepartmentDto
                FlightDto SelectedFlight = FindFlightResponse.Content.ReadAsAsync<FlightDto>().Result;
                ViewModels.Flight = SelectedFlight;

                return View(ViewModels);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //POST UPDATE
        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Flight FlightInfo)
        {
            string PostUpdateFlightUrl = "FlightData/UpdateFlight/" + id;
            FlightInfo.UserId = User.Identity.GetUserId();
            Debug.WriteLine(FlightInfo);
            HttpContent content = new StringContent(jss.Serialize(FlightInfo));
            Debug.WriteLine(FlightInfo);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage UpdateFlightResponse = client.PostAsync(PostUpdateFlightUrl, content).Result;

            if (UpdateFlightResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //DELETE FLIGHT:
        //Get Request Flight/Delete/3
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string GetFlightDeleteUrl = "Flightdata/findFlight/" + id;//find the releventFlight
            HttpResponseMessage DeleteFlightResponse = client.GetAsync(GetFlightDeleteUrl).Result;

            if (DeleteFlightResponse.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                FlightDto SelectedFlight = DeleteFlightResponse.Content.ReadAsAsync<FlightDto>().Result;
                return View(SelectedFlight);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //POST REQUEST TO DELETE
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string PostFlightDeleteUrl = "Flightdata/deleteFlight/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(PostFlightDeleteUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


    }
}