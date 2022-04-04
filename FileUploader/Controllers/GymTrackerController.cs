using BusinessLogic.BI;
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
    }
}