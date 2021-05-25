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

namespace SpinnySideUp.Models
{
    public class PilotController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static PilotController()
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
        // GET:  Pilot
        // Pilot/List
        // LIST ALL THE  PilotS IN VIEW:
        public ActionResult List()
        {
            string url = " PilotData/GetPilots";// from data controller Get Pilots

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PilotDto> SelectedPilot = response.Content.ReadAsAsync<IEnumerable<PilotDto>>().Result;
                return View(SelectedPilot);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //GET Pilot/Details/3
        public ActionResult Details(int id)
        {
            ShowPilot ViewModels = new ShowPilot();
            string PilotDetailurl = "Pilotdata/findPilot/" + id;
            HttpResponseMessage findPilotresponse = client.GetAsync(PilotDetailurl).Result;

            if (findPilotresponse.IsSuccessStatusCode)
            {

                PilotDto SelectedPilot = findPilotresponse.Content.ReadAsAsync<PilotDto>().Result;
                ViewModels.Pilot = SelectedPilot;

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

        //ADD/CREATE new Pilot
        // GET:Pilot/Create 
        [HttpGet]
        public ActionResult Create()//FORM Imputs from view
        {
            return View();
        }

        //POST Pilot/Create
        //Posts to DB and creats new Pilot if form entries are correct
        //send request to Data controller. 

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Pilot PilotInfo)
        {
            string AddPilotUrl = "PilotData/AddPilot"; //AddPilot model created in data controller
            PilotInfo.UserId = User.Identity.GetUserId();//USER ID WHO CREATES Pilot

            HttpContent content = new StringContent(jss.Serialize(PilotInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage AddPilotResponse = client.PostAsync(AddPilotUrl, content).Result;

            if (AddPilotResponse.IsSuccessStatusCode)
            {

                int PilotId = AddPilotResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = PilotId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
        //UPDATE Pilot
        //EDIT Pilot callls on update 
        //GET: Pilot/Edit/5
        public ActionResult Edit(int id)
        {

            UpdatePilot ViewModels = new UpdatePilot();

            string GetUpdatePilotUrl = "PilotData/findPilot/" + id;//locate Pilot by id
            HttpResponseMessage FindPilotResponse = client.GetAsync(GetUpdatePilotUrl).Result;

            if (FindPilotResponse.IsSuccessStatusCode)
            {
                //Put data into DepartmentDto
                PilotDto SelectedPilot = FindPilotResponse.Content.ReadAsAsync<PilotDto>().Result;
                ViewModels.Pilot = SelectedPilot;

                return View(ViewModels);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //POST UPDATE
        // POST: Pilot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Pilot PilotInfo)
        {
            string PostUpdatePilotUrl = "PilotData/UpdatePilot/" + id;
            PilotInfo.UserId = User.Identity.GetUserId();
            Debug.WriteLine(PilotInfo);
            HttpContent content = new StringContent(jss.Serialize(PilotInfo));
            Debug.WriteLine(PilotInfo);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage UpdatePilotResponse = client.PostAsync(PostUpdatePilotUrl, content).Result;

            if (UpdatePilotResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //DELETE Pilot:
        //Get Request Pilot/Delete/3
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string GetPilotDeleteUrl = "Pilotdata/findPilot/" + id;//find the relevent Pilot
            HttpResponseMessage DeletePilotResponse = client.GetAsync(GetPilotDeleteUrl).Result;

            if (DeletePilotResponse.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                PilotDto SelectedPilot = DeletePilotResponse.Content.ReadAsAsync<PilotDto>().Result;
                return View(SelectedPilot);
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
            string PostPilotDeleteUrl = "Pilotdata/deletePilot/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(PostPilotDeleteUrl, content).Result;

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
