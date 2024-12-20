using HAN.Data.Entities.CourseComponents;
using HAN.Services.DTOs.CourseComponents;

namespace HAN.Services.Mappers;

public static class CourseComponentTypeMap
{
    private static readonly Dictionary<Type, Type> _dtoToEntityMap = new()
    {
        { typeof(LessonDto), typeof(Lesson) },
        { typeof(Lesson), typeof(LessonDto) }
    };

    public static Type GetEntityType(Type dtoType)
    {
        if (_dtoToEntityMap.TryGetValue(dtoType, out var entityType))
        {
            return entityType;
        }

        throw new InvalidOperationException($"No entity type mapping found for DTO type {dtoType.FullName}");
    }
}
