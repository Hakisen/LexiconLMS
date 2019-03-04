using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LexiconLMS;
using LexiconLMS.Data;
using LexiconLMS.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LexiconLMS
{



    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var config = host.Services.GetRequiredService<IConfiguration>();

                // dotnet user-secrets set  "GYM:AdminPW" "FooBar77!"

                var AdminPw = config["GYM:AdminPW"];
                try
                {
                    SeedData.Initialize(services, AdminPw).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex.Message, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string adminPw)
        {
            using (var context = new ApplicationDbContext(
             serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (context.Course.Any())
                {

                    context.Course.RemoveRange(context.Course);


                }
                if (context.Users.Any()) { 
                  
                context.Users.RemoveRange(context.Users);
                context.UserRoles.RemoveRange(context.UserRoles);

            }

                // Let's seed!
                var courses = new List<Course>();
                for (int i = 0; i < 5; i++)
                {
                    string name = Faker.Company.Name();

                    var course = new Course
                    {
                        Name = name,
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today,
                        Description = Faker.Lorem.Sentence(3),
                   


                    };
                    courses.Add(course);
                }
                context.Course.AddRange(courses);
                context.SaveChanges();

 
                if (roleManager == null || userManager == null)
                {
                    throw new Exception("roleManager or userManager is null");
                }

                var roleNames = new[] { "Student", "Teacher" };

                foreach (var name in roleNames)
                {
                    if (await roleManager.RoleExistsAsync(name)) continue;
                    var role = new IdentityRole { Name = name };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }

                var teacheremails = new[] { "admin@lexicon.se", "stefan@lexicon.se", "John.lexicon.se" };
                foreach (var email in teacheremails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var addUserResult = await userManager.CreateAsync(user, adminPw);
                    if (!addUserResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addUserResult.Errors));
                    }
                    var addToRoleResultAdmin = await userManager.AddToRoleAsync(user, "Teacher");
                    if (!addToRoleResultAdmin.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addToRoleResultAdmin.Errors));
                    }


                }
                var studentemails = new List<string>();
               
                for (int i = 0; i < 30; i++)
                {
                    string name = Faker.Internet.Email();
                   
                    studentemails.Add(name);
                }



                foreach (var email in studentemails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    var randomcourse = Faker.RandomNumber.Next(4);
                    var user = new ApplicationUser { UserName = email, Email = email }; // , CourseId = courses[randomcourse].Id, Course = courses[randomcourse]};
               
                    var addUserResult = await userManager.CreateAsync(user, adminPw);
                    if (!addUserResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addUserResult.Errors));
                    }
                  

                    var addToRoleResultAdmin = await userManager.AddToRoleAsync(user, "Student");
                    if (!addToRoleResultAdmin.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addToRoleResultAdmin.Errors));
                    }
               

                    user.CourseId = courses[randomcourse].Id;
                    var identityResult=await userManager.UpdateAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", identityResult.Errors));
                    }



                }




            }
        }
    }
}


