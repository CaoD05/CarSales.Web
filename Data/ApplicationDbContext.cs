using CarSales.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<CarType> CarTypes => Set<CarType>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarImage> CarImages => Set<CarImage>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<PurchaseRequest> PurchaseRequests => Set<PurchaseRequest>();
        public DbSet<Deposit> Deposits => Set<Deposit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasKey(x => x.RoleId);
            modelBuilder.Entity<User>().HasKey(x => x.UserId);
            modelBuilder.Entity<Brand>().HasKey(x => x.BrandId);
            modelBuilder.Entity<CarType>().HasKey(x => x.CarTypeId);
            modelBuilder.Entity<Car>().HasKey(x => x.CarId);
            modelBuilder.Entity<CarImage>().HasKey(x => x.ImageId);
            modelBuilder.Entity<Favorite>().HasKey(x => x.FavoriteId);
            modelBuilder.Entity<PurchaseRequest>().HasKey(x => x.RequestId);
            modelBuilder.Entity<Deposit>().HasKey(x => x.DepositId);

            modelBuilder.Entity<Car>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Deposit>()
                .Property(x => x.DepositAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseRequest>()
                .HasOne(x => x.User)
                .WithMany(x => x.PurchaseRequests)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseRequest>()
                .HasOne(x => x.Staff)
                .WithMany()
                .HasForeignKey(x => x.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deposit>()
                .HasOne(x => x.User)
                .WithMany(x => x.Deposits)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deposit>()
                .HasOne(x => x.Staff)
                .WithMany()
                .HasForeignKey(x => x.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}