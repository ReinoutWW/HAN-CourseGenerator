﻿using System.ComponentModel.DataAnnotations;

namespace HAN.Services.DTOs;

public class CreateCourseComponentDto
{
    [Required(ErrorMessage = "Name is required.")] 
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
    [MinLength(5, ErrorMessage = "Name cannot exceed 5 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(450, ErrorMessage = "Description cannot exceed 450 characters.")] 
    public string? Description { get; set; }
}