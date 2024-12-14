using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using Microsoft.EntityFrameworkCore;

namespace HAN.Tests.Base;

public static class TestDbSeederValidateCourse
{
    public static int SeedValidCourseForValidation(AppDbContext context)
    {
        const int totalCourses = 1;
        const int validCourseId = 1;
        
        TestDbSeeder.SeedCourses(context, totalCourses);

        var courses = context.Courses
            .Include(c => c.Evls)
            .ToList();

        courses.AddValidEvlsToCources(context, totalCourses);
        
        context.SaveChanges();

        return validCourseId;
    }
    
    public static int SeedInvalidLessonCourseForValidation(AppDbContext context)
    {
        const int totalCourses = 1;
        const int validCourseId = 1;
        
        TestDbSeeder.SeedCourses(context, totalCourses);

        var courses = context.Courses
            .Include(c => c.Evls)
            .ToList();

        courses.AddInvalidLessonsEvlsToCources(context, totalCourses);
        
        context.SaveChanges();

        return validCourseId;
    }
    
    private static void AddValidEvlsToCources(this List<Course> courses, AppDbContext context, int seedEvlsCount)
    {
        courses.ForEach(course =>
            SeedLessonsAndExamsForEvls(context, seedEvlsCount)
                .ForEach(ev => course.Evls.Add(ev)
                ));
        
        context.SaveChanges();
    }
    
    private static void AddInvalidLessonsEvlsToCources(this List<Course> courses, AppDbContext context, int seedEvlsCount)
    {
        courses.ForEach(course =>
            SeedInvalidLessonsAndExamsForEvls(context, seedEvlsCount)
                .ForEach(ev => course.Evls.Add(ev)
                ));
        
        context.SaveChanges();
    }
    
    public static List<Evl> SeedLessonsAndExamsForEvls(AppDbContext context, int seedEvlCount)
    {
        var evls = TestDbSeeder.SeedEvls(context, seedEvlCount);
        
        evls.SeedLessonAndExamForEvls(context);

        return evls;
    }
    
    public static List<Evl> SeedInvalidLessonsAndExamsForEvls(AppDbContext context, int seedEvlCount)
    {
        var evls = TestDbSeeder.SeedEvls(context, seedEvlCount);
        
        evls.SeedMissingLessonAndExamForEvls(context);

        return evls;
    }
    
    private static void SeedMissingLessonAndExamForEvls(this List<Evl> evls, AppDbContext context)
    {
        evls.ForEach(evl =>
        {
            var exam = DbEntityCreator<Exam>.CreateEntity();
            evl.Exams ??= new List<Exam>();
            evl.Exams.Add(exam);
        });
        
        context.SaveChanges();
    }
    
    private static void SeedLessonAndExamForEvls(this List<Evl> evls, AppDbContext context)
    {
        evls.ForEach(evl =>
        {
            var lesson = DbEntityCreator<Lesson>.CreateEntity();
            var exam = DbEntityCreator<Exam>.CreateEntity();

            evl.Exams ??= new List<Exam>();
            evl.Lessons ??= new List<Lesson>();            
            
            evl.Lessons.Add(lesson);
            evl.Exams.Add(exam);
        });
        
        context.SaveChanges();
    }
    
    
    
}