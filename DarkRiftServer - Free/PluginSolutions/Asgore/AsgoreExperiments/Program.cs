using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asgore;

namespace AsgoreExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AsgoreDatabaseContext("test.sql"))
            {
                db.Database.EnsureCreated();
                var u = new User { Name = "Luke" };
                db.Users.Add(u);
                db.SaveChanges();
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
