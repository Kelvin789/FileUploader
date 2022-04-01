using System;

namespace BusinessLogic.ViewModels
{
    public class RecordBuilder
    {
        public DateTime DateTime { get; set; }
        public string BodyPart { get; set; }
        public string Exercise { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
    }
}