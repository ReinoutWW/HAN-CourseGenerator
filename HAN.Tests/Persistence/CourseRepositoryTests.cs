using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Persistence;

public class CourseRepositoryTests : TestBase
{
    private readonly ICourseRepository _repository;

    public CourseRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<ICourseRepository>();
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
        _repository.Add(newCourse);
        var courses = _repository.GetAll().ToList();

        // Assert
        Assert.Contains(courses, u => u.Name == newCourse.Name);
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
        
        var course = new Course() { Id = 1, Name = $"{Guid.NewGuid()}" };
        _repository.Add(course);
        
        Assert.ThrowsAny<Exception>(() => _repository.Add(course));        
    }

    [Fact]
    public void CreateCourse_ShouldThrowException_WhenCourseIsNull()
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _repository.Add(null!);
        });

        Assert.NotNull(expectedException);
        Assert.IsType<ArgumentNullException>(expectedException);
    }

    private void AddCourseExpectValidationException(Course newCourse)
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _repository.Add(newCourse);
        });

        Assert.NotNull(expectedException);
        Assert.IsType<AggregateException>(expectedException);
    }
}