using AutoMapper;
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
        #region My Code

        //private readonly IUnitOfWork unitOfWork;

        //public TrainerService(IUnitOfWork _unitOfWork)
        //{
        //    unitOfWork = _unitOfWork;
        //}
        //public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        //{
        //    try
        //    {
        //        // Has One Specialty
        //        var specialty = createTrainerViewModel.Specialty;
        //        if (!Enum.IsDefined<Specialties>(specialty))
        //            return false;

        //        if (EmailExists(createTrainerViewModel.Email) || PhoneExists(createTrainerViewModel.Phone))
        //            return false;
        //        var Trainer = new Trainer
        //        {
        //            Name = createTrainerViewModel.Name,
        //            Email = createTrainerViewModel.Email,
        //            Phone = createTrainerViewModel.Phone,
        //            DateOfBirth = createTrainerViewModel.DateOfBirth,
        //            Gender = createTrainerViewModel.Gender,
        //            Specialties = (Specialties)specialty,
        //            Address = new Address
        //            {
        //                BuildingNumber = createTrainerViewModel.BuildingNumber,
        //                Street = createTrainerViewModel.Street,
        //                City = createTrainerViewModel.City
        //            }
        //        };
        //        unitOfWork.GetRepository<Trainer>().Add(Trainer);
        //        return unitOfWork.SaveChanges() > 0;

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteTrainer(int trainerId)
        //{
        //    var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
        //    if (Trainer is null)
        //        return false;

        //    // Check If Has Future Sessions
        //    var ActiveSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.CreatedAt > DateTime.Now);
        //    if (ActiveSessions.Any())
        //        return false;

        //    // Cascading Delete Will Delete
        //    var TrainerSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId);
        //    try
        //    {
        //        if (TrainerSessions is not null && TrainerSessions.Any())
        //        {
        //            foreach (var Session in TrainerSessions)
        //            {
        //                unitOfWork.GetRepository<Session>().Delete(Session);
        //            }
        //        }
        //        unitOfWork.GetRepository<Trainer>().Delete(Trainer);
        //        return unitOfWork.SaveChanges() > 0;
        //    }
        //    catch 
        //    {
        //        return false;
        //    }
        //}

        //public IEnumerable<TrainerViewModel> GetAllTrainers()
        //{
        //    var trainers = unitOfWork.GetRepository<Trainer>().GetAll();
        //    if (trainers is null || !trainers.Any())
        //        return Enumerable.Empty<TrainerViewModel>();

        //    return trainers.Select(trainer => new TrainerViewModel
        //    {
        //        Id = trainer.Id,
        //        Name = trainer.Name,
        //        Email = trainer.Email,
        //        Phone = trainer.Phone,
        //        Specialty = trainer.Specialties.ToString()
        //    });
        //}

        //public TrainerViewModel? GetTrainerDetails(int trainerId)
        //{
        //    var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
        //    if (trainer is null)
        //        return null;
        //    return new TrainerViewModel
        //    {
        //        Id = trainer.Id,
        //        Name = trainer.Name,
        //        Email = trainer.Email,
        //        Phone = trainer.Phone,
        //        Specialty = $"{trainer.Specialties.ToString()} Trainer",
        //        DateOfBirth = trainer.DateOfBirth.ToString("dd/MM/yyyy"),
        //        Address = $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}"
        //    };
        //}

        //public TrainerToUpdate? GetTrainerToUpdate(int trainerId)
        //{
        //    var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
        //    if (trainer is null)
        //        return null;
        //    // Check If Has Future Sessions
        //    var ActiveSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.CreatedAt > DateTime.Now);
        //    if (ActiveSessions.Any())
        //        return null;

        //    return new TrainerToUpdate
        //    {
        //        Name = trainer.Name,
        //        Email = trainer.Email,
        //        Phone = trainer.Phone,
        //        Specialty = trainer.Specialties,
        //        BuildingNumber = trainer.Address.BuildingNumber,
        //        Street = trainer.Address.Street,
        //        City = trainer.Address.City
        //    };
        //}

        //public bool UpdateTrainer(int trainerId, TrainerToUpdate trainerToUpdate)
        //{
        //    var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
        //    if(Trainer is null)
        //        return false;

        //    // Check If Has Future Sessions
        //    var ActiveSessions = unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.CreatedAt > DateTime.Now);
        //    if (ActiveSessions.Any())
        //        return false;

        //    if (EmailExists(trainerToUpdate.Email) || PhoneExists(trainerToUpdate.Phone)) return false;

        //    Trainer.Email = trainerToUpdate.Email;
        //    Trainer.Phone = trainerToUpdate.Phone;
        //    Trainer.Specialties = trainerToUpdate.Specialty;
        //    Trainer.Address.BuildingNumber = trainerToUpdate.BuildingNumber;
        //    Trainer.Address.Street = trainerToUpdate.Street;
        //    Trainer.Address.City = trainerToUpdate.City;

        //    unitOfWork.GetRepository<Trainer>().Update(Trainer);
        //    return unitOfWork.SaveChanges() > 0;
        //}

        //#region Helper
        //private bool EmailExists(string email)
        //{
        //    return unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        //}
        //private bool PhoneExists(string phone)
        //{
        //    return unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();
        //}

        //#endregion

        #endregion

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
            mapper = _mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();

                var EmailExists = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Email == createdTrainer.Email && m.Name != createdTrainer.Name).Any();
                var PhoneExists = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Phone == createdTrainer.Phone && m.Name != createdTrainer.Name).Any();

                if (EmailExists || PhoneExists) return false;
                var Trainer = mapper.Map<Trainer>(createdTrainer);


                Repo.Add(Trainer);

                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];

            return mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;


            return mapper.Map<TrainerViewModel>(Trainer);
        }
        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;

            return mapper.Map<TrainerToUpdateViewModel>(Trainer);
        }
        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(trainerId);

            var EmailExists = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Email == updatedTrainer.Email && m.Id != trainerId).Any();
            var PhoneExists = _unitOfWork.GetRepository<Trainer>().GetAll(
            m => m.Phone == updatedTrainer.Phone && m.Id != trainerId).Any();

            if (TrainerToUpdate is null || EmailExists || PhoneExists) return false;
            //if (TrainerToUpdate is null || IsEmailExists(updatedTrainer.Email) || IsPhoneExists(updatedTrainer.Phone)) return false;

            mapper.Map(updatedTrainer, TrainerToUpdate);

            Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.Email == email).Any();
            return existing;
        }

        private bool IsPhoneExists(string phone)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                m => m.Phone == phone).Any();
            return existing;
        }

        private bool HasActiveSessions(int Id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(
               s => s.TrainerId == Id && s.CreatedAt > DateTime.Now).Any();
            return activeSessions;
        }
        #endregion

    }
}
