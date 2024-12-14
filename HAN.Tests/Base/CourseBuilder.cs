using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;

namespace HAN.Tests.Base;

public class CourseBuilder
{
    private readonly Course _course;
    
    public CourseBuilder()
    {
        _course = DbEntityCreator<Course>.CreateEntity();
        _course.Evls = new List<Evl>();
    }

    public CourseBuilder WithName(string name)
    {
        _course.Name = name;
        return this;
    }

    public CourseBuilder AddValidEvls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var evl = DbEntityCreator<Evl>.CreateEntity();
            evl.Lessons = new List<Lesson> { DbEntityCreator<Lesson>.CreateEntity() };
            evl.Exams = new List<Exam> { DbEntityCreator<Exam>.CreateEntity() };
            _course.Evls.Add(evl);
        }
        return this;
    }

    public CourseBuilder AddInvalidEvls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var evl = DbEntityCreator<Evl>.CreateEntity();
            evl.Lessons = new List<Lesson> { DbEntityCreator<Lesson>.CreateEntity() };
            evl.Exams = null;
            _course.Evls.Add(evl);
            _course.Evls.Add(evl);
        }
        return this;
    }

    public Course Build()
    {
        return _course;
    }
}
