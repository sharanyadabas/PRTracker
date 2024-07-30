using AutoMapper;
using PRTracker.Entities;
using PRTracker.Models;

namespace PRTracker
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Exercise, CreateExerciseViewModel>();
            CreateMap<CreateExerciseViewModel, Exercise>();

            CreateMap<User, CreateUserViewModel>();
            CreateMap<CreateUserViewModel, User>();

            CreateMap<UserLift, CreateUserLiftViewModel>();
            CreateMap<CreateUserLiftViewModel, UserLift>();
        }
    }
}
