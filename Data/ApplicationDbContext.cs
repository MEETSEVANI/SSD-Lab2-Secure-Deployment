using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Models;

namespace SSD_Lab1.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<OfficeLocation> OfficeLocations => Set<OfficeLocation>();
    public DbSet<BoardMessage> BoardMessages => Set<BoardMessage>();
}
