using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

internal class StudentEntity
{
    [Key]
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string CoursesJson { get; set; }
}