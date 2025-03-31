using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse.API.Data.Entities;

namespace Warehouse.API.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
{
   
    public DbSet<Paper> Papers { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<AppUser>();
        base.OnModelCreating(modelBuilder);
     
        modelBuilder.Entity<AppUser>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Department>()
            .HasData(new Department { Id = 1, Name = "ИТ" });
        
        modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Paper", NormalizedName = "PAPER" },
                new IdentityRole { Id = "3", Name = "Cartridge", NormalizedName = "CARTRIDGE" });
        
        var user = new AppUser
            {
                Id = "1", // Фиксированный Id
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@domain.ru",
                NormalizedEmail = "ADMIN@DOMAIN.RU",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "admin123"),
                SecurityStamp = "security-stamp-2", 
                Firstname = "Admin",
                Lastname = "Admin",
                AvatarImageUrl = "https://example.com/avatar2.jpg",
                DepartmentId = 1,
                RefreshToken = "refresh-token-2",
                RefreshTokenExpiry = new DateTimeOffset(2035, 10, 10, 0, 0, 0, TimeSpan.Zero) 
            };    
        modelBuilder.Entity<AppUser>().HasData(user);
        
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1", 
                UserId = "1"  
            }
        );
    }
}