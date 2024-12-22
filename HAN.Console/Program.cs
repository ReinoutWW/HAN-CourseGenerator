// See https://aka.ms/new-console-template for more information

using HAN.Console;
using HAN.Data;
using HAN.Services.DTOs;
using HAN.Services.Extensions;
using HAN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

static IServiceProvider BuildServiceProvider()
{
    var inMemoryDbName = $"InMemDB-{Guid.NewGuid():N}";
        
    var services = new ServiceCollection();

    services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase(inMemoryDbName));
        
    services.AddCourseServices();

    return services.BuildServiceProvider();
}

var serviceProvider = BuildServiceProvider();
var scope = serviceProvider.CreateScope();

var courseService = scope.ServiceProvider.GetRequiredService<ICourseService>();
var evlService = scope.ServiceProvider.GetRequiredService<IEvlService>();

var course = new CourseDto
{
    Name = "HAN I-OOSE",
    Description = "The magical course "
};

List<EvlDto> evls =
[
    new() {
        Name = "GP",
        Description = "Structured programming"
    },
    new() {
        Name = "OOP",
        Description = "Object Oriented Design"
    },
    new()  {
        Name = "Databases",
        Description = "Advanced Database"
    }
];

var createdCourse = courseService.CreateCourse(course);

foreach (var createdEvl in evls.Select(evl => evlService.CreateEvl(evl)))
{
    courseService.AddEvlToCourse(createdCourse.Id, createdEvl.Id);
}

Console.WriteLine("Course created!");
DemoCourseLogger.LogCourse(createdCourse);

var createdEvls = courseService.GetEvls(createdCourse.Id);

Console.WriteLine("----- Total EVLS ------");
for(var i = 0; i < evls.Count; i++)
{
    // ReSharper disable once PossibleMultipleEnumeration
    var evl = createdEvls.ElementAt(i);
    DemoCourseLogger.LogEvl(evl);
}