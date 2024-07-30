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
            CreateMap<Exercise, UpdateExerciseViewModel>();
            CreateMap<CreateExerciseViewModel, Exercise>();
            CreateMap<CreateExerciseViewModel, Exercise>();
        }
    }
}
