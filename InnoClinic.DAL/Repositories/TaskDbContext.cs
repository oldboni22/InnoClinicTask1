using InnoClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.DAL.Repositories;

public class TaskDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}