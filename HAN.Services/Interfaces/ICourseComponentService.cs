using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;

namespace HAN.Services.Interfaces;

public interface ICourseComponentService<TDto> where TDto : CourseComponentDto
{
    TDto CreateCourseComponent(TDto courseComponentDto);
    void AddEvlToCourseComponent(int courseComponentId, int evlId);
    void AddFileToCourseComponent(int courseComponentId, int fileId);
    List<FileDto> GetFilesForCourseComponent(int courseComponentId);
    List<EvlDto> GetEvlsForCourseComponent(int courseComponentId);
    TDto GetCourseComponentById(int id);
}
