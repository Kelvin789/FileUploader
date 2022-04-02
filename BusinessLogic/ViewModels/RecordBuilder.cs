using System;

namespace BusinessLogic.ViewModels
{
    public class RecordBuilder
    {
        public DateTime DateCreated { get; set; }
        public string BodyPart { get; set; }
        public string Exercise { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }
        public string Weights { get; set; }
    }
}