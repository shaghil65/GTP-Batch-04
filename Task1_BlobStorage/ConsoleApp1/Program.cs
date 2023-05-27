using System;


namespace ConsoleApp1
{
    internal class Program
    {
 
        static void Main(string[] args)
        {
            Console.WriteLine("Please mention size of chunk?");
            int size = Convert.ToInt32(Console.ReadLine());
            Uploader u = new Uploader();
            u.Upload(size);
        }
    }
}