﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class CourseComponent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] 
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [Required] 
    [DataType(DataType.Text)]
    public string Desciption { get; set; } = string.Empty;

    public List<Evl> Evls { get; set; } = [];
    public List<File> Files { get; set; } = [];
}