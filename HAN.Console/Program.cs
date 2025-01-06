// See https://aka.ms/new-console-template for more information

using HAN.Client.API.RabbitMQ;
using HAN.Console;

var course = new Course
{
    Name = "HAN I-OOSE",
    Description = "The magical course "
};

var httpClient = new HttpClient();
var authService = new AuthService(httpClient);

var accessToken = await authService.LoginAsync("alice", "password");

var courseService = new CourseService();

courseService.CreateCourse(course, accessToken);

Console.WriteLine("Message sent!");
Console.ReadLine();