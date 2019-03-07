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

                // dotnet user-secrets set  "LexiconLMS:AdminPW": "Q1!qwerty"

                var AdminPw = config["LexiconLMS:AdminPW"];
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
        internal class LmsUser
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string UserName { get; set; }
        }


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
                    context.Module.RemoveRange(context.Module);


                }
                if (context.Users.Any())
                {
                    context.Users.RemoveRange(context.Users);
                  
                   
                }
                context.SaveChanges();
                // Let's seed!
                var courses = new List<Course>();
                var courseNames = new[] { "Java Grundkurs", "Java Avanserad", "Unix från grunden", "DotNet för alla", "DotNet för webben" };
                

                    foreach (var name in courseNames)
                    {




                        var course = new Course
                        {
                            Name = name,
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today.AddDays(7),
                            Description = Faker.Lorem.Sentence(3),
                        };
                        courses.Add(course);
                    }
                
                    context.Course.AddRange(courses);
                    context.SaveChanges();
              
                var modules = new List<Module>();

                

                    for (int i = 20; i < 40; i++)
                    {





                        var random = Faker.RandomNumber.Next(4);
                        var module = new Module
                        {
                            Name = $"module{i}",
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today.AddDays(7),
                            Description = Faker.Lorem.Sentence(3),

                            CourseId = courses[random].Id
                        };
                        modules.Add(module);
                    }
                    context.Module.AddRange(modules);
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
                //if (!context.Users.Any())
                //{


                var teacheremails = new[] { "admin@lexicon.se", "stefan@lexicon.se", "John@lexicon.se", "teacher1@lexicon.se", "teacher2@lexicon.se", "teacher3@lexicon.se" };
                foreach (var email in teacheremails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    var user = new ApplicationUser { UserName = email, Email = email/*, PhoneNumber = Faker.Phone.()*/ };
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
                var students = new List<LmsUser>();

                for (int i = 0; i < 20; i++)
                {
                    var fakename = $"Elev{i}";
                    LmsUser newStudent = new LmsUser {UserName= $"{fakename}@lexicon.se", Email = $"{fakename}@lexicon.se", Name = $"{fakename} Stensson"/*, PhoneNumber = Faker.Phone.Number()*/ };


                    students.Add(newStudent);
                }



                foreach (var student in students)
                {
                    var foundUser = await userManager.FindByEmailAsync(student.Email);
                    if (foundUser != null) continue;

                    var user = new ApplicationUser { UserName = student.Email, Email = student.Email, Name = student.Name/*,PhoneNumber=student.PhoneNumber*/ }; // , CourseId = courses[randomcourse].Id, Course = courses[randomcourse]};

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

                    var randomcourse = Faker.RandomNumber.Next(4);
                    user.CourseId = courses[randomcourse].Id;
                    var identityResult = await userManager.UpdateAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", identityResult.Errors));
                    }
                }
            }

        }
    }
}


