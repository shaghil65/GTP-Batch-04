using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task9
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public List<Course> Courses { get; set; }
    }
}
