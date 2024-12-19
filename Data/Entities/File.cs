using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class File : BaseEntity
{
    public string Content { get; set; } = string.Empty;
}