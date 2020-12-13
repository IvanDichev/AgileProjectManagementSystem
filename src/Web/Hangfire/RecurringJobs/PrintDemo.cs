using Microsoft.Extensions.Logging;
using System;

namespace Web.Hangfire.RecurringJobs
{
    public class PrintDemo : IPrintDemo
    {
        private readonly ILogger<IPrintDemo> logger;

        public PrintDemo(ILogger<IPrintDemo> logger)
        {
            this.logger = logger;
        }

        public void Print(string msg)
        {
            this.logger.LogTrace(msg);

            Console.WriteLine(msg);
        }
    }
}
