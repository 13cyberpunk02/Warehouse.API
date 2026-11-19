using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data.Entities;

namespace Warehouse.API.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
{
   
    public DbSet<Paper> Papers { get; set; }
    public DbSet<Cartridge> Cartridges { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<DeliveryUser> DeliveryUsers { get; set; }
    public DbSet<DeliveryItem> DeliveryItems { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ProcessedFile> ProcessedFiles { get; set; }
    public DbSet<ShortUrl> ShortUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeliveryUser>()
            .HasOne(u => u.Department)
            .WithMany(d => d.DeliveryUsers)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<ShortUrl>(entity =>
        {
            entity.HasIndex(e => e.ShortCode).IsUnique();
        });
        
        modelBuilder.Entity<ProcessedFile>(entity =>
        {
            entity.HasIndex(e => e.ShortCode).IsUnique();
        });
        
        modelBuilder.Entity<Delivery>()
            .HasOne(d => d.DeliveryItem)
            .WithOne(i => i.Delivery)
            .HasForeignKey<Delivery>(d => d.DeliveryItemId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Department>()
            .HasData(
                new Department { Id = 1, Name = "Отдел газоснабжения" },
                new Department { Id = 2, Name = "Сметный отдел" },
                new Department { Id = 3, Name = "Сектор экологии" },
                new Department { Id = 4, Name = "Отдел информ.технологий" },
                new Department { Id = 5, Name = "Планово-экономический отдел" },
                new Department { Id = 6, Name = "Служба эксплуатации" },
                new Department { Id = 7, Name = "Отдел Генподряда" },
                new Department { Id = 8, Name = "Технологический отдел" },
                new Department { Id = 9, Name = "Прооизв.-испыт. отдел" },
                new Department { Id = 10, Name = "Отдел маркетинга" },
                new Department { Id = 11, Name = "Строительный отдел" },
                new Department { Id = 12, Name = "Теплопроект-Т" },
                new Department { Id = 13, Name = "Отдел тепловых сетей" },
                new Department { Id = 14, Name = "Приемная" },
                new Department { Id = 15, Name = "Отдел эл/снабжения" },
                new Department { Id = 16, Name = "ГИПы" },
                new Department { Id = 17, Name = "Отдел ВиК" },
                new Department { Id = 18, Name = "От.техноллог. Програм." },
                new Department { Id = 19, Name = "Джамбула" },
                new Department { Id = 20, Name = "Группа ПОС ППР" },
                new Department { Id = 21, Name = "АУП" },
                new Department { Id = 22, Name = "Технологическая группа №2" },
                new Department { Id = 23, Name = "От.информ.систем и модел." },
                new Department { Id = 24, Name = "Эксплуатационный отдел" },
                new Department { Id = 25, Name = "Отдел магистральных газопроводов" },
                new Department { Id = 26, Name = "Группа КИПиА" },
                new Department { Id = 27, Name = "Гр.генпланирования" },
                new Department { Id = 28, Name = "Технологическая группа №1" },
                new Department { Id = 29, Name = "Транспортная группа" },
                new Department { Id = 30, Name = "Производственный отдел" },
                new Department { Id = 31, Name = "Отдел КИПиА" },
                new Department { Id = 32, Name = "Отдел АСУ" },
                new Department { Id = 33, Name = "Отдел эл/снаб. и пож-охр.сигнал." },
                new Department { Id = 34, Name = "От.техноллог. програм." },
                new Department { Id = 35, Name = "Группа экологии" },
                new Department { Id = 36, Name = "Служба наладки и эксплуатации" },
                new Department { Id = 37, Name = "Технологическая группа №3" },
                new Department { Id = 38, Name = "Группа КИПиА №2" },
                new Department { Id = 39, Name = "Гр.генерального плана" },
                new Department { Id = 40, Name = "Строительно-конструкторский отдел" },
                new Department { Id = 41, Name = "От.спец.программных систем" },
                new Department { Id = 42, Name = "Инженер по ВП" },
                new Department { Id = 43, Name = "Группа КИПиА №1" },
                new Department { Id = 44, Name = "ЗАО Теплопроект-Т" },
                new Department { Id = 45, Name = "Произв.-испыт. отдел" },
                new Department { Id = 56, Name = "ПТО" }
            );
        
        modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = null },
                new IdentityRole { Id = "2", Name = "Paper", NormalizedName = "PAPER", ConcurrencyStamp = null },
                new IdentityRole { Id = "3", Name = "Cartridge", NormalizedName = "CARTRIDGE", ConcurrencyStamp = null });
        
        var user = new List<AppUser>([
            new AppUser
            {
                Id = "1", // Фиксированный Id
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@domain.ru",
                NormalizedEmail = "ADMIN@DOMAIN.RU",
                EmailConfirmed = true,
                // Хеши зафиксированы: HashPassword() в OnModelCreating менял модель при каждой сборке
                PasswordHash = "AQAAAAIAAYagAAAAEJxOXx74exdZIUXkhXhe2pLC+Uk2syR2fJhSOq+c7LFURfpzMnhcPxW9jqxPQH7Mrw==",
                ConcurrencyStamp = "4ffa9886-06c9-42d7-b2d0-42f45170bd36",
                SecurityStamp = "security-stamp-2", 
                Firstname = "Admin",
                Lastname = "Admin",
                RefreshToken = "refresh-token-2",
                RefreshTokenExpiry = new DateTimeOffset(2035, 10, 10, 0, 0, 0, TimeSpan.Zero) 
            },
            new AppUser
            {
                Id = "2", // Фиксированный Id
                UserName = "paper",
                NormalizedUserName = "PAPER",
                Email = "paper@domain.ru",
                NormalizedEmail = "PAPER@DOMAIN.RU",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEHohL/ua2cfRLR77/C2xoVerieNyFUtXnAnf/PN1LS7CfWEQLNGr9QX2DoxRZNmDGw==",
                ConcurrencyStamp = "0ebe8061-da6a-4231-8eb2-2250b49f9552",
                SecurityStamp = "security-stamp-2", 
                Firstname = "Paper",
                Lastname = "Paper",
                RefreshToken = "refresh-token-2",
                RefreshTokenExpiry = new DateTimeOffset(2035, 10, 10, 0, 0, 0, TimeSpan.Zero)
            },
            new AppUser
            {
                Id = "3", // Фиксированный Id
                UserName = "cartridge",
                NormalizedUserName = "CARTRIDGE",
                Email = "cartridge@domain.ru",
                NormalizedEmail = "PAPER@DOMAIN.RU",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEDQq0rPQ0NWuQdD/cQtkWjnYfVWD8fluEt27UIFkj1F1T8nxkn9TBZcgltZaGqjgqQ==",
                ConcurrencyStamp = "064da941-d273-4213-9fa0-1b74a2fcd489",
                SecurityStamp = "security-stamp-2", 
                Firstname = "Cartridge",
                Lastname = "Cartridge",
                RefreshToken = "refresh-token-2",
                RefreshTokenExpiry = new DateTimeOffset(2035, 10, 10, 0, 0, 0, TimeSpan.Zero)
            }
        ]);    
        modelBuilder.Entity<AppUser>().HasData(user);
        
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1", 
                UserId = "1"  
            },
            new IdentityUserRole<string>
            {
                RoleId = "2", 
                UserId = "2"  
            },
            new IdentityUserRole<string>
            {
                RoleId = "3", 
                UserId = "3"  
            }
        );
    }
}