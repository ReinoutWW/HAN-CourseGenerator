﻿namespace HAN.Services.Interfaces;

public interface ICourseValidationService
{
    public bool ValidateCourse(int courseId);    
    public bool IsCourseComplete(int courseId);
    public bool HasCourseValidOrder(int courseId);
}