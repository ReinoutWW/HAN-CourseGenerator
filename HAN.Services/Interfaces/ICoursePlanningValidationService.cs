using System.ComponentModel.DataAnnotations;
using HAN.Domain.Entities;

namespace HAN.Services;

public interface ICoursePlanningValidationService
{
    public bool ValidateCoursePlanning(int id);
}