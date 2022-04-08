using BusinessLogic.BI;
using BusinessLogic.Models;
using FileUploader.Helpers;
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

            GymTrackerListViewModel List = new GymTrackerListViewModel
            {
                GymTrackerList = GBI.GetAllGymTrackerRecords()
            };

            return View(List);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GymTracker GymTracker)
        {
            GymBI GBI = new GymBI();
            GymTracker CreatedGymTracker = GBI.CreateGymTracker(GymTracker);

            if (CreatedGymTracker == null)
            {
                return View(GymTracker);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            GymBI GBI = new GymBI();
            GymTracker GymTracker = GBI.FindGymTracker(ID);

            ModelToVM ModelToVM = new ModelToVM();
            GymTrackerViewModel VM = ModelToVM.GymTrackerToViewModel(GymTracker);

            return View(VM);
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

        [HttpGet]
        public ActionResult Delete(int ID)
        {
            GymBI GBI = new GymBI();
            GymTracker GymTracker = GBI.FindGymTracker(ID);

            ModelToVM ModelToVM = new ModelToVM();
            GymTrackerViewModel VM = ModelToVM.GymTrackerToViewModel(GymTracker);

            return View(VM);
        }

        [HttpPost]
        public ActionResult Delete(GymTracker GymTracker)
        {
            if (ModelState.IsValid)
            {
                GymBI GBI = new GymBI();
                GBI.DeleteGymTracker(GymTracker);
            }
            else
            {
                return View(GymTracker);
            }

            return RedirectToAction("Index");
        }
    }
}