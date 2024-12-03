// See https://aka.ms/new-console-template for more information

using HAN.Console;
using HAN.Services;
using HAN.Services.DTOs;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

// For demo purposes, we use the Test serviceprovider
var serviceProvider = TestServiceProvider.BuildServiceProvider();
var scope = serviceProvider.CreateScope();

var courseService = scope.ServiceProvider.GetRequiredService<ICourseService>();
var evlService = scope.ServiceProvider.GetRequiredService<IEvlService>();

var course = new CreateCourseDto
{
    Name = "HAN I-OOSE",
    Description = "The magical course "
};

List<CreateEvlDto> evls =
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