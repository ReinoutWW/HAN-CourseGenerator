﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class Course : BaseEntity
{
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [DataType(DataType.Text)]
    public string? Description { get; set; }
    
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<int> EvlIds { get; set; } = [];
    public Schedule Schedule { get; set; } = new ();
}