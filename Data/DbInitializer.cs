using CarSales.Web.Helpers;
using CarSales.Web.Models;

namespace CarSales.Web.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "Staff" },
                    new Role { RoleName = "Customer" }
                );
                context.SaveChanges();
            }

            if (!context.Brands.Any())
            {
                context.Brands.AddRange(
                    new Brand { BrandName = "Toyota", Country = "Nhật Bản", IsActive = true },
                    new Brand { BrandName = "Hyundai", Country = "Hàn Quốc", IsActive = true },
                    new Brand { BrandName = "Kia", Country = "Hàn Quốc", IsActive = true },
                    new Brand { BrandName = "Nissan", Country = "Nhật Bản", IsActive = true },
                    new Brand { BrandName = "Porsche", Country = "Đức", IsActive = true },
                    new Brand { BrandName = "Ferrari", Country = "Ý", IsActive = true },
                    new Brand { BrandName = "BMW", Country = "Đức", IsActive = true },
                    new Brand { BrandName = "Lamborghini", Country = "Ý", IsActive = true }
                );
                context.SaveChanges();
            }

            if (!context.CarTypes.Any())
            {
                context.CarTypes.AddRange(
                    new CarType { TypeName = "Sedan" },
                    new CarType { TypeName = "SUV" },
                    new CarType { TypeName = "Hatchback" },
                    new CarType { TypeName = "Sports" }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                int adminRoleId = context.Roles.First(x => x.RoleName == "Admin").RoleId;
                int staffRoleId = context.Roles.First(x => x.RoleName == "Staff").RoleId;
                int customerRoleId = context.Roles.First(x => x.RoleName == "Customer").RoleId;

                context.Users.AddRange(
                    new User
                    {
                        FullName = "Admin",
                        Email = "admin@gmail.com",
                        PasswordHash = PasswordHelper.HashPassword("123456"),
                        Phone = "0900000001",
                        RoleId = adminRoleId,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        FullName = "Staff",
                        Email = "staff@gmail.com",
                        PasswordHash = PasswordHelper.HashPassword("123456"),
                        Phone = "0900000002",
                        RoleId = staffRoleId,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        FullName = "Customer",
                        Email = "customer@gmail.com",
                        PasswordHash = PasswordHelper.HashPassword("123456"),
                        Phone = "0900000003",
                        RoleId = customerRoleId,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                );

                context.SaveChanges();
            }
            if (!context.Cars.Any())
            {
                int toyotaId = context.Brands.First(x => x.BrandName == "Toyota").BrandId;
                int hyundaiId = context.Brands.First(x => x.BrandName == "Hyundai").BrandId;
                int nissanId = context.Brands.First(x => x.BrandName == "Nissan").BrandId;
                int porscheId = context.Brands.First(x => x.BrandName == "Porsche").BrandId;
                int ferrariId = context.Brands.First(x => x.BrandName == "Ferrari").BrandId;
                int bmwId = context.Brands.First(x => x.BrandName == "BMW").BrandId;
                int lamborghiniId = context.Brands.First(x => x.BrandName == "Lamborghini").BrandId;
                int sedanId = context.CarTypes.First(x => x.TypeName == "Sedan").CarTypeId;
                int suvId = context.CarTypes.First(x => x.TypeName == "SUV").CarTypeId;
                int sportsId = context.CarTypes.First(x => x.TypeName == "Sports").CarTypeId;

                context.Cars.AddRange(
                    new Car
                    {
                        CarName = "Toyota Camry 2.5Q",
                        BrandId = toyotaId,
                        CarTypeId = sedanId,
                        Price = 1450000000,
                        Year = 2023,
                        Mileage = 12000,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 5,
                        Color = "Đen",
                        Engine = "2.5L",
                        Origin = "Nhật Bản",
                        Description = "Mẫu sedan cao cấp, vận hành êm ái.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "Hyundai SantaFe",
                        BrandId = hyundaiId,
                        CarTypeId = suvId,
                        Price = 1290000000,
                        Year = 2022,
                        Mileage = 18000,
                        FuelType = "Dầu",
                        Transmission = "Số tự động",
                        Seats = 7,
                        Color = "Trắng",
                        Engine = "2.2L",
                        Origin = "Hàn Quốc",
                        Description = "SUV gia đình rộng rãi.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "Nissan 350Z",
                        BrandId = nissanId,
                        CarTypeId = sportsId,
                        Price = 2500000000,
                        Year = 2023,
                        Mileage = 0,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 2,
                        Color = "Đỏ",
                        Engine = "3.5L V6",
                        Origin = "Nhật Bản",
                        Description = "Xe thể thao hiệu năng cao với động cơ V6 mạnh mẽ.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "Porsche 911 Carrera 4S",
                        BrandId = porscheId,
                        CarTypeId = sportsId,
                        Price = 9500000000,
                        Year = 2023,
                        Mileage = 0,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 4,
                        Color = "Trắng",
                        Engine = "3.0L Twin-Turbo",
                        Origin = "Đức",
                        Description = "Xe thể thao hạng sang với công nghệ tiên tiến.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "Ferrari 296 GTB",
                        BrandId = ferrariId,
                        CarTypeId = sportsId,
                        Price = 12000000000,
                        Year = 2023,
                        Mileage = 0,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 2,
                        Color = "Đỏ",
                        Engine = "3.0L V6 Hybrid",
                        Origin = "Ý",
                        Description = "Siêu xe Ferrari với công nghệ hybrid tiên tiến.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "Ferrari 488 GTB",
                        BrandId = ferrariId,
                        CarTypeId = sportsId,
                        Price = 11000000000,
                        Year = 2023,
                        Mileage = 0,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 2,
                        Color = "Đỏ",
                        Engine = "3.9L Twin-Turbo",
                        Origin = "Ý",
                        Description = "Xe thể thao Ferrari với hiệu năng vượt trội.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "BMW E92 M3",
                        BrandId = bmwId,
                        CarTypeId = sportsId,
                        Price = 3500000000,
                        Year = 2023,
                        Mileage = 0,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 4,
                        Color = "Xám",
                        Engine = "3.0L Twin-Turbo",
                        Origin = "Đức",
                        Description = "Xe thể thao BMW với thiết kế cổ điển đẹp mắt.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Car
                    {
                        CarName = "Lamborghini Huracan",
                        BrandId = lamborghiniId,
                        CarTypeId = sportsId,
                        Price = 15000000000,
                        Year = 2023,
                        Mileage = 0,
                        FuelType = "Xăng",
                        Transmission = "Số tự động",
                        Seats = 2,
                        Color = "Vàng",
                        Engine = "5.2L V10",
                        Origin = "Ý",
                        Description = "Siêu xe Lamborghini với thiết kế ngoài táo bạo.",
                        Thumbnail = "/uploads/cars/default-car.jpg",
                        Status = "Available",
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    }
                );

                context.SaveChanges();
            }
        }
    }
}