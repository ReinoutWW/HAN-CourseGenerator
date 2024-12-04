using HAN.Data.Entities;

namespace HAN.Repositories.Interfaces;

public interface ICourseComponentRepository
{
    bool SaveChanges();
    CourseComponent CreateCourseComponent(CourseComponent courseComponent);
    CourseComponent GetCourseComponentById(int id);
    IEnumerable<CourseComponent> GetAllCourseComponents();
    bool CourseComponentExists(int courseComponentId);
    IEnumerable<Evl> GetEvlsByCourseComponentId(int id);
    void AddEvlToCourseComponent(int courseId, int evlId);
}