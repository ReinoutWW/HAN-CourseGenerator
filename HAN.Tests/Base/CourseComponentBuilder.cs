using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using File = HAN.Data.Entities.File;

namespace HAN.Tests.Base;

public class CourseComponentBuilder
{
    private CourseComponent _courseComponent = DbEntityCreator<CourseComponent>.CreateEntity();

    public CourseComponentBuilder AsLesson()
    {
        _courseComponent = DbEntityCreator<Lesson>.CreateEntity();
        return this;
    }

    public CourseComponentBuilder WithName(string name)
    {
        _courseComponent.Name = name;
        return this;
    }

    public CourseComponentBuilder AddFiles(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var file = DbEntityCreator<File>.CreateEntity();
            _courseComponent.Files.Add(file);
        }
        return this;
    }
    
    public CourseComponentBuilder AddEvls(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var evl = DbEntityCreator<Evl>.CreateEntity();
            _courseComponent.Evls.Add(evl);
        }
        return this;
    }

    public CourseComponent Build()
    {
        return _courseComponent;
    }
}