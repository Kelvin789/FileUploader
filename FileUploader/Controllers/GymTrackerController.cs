using BusinessLogic.BI;
using BusinessLogic.Models;
using FileUploader.ViewModels;
using System.Web.Mvc;

namespace FileUploader.Controllers
{
    public class GymTrackerController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            GymBI GBI = new GymBI();

            GymTrackerViewModel List = new GymTrackerViewModel
            {
                GymTrackerList = GBI.GetAllGymTrackerRecords()
            };

            return View(List);
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            GymBI GBI = new GymBI();
            GymTracker GymTracker = GBI.FindGymTracker(ID);

            return View(GymTracker);
        }

        [HttpPost]
        public ActionResult Edit(GymTracker GymTracker)
        {
            if (ModelState.IsValid)
            {
                GymBI GBI = new GymBI();
                GBI.EditGymTracker(GymTracker);
            }
            else
            {
                return View(GymTracker);
            }

            return RedirectToAction("Index");
        }
    }
}