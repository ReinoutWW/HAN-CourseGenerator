using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests;

public class CourseRepositoryTests : TestBase
{
    private readonly ICourseRepository _repository;
    private readonly AppDbContext _context;
    private const int SeedCourseCount = 2;
    
    public CourseRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<ICourseRepository>();    
        _context = ServiceProvider.GetRequiredService<AppDbContext>();
        
        TestDbSeeder.SeedCourses(_context, SeedCourseCount);
    }

    [Fact]
    public void AddCourse_ShouldAddCourse()
    {
        Course course = new Course()
        {
            Name = "Test Course",
            Description = "Test Description",
        };
        
        var createdCourse = _repository.CreateCourse(course);
        _repository.SaveChanges();
        
        Assert.NotNull(createdCourse);
        Assert.Equal(course.Name, createdCourse.Name);
    }

    [Fact]
    public void CreateCourse_ShouldReturnCourse()
    {
        var courseId = 1;
        Course course = _repository.GetCourseById(courseId);
        
        Assert.NotNull(course);
        Assert.Equal(courseId, course.Id);
    }
    
    [Fact]
    public void ShouldGetAllCourses()
    {
        var CreateCourses = _repository.GetAllCourses().ToList();
        
        Assert.NotEmpty(CreateCourses);
        Assert.Equal(SeedCourseCount, CreateCourses.Count);
    }
    
    [Theory]
    [InlineData("CourseName1")]
    [InlineData("CourseName2")]
    public void CreateCourse_ShouldAddnewCourse(string courseName)
    {
        var newCourse = new Course()
        {
            Name = courseName,
        };

        // Act
        var createdCourse = _repository.CreateCourse(newCourse);
        _repository.SaveChanges();
        
        var Courses = _repository.GetAllCourses().ToList();

        // Assert
        Assert.Contains(Courses, u => u.Name == createdCourse.Name);
    }

    [Theory]
    [InlineData("CourseName11111111111111111111111111111111111111111111111111111111111111111111111111111CourseName11111111111111111111111111111111111111111111111111111111111111111111111111111")]
    [InlineData("")]
    public void CreateCourse_ShouldThrowException_WhenWrongCourseName(string CourseName)
    {
        var newCourse = new Course()
        {
            Name = CourseName
        };
       
        AddCourseExpectValidationException(newCourse);
    }
    
    private void AddCourseExpectValidationException(Course newCourse)
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _repository.CreateCourse(newCourse);
            _repository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<AggregateException>(expectedException);
    }

    [Fact]
    public void CreateCourse_ShouldThrowException_WhenCourseAlreadyExists()
    {
        var Course = new Course() { Id = 1, Name = $"{Guid.NewGuid()}" };
        Assert.ThrowsAny<Exception>(() => _repository.CreateCourse(Course));        
    }

    [Fact]
    public void CreateCourse_ShouldThrowException_WhenCourseIsNull()
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _repository.CreateCourse(null!);
            _repository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<ArgumentException>(expectedException);
    }
}