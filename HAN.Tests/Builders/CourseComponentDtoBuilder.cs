using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Tests.Base;

namespace HAN.Tests.Builders;

public class CourseComponentDtoBuilder : CourseComponentDto
{
    private CourseComponentDto _courseComponent = DbEntityCreator<CourseComponentDto>.CreateEntity();

    public CourseComponentDtoBuilder AsLesson()
    {
        _courseComponent = DbEntityCreator<LessonDto>.CreateEntity();
        return this;
    }
    
    public CourseComponentDtoBuilder AsExam()
    {
        _courseComponent = DbEntityCreator<ExamDto>.CreateEntity();
        return this;
    }
    
    public CourseComponentDtoBuilder WithName(string name)
    {
        _courseComponent.Name = name;
        return this;
    }

    public CourseComponentDtoBuilder AddFiles(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var file = DbEntityCreator<FileDto>.CreateEntity();
            _courseComponent.Files ??= [];
            _courseComponent.Files.Add(file);
        }
        return this;
    }
    
    public CourseComponentDtoBuilder WithEvl(EvlDto evl)
    {
        _courseComponent.Evls ??= [];
        _courseComponent.Evls.Add(evl);        
        return this;
    }

    public CourseComponentDto Build()
    {
        return _courseComponent;
    }
}