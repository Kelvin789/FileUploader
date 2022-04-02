using System.Web;
using BusinessLogic.ViewModels;

namespace FileUploader.ViewModels
{
    public class ImporterSubmission
    {
        public HttpPostedFileBase SubmittedCSVFile { get; set; }
        public string ErrorMessage { get; set; }
        public string FileData { get; set; }
        public ViewResults ImportViewResult { get; set; }
    }
}