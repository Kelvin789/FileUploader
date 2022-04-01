using System;
using System.Collections.Generic;

namespace BusinessLogic.ViewModels
{
    public class ImportViewResult
    {
        public ImportResultSummary ImportResultSummary { get; set; }
        public List<RecordBuilder> RecordBuilderList { get; set; }
        public string Validation { get; set; }
    }
}
