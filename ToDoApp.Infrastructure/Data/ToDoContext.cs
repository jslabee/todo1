using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Data
{
    public class ToDoContext(DbContextOptions<ToDoContext> options) : DbContext(options)
    {
        public DbSet<Opravilo> Opravila { get; set; }
    }
}