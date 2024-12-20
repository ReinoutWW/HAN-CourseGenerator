using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;

namespace HAN.Services.Interfaces;

public interface ICourseComponentService
{
    public T CreateCourseComponent<T>(T courseComponentDto) where T : CourseComponentDto;
    public void AddEvlToCourseComponent(int courseComponentId, int evlId);
    public void AddFileToCourseComponent(int courseComponentId, int fileId);
    public List<FileDto> GetFilesForCourseComponent(int courseComponentId);
    public List<EvlDto> GetEvlsForCourseComponent(int courseComponentId);
    public T GetCourseComponentById<T>(int id) where T : CourseComponentDto;
}