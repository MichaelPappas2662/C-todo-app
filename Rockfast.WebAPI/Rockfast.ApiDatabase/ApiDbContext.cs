using Microsoft.EntityFrameworkCore;
using Rockfast.ApiDatabase.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rockfast.ApiDatabase
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        public ApiDbContext(DbContextOptions options)
            : base(options)
        {
            // this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
