using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    
    public DbSet<Teacher> Teachers { get; set; }  


    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<JobHistory>().HasKey(jh => new { jh.DepartmentId, jh.JobId });
    //     base.OnModelCreating(modelBuilder);
    // }
}