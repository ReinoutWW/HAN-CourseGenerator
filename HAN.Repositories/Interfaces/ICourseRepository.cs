using HAN.Data.Entities;

namespace HAN.Repositories.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
    IEnumerable<Evl> GetEvlsByCourseId(int id);
    void AddEvlToCourse(int courseId, int evlId);
}