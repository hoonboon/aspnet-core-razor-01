using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebRazor01.Models;

namespace AspNetCoreWebRazor01.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext (DbContextOptions<MyAppContext> options)
            : base(options)
        {
        }

        public DbSet<AspNetCoreWebRazor01.Models.Movie> Movie { get; set; }
    }
}
