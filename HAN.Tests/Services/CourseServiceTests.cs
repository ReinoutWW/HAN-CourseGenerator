using HAN.Services;
using HAN.Services.DTOs;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseServiceTests : TestBase
{
    private readonly ICourseService _courseService;
    private readonly IEvlService _evlService;
    private const int SeedCourseAmount = 2;
    private const int SeedEvlAmount = 2;
    
    public CourseServiceTests()
    {
        _courseService = ServiceProvider.GetRequiredService<ICourseService>();
        _evlService = ServiceProvider.GetRequiredService<IEvlService>();
        
        TestDbSeeder.SeedCourses(Context, SeedCourseAmount);
        TestDbSeeder.SeedEvls(Context, SeedEvlAmount);
    }

    [Fact]
    public void CreateCourse_ShouldCreateCourse()
    {
        CourseDto course = new()
        {
            Name = "Test Course",
            Description = "Test Description",
        };

        var createdCourse = _courseService.CreateCourse(course);
        
        Assert.NotNull(createdCourse);
        Assert.Equal(course.Name, createdCourse.Name);
        Assert.Equal(course.Description, createdCourse.Description);
    }

    [Fact]
    public void CreateCourse_ShouldThrowException_WhenNameIsNull()
    {   
        Assert.Throws<ArgumentNullException>(() => _courseService.CreateCourse(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("..")]
    [InlineData("industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of sheets containing Lorem Ipsum passages, and more rec")]
    [InlineData(null)]
    public void CreateCourse_ShouldThrowException_WhenNameIsInvalid(string invalidName)
    {
        CourseDto course = new() { Name = invalidName  };
        CreateCourseExpectValidationException(course);
    }

    [Theory]
    [InlineData("\"Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?\"\n\n1914 translation by H. Rackham\n\"But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?\"")]
    public void CreateCourse_ShouldThrowException_WhenDescriptionIsInvalid(string invalidDescription)
    {
        CourseDto course = new()
        {
            Name = "Valid name",
            Description = invalidDescription
        };
        
        CreateCourseExpectValidationException(course);
    }

    [Fact]
    public void AddEvlToCourse_ShouldAddEvl()
    {
        var evl = _evlService.GetEvlById(1);
        var course = _courseService.GetCourseById(1);

        var exception = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(evl.Id, course.Id);
        });
        
        Assert.Null(exception);
    }

    [Fact]
    public void AddEvlToCourse_ShouldThrowException_EvlNotFound()
    {
        var course = _courseService.GetCourseById(1);
        const int nonExistentEvilId = 100000;
        
        var expectedException = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, nonExistentEvilId);
        });
        
        Assert.NotNull(expectedException);
        Assert.IsType<KeyNotFoundException>(expectedException);
    }
    
    [Fact]
    public void AddEvlToCourse_ShouldThrowException_CourseNotFound()
    {
        const int nonExistentCourseId = 100000;
        var evl = _evlService.GetEvlById(1);

        var expectedException = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(nonExistentCourseId, evl.Id);
        });
        
        Assert.NotNull(expectedException);
        Assert.IsType<KeyNotFoundException>(expectedException);
    }

    [Fact]
    public void AddEvlToCourse_ShouldThrowException_EvlAlreadyAdded()
    {
        var evl = _evlService.GetEvlById(1);
        var course = _courseService.GetCourseById(1);
        
        var expectedExceptionNull = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evl.Id);
        });
        
        var expectedException = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evl.Id);
        });
        
        Assert.Null(expectedExceptionNull);
        Assert.NotNull(expectedException);
        Assert.IsType<InvalidOperationException>(expectedException);
    }

    [Fact]
    private void GetAllEvls_ShouldReturnAllEvls()
    {
        var evl = _evlService.GetEvlById(1);
        var course = _courseService.GetCourseById(1);
        
        var expectedExceptionNull = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evl.Id);
        });

        var evls = _courseService.GetEvls(course.Id).ToList();
        
        Assert.Null(expectedExceptionNull);
        Assert.NotNull(evls);
        Assert.Single(evls);
    }
    
    private void CreateCourseExpectValidationException(CourseDto course)
    {
        var expectedException = Record.Exception(() =>
        {
            _courseService.CreateCourse(course);
        });

        Assert.NotNull(expectedException);
        Assert.IsType<HAN.Services.Exceptions.ValidationException>(expectedException);
    }
}