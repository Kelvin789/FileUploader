using System;
using System.ComponentModel.DataAnnotations;

namespace FileUploader.ViewModels
{
    public class GymTrackerViewModel
    {
        public int ID { get; set; }

        [Display(Name="Date")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Body Part")]
        public string BodyPart { get; set; }

        [Display(Name = "Exercise")]
        public string Exercise { get; set; }

        [Display(Name = "Sets")]
        public string Sets { get; set; }

        [Display(Name = "Reps")]
        public string Reps { get; set; }

        [Display(Name = "Weights")]
        public string Weights { get; set; }
    }
}