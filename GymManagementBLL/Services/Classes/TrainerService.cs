using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;

        public TrainerService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            try
            {
                // Has One Specialty
                var specialty = createTrainerViewModel.Specialty;
                if (!Enum.IsDefined<Specialties>(specialty))
                    return false;

                if (EmailExists(createTrainerViewModel.Email) || PhoneExists(createTrainerViewModel.Phone))
                    return false;
                var Trainer = new Trainer
                {
                    Name = createTrainerViewModel.Name,
                    Email = createTrainerViewModel.Email,
                    Phone = createTrainerViewModel.Phone,
                    DateOfBirth = createTrainerViewModel.DateOfBirth,
                    Gender = createTrainerViewModel.Gender,
                    Specialties = (Specialties)specialty,
                    Address = new Address
                    {
                        BuildingNumber = createTrainerViewModel.BuildingNumber,
                        Street = createTrainerViewModel.Street,
                        City = createTrainerViewModel.City
                    }
                };
                unitOfWork.GetRepository<Trainer>().Add(Trainer);
                return unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTrainer(int trainerId)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null)
                return false;

            // Check If Has Future Sessions
            var ActiveSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.CreatedAt > DateTime.Now);
            if (ActiveSessions.Any())
                return false;

            // Cascading Delete Will Delete
            var TrainerSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId);
            try
            {
                if (TrainerSessions is not null && TrainerSessions.Any())
                {
                    foreach (var Session in TrainerSessions)
                    {
                        unitOfWork.GetRepository<Session>().Delete(Session);
                    }
                }
                unitOfWork.GetRepository<Trainer>().Delete(Trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any())
                return Enumerable.Empty<TrainerViewModel>();

            return trainers.Select(trainer => new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialties.ToString()
            });
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null)
                return null;
            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = $"{trainer.Specialties.ToString()} Trainer",
                DateOfBirth = trainer.DateOfBirth.ToString("dd/MM/yyyy"),
                Address = $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}"
            };
        }

        public TrainerToUpdate? GetTrainerToUpdate(int trainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null)
                return null;
            // Check If Has Future Sessions
            var ActiveSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.CreatedAt > DateTime.Now);
            if (ActiveSessions.Any())
                return null;

            return new TrainerToUpdate
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialties,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City
            };
        }

        public bool UpdateTrainer(int trainerId, TrainerToUpdate trainerToUpdate)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if(Trainer is null)
                return false;

            // Check If Has Future Sessions
            var ActiveSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.CreatedAt > DateTime.Now);
            if (ActiveSessions.Any())
                return false;

            if (EmailExists(trainerToUpdate.Email) || PhoneExists(trainerToUpdate.Phone)) return false;

            Trainer.Email = trainerToUpdate.Email;
            Trainer.Phone = trainerToUpdate.Phone;
            Trainer.Specialties = trainerToUpdate.Specialty;
            Trainer.Address.BuildingNumber = trainerToUpdate.BuildingNumber;
            Trainer.Address.Street = trainerToUpdate.Street;
            Trainer.Address.City = trainerToUpdate.City;

            unitOfWork.GetRepository<Trainer>().Update(Trainer);
            return unitOfWork.SaveChanges() > 0;
        }

        #region Helper
        private bool EmailExists(string email)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        }
        private bool PhoneExists(string phone)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();
        }

        #endregion


    }
}
