﻿using System;
using System.Collections.Generic;
using System.Text;
using LexiconLMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LexiconLMS.Data
{
    //    public class ApplicationDbContext : IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LexiconLMS.Models.Course> Course { get; set; }
        public DbSet<LexiconLMS.Models.Module> Module { get; set; }
        public DbSet<LexiconLMS.Models.LmsActivity> LmsActivity { get; set; }
        public DbSet<LexiconLMS.Models.ActivityType> ActivityType { get; set; }
        public DbSet<LexiconLMS.Models.ReadyState> ReadyState { get; set; }
        public DbSet<LexiconLMS.Models.Document> Document { get; set; }
        public DbSet<LexiconLMS.Models.LmsTask> LmsTask { get; set; }
    }
}
