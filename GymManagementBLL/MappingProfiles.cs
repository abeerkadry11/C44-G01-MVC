using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(des => des.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(des => des.TrainerName, options => options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(des => des.AvailableSlots, options => options.Ignore());

            // There IS No Difference So You Don't Need Any Configurations
            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
    }
}
