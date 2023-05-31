using Newtonsoft.Json;
using System.Xml;
using Task9;

public class Program
{
    public static void Main(String[] args)
    {
        var student = new Student();
        student.Courses = new List<Course>();

        Console.WriteLine("Enter student details:");
        Console.Write("Student ID: ");
        student.StudentId = int.Parse(Console.ReadLine());

        Console.Write("Student Name: ");
        student.StudentName = Console.ReadLine();

        string courseId;
        do
        {
            Console.WriteLine("Enter course details or type exit:");
            Console.Write("Course ID: ");
            courseId = Console.ReadLine();

            if (courseId.ToLower() == "exit")
                break;

            Console.Write("Course Name: ");
            var courseName = Console.ReadLine();

            var course = new Course
            {
                CourseId = int.Parse(courseId),
                CourseName = courseName
            };

            student.Courses.Add(course);

        } while (true);

        string json = JsonConvert.SerializeObject(student);

        var studentEntity = new StudentEntity
        {
            StudentName = student.StudentName,
            CoursesJson = json
        };

        using (var context = new AppicationDbContext())
        {
            context.Students.Add(studentEntity);
            context.SaveChanges();
        }
        Console.ReadLine();
    }

}