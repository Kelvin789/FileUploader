using System.Web;

namespace BusinessLogic.ViewModels
{
    public class UploaderSubmission
    {
        public HttpPostedFileBase SubmittedCSVFile { get; set; }
        public string ErrorMessage { get; set; }
        public string FileData { get; set; }
        public UploaderViewResult UploaderViewResult { get; set; }
    }
}