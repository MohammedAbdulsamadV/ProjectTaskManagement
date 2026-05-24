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
    }
}