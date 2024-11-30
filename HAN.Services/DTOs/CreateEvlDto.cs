﻿using System.ComponentModel.DataAnnotations;

namespace HAN.Services.DTOs;

public class CreateEvlDto
{
    [Required(ErrorMessage = "Name is required.")] 
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(450, ErrorMessage = "Description cannot exceed 450 characters.")] 
    [DataType(DataType.Text)]
    public string? Description { get; set; }
}