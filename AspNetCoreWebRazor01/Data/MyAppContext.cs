using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebRazor01.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspNetCoreWebRazor01.Data
{
    public class MyAppContext : IdentityDbContext
    {
        public MyAppContext (DbContextOptions<MyAppContext> options)
            : base(options)
        {
        }

        public DbSet<AspNetCoreWebRazor01.Models.Movie> Movie { get; set; }
    }
}
