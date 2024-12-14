using HAN.Data.Entities;

namespace HAN.Repositories.Interfaces;

public interface ICourseComponentRepository : IGenericRepository<CourseComponent>
{
    IEnumerable<Evl> GetEvlsByCourseComponentId(int id);
    void AddEvlToCourseComponent(int courseComponentId, int evlId);
}