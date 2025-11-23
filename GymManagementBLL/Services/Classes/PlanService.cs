using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlanService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            var planViewModels = plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });

            return planViewModels;
        }

        public PlanViewModel? GetPlanId(int PlanId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
                IsActive = Plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || Plan.IsActive == false) return null;

            // Ensure That Has Not Active Memberships

            if (HasActiveMemberships(PlanId)) return null;

            return new UpdatePlanViewModel()
            {
                PlanName = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
            };
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan is null || HasActiveMemberships(PlanId)) return false;

            //plan.Name = updatedPlan.PlanName;
            //plan.Description = updatedPlan.Description;
            //plan.DurationDays = updatedPlan.DurationDays;
            //plan.Price = updatedPlan.Price;

            // Mapping Using TuPPle
            try
            {
                (plan.Name, plan.Description, plan.DurationDays, plan.Price, plan.UpdatedAt) =
                                      (updatedPlan.PlanName, updatedPlan.Description, updatedPlan.DurationDays, updatedPlan.Price, DateTime.Now);

                unitOfWork.GetRepository<Plan>().Update(plan); // Locally
                return unitOfWork.SaveChanges() > 0; // Commit

            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool ToggleStatus(int PlanId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan is null || HasActiveMemberships(PlanId)) return false;

            //plan.IsActive = !plan.IsActive;
            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.Now;

            try
            {
                unitOfWork.GetRepository<Plan>().Update(plan); // Locally
                return unitOfWork.SaveChanges() > 0; // Commit 

            }
            catch
            {
                return false;
            }
        }


        #region Helper

        private bool HasActiveMemberships(int planId)
        {
            return unitOfWork.GetRepository<MemberShip>().GetAll(X => X.PlanId == planId && X.Status == "Active")
                .Any();

        }


        #endregion


    }
}
