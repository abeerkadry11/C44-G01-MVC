using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        #region MyCode

        //IEnumerable<TrainerViewModel> GetAllTrainers();

        //TrainerViewModel? GetTrainerDetails(int trainerId);

        //bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel);

        //TrainerToUpdate? GetTrainerToUpdate(int trainerId);
        //bool UpdateTrainer(int trainerId, TrainerToUpdate trainerToUpdate);

        //bool DeleteTrainer(int trainerId);

        #endregion
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createdTrainer);
        TrainerViewModel? GetTrainerDetails(int trainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);
        bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId);
        bool RemoveTrainer(int trainerId);
    }
}
