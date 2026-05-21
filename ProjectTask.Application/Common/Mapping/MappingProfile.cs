using AutoMapper;
using ProjectTask.Application.DTOs;
using ProjectTask.Domain.Entities;
using Task = ProjectTask.Domain.Entities.Task;

namespace ProjectTask.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();

        CreateMap<Task, TaskDto>()
            .ForMember(dest => dest.Status, 
                opt =>
                    opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Priority, 
                opt => 
                    opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateTaskDto, Task>();
    }
}