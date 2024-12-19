using HAN.Data;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories.Interfaces;

namespace HAN.Repositories;

public class LessonRepository(AppDbContext context) : GenericRepository<Lesson>(context), ILessonRepository
{
    
}