using Microsoft.EntityFrameworkCore;

namespace SmartHotel.Services.Task.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<Task.Domain.Task> Tasks { get; set; }

        public TaskDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}