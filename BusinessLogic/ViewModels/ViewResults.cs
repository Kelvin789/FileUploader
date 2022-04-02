using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class ViewResults
    {
        public List<RecordBuilder> RecordBuilderList { get; set; }
        public string Validation { get; set; }

        [Display(Name = "Records to be created: ")]
        public int RecordCreated { get; set; }
    }
}
