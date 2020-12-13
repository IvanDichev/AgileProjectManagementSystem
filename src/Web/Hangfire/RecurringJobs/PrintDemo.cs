using System;

namespace Web.Hangfire.RecurringJobs
{
    public class PrintDemo : IPrintDemo
    {
        public void Print (string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
