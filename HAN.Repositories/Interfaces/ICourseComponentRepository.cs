using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using File = HAN.Data.Entities.File;

namespace HAN.Repositories.Interfaces;

public interface ICourseComponentRepository<TEntity> : IGenericRepository<TEntity>  where TEntity : CourseComponent
{
    public void AddFileToCourseComponent(int courseComponentId, int fileId);
    public void AddEvlToCourseComponent(int courseComponentId, int evlId);
    List<File> GetFilesForCourseComponent(int courseComponentId);
    List<Evl> GetEvlsForCourseComponent(int courseComponentId);
}