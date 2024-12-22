﻿using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IEvlRepository, EvlRepostory>()
            .AddScoped<ICourseRepository, CourseRepository>()
            .AddScoped<IFileRepository, FileRepository>()
            .AddScoped<IScheduleRepository, ScheduleRepository>()
            .AddScoped<ICourseComponentRepository<Lesson>, CourseComponentRepository<Lesson>>()
            .AddScoped<ICourseComponentRepository<Exam>, CourseComponentRepository<Exam>>()
            .AddScoped<ICourseComponentRepository<CourseComponent>, CourseComponentRepository<CourseComponent>>();
    }
}