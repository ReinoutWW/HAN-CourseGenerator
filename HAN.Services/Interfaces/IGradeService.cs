using HAN.Utilities.Messaging.Models;

namespace HAN.Services.Interfaces;

public interface IGradeService
{
    Task SaveGradeAsync(string studentId, string courseId, string grade);
    Task<List<GradeRecord>> GetGradesAsync(string studentId);
}