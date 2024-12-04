using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Persistence;

public class CourseComponentRepositoryTests : TestBase
{
    private const int SeedCourseComponentCount = 2;
    private readonly ICourseComponentRepository _repository;

    public CourseComponentRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<ICourseComponentRepository>();
        
        TestDbSeeder.SeedCourseComponents(Context, SeedCourseComponentCount);
    }

    [Fact]
    public void ShouldGetAllCourseComponents()
    {
        var courseComponents = _repository.GetAllCourseComponents().ToList();
        
        Assert.NotEmpty(courseComponents);
        Assert.Equal(SeedCourseComponentCount, courseComponents.Count);
    }

    [Fact]
    public void AddCourseComponent_ShouldAddCourseComponent()
    {
        CourseComponent courseComponent = new CourseComponent()
        {
            Name = "Test CourseComponent",
            Description = "Test Description",
        };
        
        var createdCourseComponent = _repository.CreateCourseComponent(courseComponent);
        _repository.SaveChanges();
        
        Assert.NotNull(createdCourseComponent);
        Assert.Equal(courseComponent.Name, createdCourseComponent.Name);
    }

    [Fact]
    public void CreateCourseComponent_ShouldReturnCourseComponent()
    {
        var courseComponentId = 1;
        var courseComponent = _repository.GetCourseComponentById(courseComponentId);
        
        Assert.NotNull(courseComponent);
        Assert.Equal(courseComponentId, courseComponent.Id);
    }

    [Theory]
    [InlineData("TestCourseComponent1")]
    [InlineData("TestCourseComponent2")]
    public void CreateCourseComponent_ShouldAddNewCourseComponent(string courseComponentName)
    {
        var newCourseComponent = new CourseComponent()
        {
            Name = courseComponentName,
        };

        // Act
        var createdCourseComponent = _repository.CreateCourseComponent(newCourseComponent);
        _repository.SaveChanges();
        var courseComponents = _repository.GetAllCourseComponents().ToList();

        // Assert
        Assert.Contains(courseComponents, u => u.Name == createdCourseComponent.Name);
    }

    [Theory]
    [InlineData("")]
    public void CreateCourseComponent_ShouldThrowException_WhenWrongCourseComponentName(string courseComponentName)
    {
        var newCourseComponent = new CourseComponent()
        {
            Name = courseComponentName
        };
       
        AddCourseComponentExpectValidationException(newCourseComponent);
    }

    [Fact]
    public void CreateCourseComponent_ShouldThrowException_WhenCourseComponentAlreadyExists()
    {
        var CourseComponent = new CourseComponent() { Id = 1, Name = $"{Guid.NewGuid()}" };
        Assert.ThrowsAny<Exception>(() => _repository.CreateCourseComponent(CourseComponent));        
    }

    [Fact]
    public void CreateCourseComponent_ShouldThrowException_WhenCourseComponentIsNull()
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _repository.CreateCourseComponent(null!);
            _repository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<ArgumentException>(expectedException);
    }

    private void AddCourseComponentExpectValidationException(CourseComponent newCourseComponent)
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _repository.CreateCourseComponent(newCourseComponent);
            _repository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<AggregateException>(expectedException);
    }
}