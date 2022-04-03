using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class UploaderViewResult
    {
        public List<RecordBuilder> RecordBuilderList { get; set; }
        public string Validation { get; set; }

        [Display(Name = "Records to be created: ")]
        public int RecordsCreated { get; set; }
    }
}
