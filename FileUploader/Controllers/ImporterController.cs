using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FileUploader.ViewModels;
using BusinessLogic.BI;
using System.Text;
using BusinessLogic.Helper;
using BusinessLogic.ViewModels;
using FileUploader.Models;

namespace FileUploader.Controllers
{
    public class ImporterController : Controller
    {
        // TODO: Import page to file upload > dry run page for results > save > return to index

        [HttpGet]
        public ActionResult Index()
        {
            GymTracker tracker = new GymTracker();

            return View();
        }

        [HttpGet]
        public ActionResult ImportCSV(ImporterLoader ImporterLoader)
        {
            ImporterLoader GymTrackerLoader = new ImporterLoader
            {
                ErrorMessage = ImporterLoader.ErrorMessage
            };

            return View(GymTrackerLoader);
        }

        [HttpPost]
        public ActionResult ImportCSV(ImporterSubmission ImporterSubmission)
        {
            try
            {

                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpPost]
        public ActionResult ImportCSVTestRun(ImporterSubmission IS)
        {
            try
            {
                HttpPostedFileBase SubmittedCSVFile = IS.SubmittedCSVFile;

                if (SubmittedCSVFile != null && Path.GetExtension(SubmittedCSVFile.FileName) == ".csv")
                {
                    if (SubmittedCSVFile.ContentLength > 0)
                    {
                        // Saves file data as string into view model
                        using (BinaryReader BR = new BinaryReader(SubmittedCSVFile.InputStream))
                        {
                            SubmittedCSVFile.InputStream.Position = 0;
                            byte[] FileData = BR.ReadBytes(SubmittedCSVFile.ContentLength);
                            IS.FileData = Encoding.UTF8.GetString(FileData);
                        }

                        // Reconstructs file using helper class as file data is wiped after being read once
                        byte[] ByteData = Encoding.ASCII.GetBytes(IS.FileData);
                        BytesToFile RebuiltFile = new BytesToFile(ByteData);

                        // Passes file to be parsed and saved
                        GymBI GBI = new GymBI();
                        ImportViewResult IVR = GBI.BeginImportProcess(RebuiltFile, false);

                        // Return if issues were found while parsing or saving
                        if (IVR.Validation != "")
                        {
                            TempData["ErrorMessage"] = IVR.Validation;
                            RedirectToAction("ImportCSV");
                        }

                        IS.ImportViewResult = new ImportViewResult
                        {
                            ImportResultSummary = IVR.ImportResultSummary,
                            RecordBuilderList = IVR.RecordBuilderList
                        };

                        return View(IS);
                    }
                }

                IS.ErrorMessage = "Invalid file input, please enter a csv.";
                return RedirectToActionPermanent("ImportCSV", IS);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}