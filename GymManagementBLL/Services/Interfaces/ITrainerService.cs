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
        IEnumerable<TrainerViewModel> GetAllTrainers();

        TrainerViewModel? GetTrainerDetails(int trainerId);

        bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel);

        TrainerToUpdate? GetTrainerToUpdate(int trainerId);
        bool UpdateTrainer(int trainerId, TrainerToUpdate trainerToUpdate);

        bool DeleteTrainer(int trainerId);
    }
}
