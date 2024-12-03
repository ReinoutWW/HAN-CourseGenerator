using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Persistence;

public class CourseRepositoryTests : TestBase
{
    private const int SeedCourseCount = 2;
    private readonly ICourseRepository _repository;

    public CourseRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<ICourseRepository>();
        
        TestDbSeeder.SeedCourses(Context, SeedCourseCount);
    }

    [Fact]
    public void ShouldGetAllCourses()
    {
        var courses = _repository.GetAllCourses().ToList();
        
        Assert.NotEmpty(courses);
        Assert.Equal(SeedCourseCount, courses.Count);
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
        var course = _repository.GetCourseById(courseId);
        
        Assert.NotNull(course);
        Assert.Equal(courseId, course.Id);
    }

    [Theory]
    [InlineData("TestCourse1")]
    [InlineData("TestCourse2")]
    public void CreateCourse_ShouldAddNewCourse(string courseName)
    {
        var newCourse = new Course()
        {
            Name = courseName,
        };

        // Act
        var createdCourse = _repository.CreateCourse(newCourse);
        _repository.SaveChanges();
        var courses = _repository.GetAllCourses().ToList();

        // Assert
        Assert.Contains(courses, u => u.Name == createdCourse.Name);
    }

    [Theory]
    [InlineData("")]
    public void CreateCourse_ShouldThrowException_WhenWrongCourseName(string courseName)
    {
        var newCourse = new Course()
        {
            Name = courseName
        };
       
        AddCourseExpectValidationException(newCourse);
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
}