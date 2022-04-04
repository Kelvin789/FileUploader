using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using BusinessLogic.BI;
using System.Text;
using BusinessLogic.Helper;
using BusinessLogic.ViewModels;

namespace FileUploader.Controllers
{
    public class UploaderController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadCSV(UploaderLoader UploaderLoader)
        {
            UploaderLoader UL = new UploaderLoader
            {
                ErrorMessage = UploaderLoader.ErrorMessage
            };

            return View(UL);
        }

        [HttpPost]
        public ActionResult UploadCSV(UploaderSubmission UploaderSubmission)
        {
            try
            {
                bool SaveData = true;

                if (UploaderSubmission.FileData != null)
                {
                    byte[] ByteData = Encoding.ASCII.GetBytes(UploaderSubmission.FileData);
                    BytesToFile RebuiltFile = new BytesToFile(ByteData);

                    if (RebuiltFile != null && RebuiltFile._ContentLength > 0)
                    {
                        GymBI GBI = new GymBI();
                        UploaderViewResult IVR = GBI.BeginUploadProcess(RebuiltFile, SaveData);

                        return RedirectToAction("Index", "GymTracker");
                    }
                }

                return RedirectToAction("Index", "GymTracker");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        /// <summary>
        /// Takes a file, rebuilds it, begins upload process to display a test run page
        /// </summary>
        /// <param name="UploaderSubmission"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadCSVTestRun(UploaderSubmission UploaderSubmission)
        {
            try
            {
                bool SaveData = false;
                HttpPostedFileBase CSV = UploaderSubmission.SubmittedCSVFile;

                if (CSV != null && Path.GetExtension(CSV.FileName) == ".csv")
                {
                    if (CSV.ContentLength > 0)
                    {
                        // Saves file data as string into view model
                        using (BinaryReader BR = new BinaryReader(CSV.InputStream))
                        {
                            CSV.InputStream.Position = 0;
                            byte[] FileData = BR.ReadBytes(CSV.ContentLength);
                            UploaderSubmission.FileData = Encoding.UTF8.GetString(FileData);
                        }

                        // Reconstructs file using helper class as file data is wiped after being read once
                        byte[] ByteData = Encoding.ASCII.GetBytes(UploaderSubmission.FileData);
                        BytesToFile RebuiltFile = new BytesToFile(ByteData);

                        // Passes file to be parsed and saved
                        GymBI GBI = new GymBI();
                        UploaderViewResult IVR = GBI.BeginUploadProcess(RebuiltFile, SaveData);

                        // Return if issues were found while parsing or saving
                        if (IVR.Validation != "")
                        {
                            TempData["ErrorMessage"] = IVR.Validation;
                            RedirectToAction("UploadCSV");
                        }

                        UploaderSubmission.UploaderViewResult = new UploaderViewResult
                        {
                            RecordsCreated = IVR.RecordsCreated,
                            RecordBuilderList = IVR.RecordBuilderList
                        };

                        return View(UploaderSubmission);
                    }
                }

                UploaderSubmission.ErrorMessage = "Invalid file input, please enter a csv.";
                return RedirectToActionPermanent("UploadCSV", UploaderSubmission);
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