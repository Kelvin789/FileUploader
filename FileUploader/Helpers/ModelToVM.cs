using BusinessLogic.Models;
using FileUploader.ViewModels;

namespace FileUploader.Helpers
{
    public class ModelToVM
    {
        public GymTrackerViewModel GymTrackerToViewModel(GymTracker GymTracker)
        {
            GymTrackerViewModel VM = new GymTrackerViewModel()
            {
                ID = GymTracker.ID,
                DateCreated = GymTracker.DateCreated,
                BodyPart = GymTracker.BodyPart,
                Exercise = GymTracker.Exercise,
                Sets = GymTracker.Sets,
                Reps = GymTracker.Reps,
                Weights = GymTracker.Weights
            };

            return VM;
        }
    }
}