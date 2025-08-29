using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.Models;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = BubbleTeaShop.Backend.Enums.DayOfWeek;

namespace BubbleTeaShop.Backend.Helpers;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuItemAllergen> MenuItemAllergens { get; set; }
    public DbSet<MilkTea> MilkTeas { get; set; }
    public DbSet<FruitTea> FruitTeas { get; set; }
    public DbSet<Frappe> Frappes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<OrderLineToppingMapping> OrderLineToppingMappings { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<AssignmentHistory> AssignmentHistories { get; set; }
    public DbSet<EmployeeCertificate> EmployeeCertificates { get; set; }
    public DbSet<EmployeeRoleMapping> EmployeeRoleMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // -------------------------------------- INHERITANCE (MenuItem) ----------------------------------------
        modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
        modelBuilder.Entity<FruitTea>().ToTable("FruitTeas");
        modelBuilder.Entity<Frappe>().ToTable("Frappes");
        modelBuilder.Entity<MilkTea>().ToTable("MilkTeas");

        // Ensure MenuItemAllergen has its own table
        modelBuilder.Entity<MenuItemAllergen>().ToTable("MenuItemAllergens");

        // Many-to-many MenuItem <-> MenuItemAllergen:
        // Use a separate join table name to avoid colliding with MenuItemAllergens table.
        modelBuilder.Entity<MenuItem>()
            .HasMany(mi => mi.MenuItemAllergens)
            .WithMany(a => a.MenuItems)
            .UsingEntity(j => j.ToTable("MenuItem_MenuItemAllergens"));

        // ---------------------------- ASSOCIATION WITH ATTRIBUTE (OrderLine) ----------------------------------
        // OrderLine now uses a surrogate primary key (Id).
        modelBuilder.Entity<OrderLine>()
            .HasKey(ol => ol.Id);

        // multi-value Toppings table and FK to OrderLine.Id
        modelBuilder.Entity<OrderLineToppingMapping>()
            .ToTable("OrderLineToppings");

        modelBuilder.Entity<OrderLineToppingMapping>()
            .HasKey(olt => new { olt.OrderLineId, olt.Topping });

        modelBuilder.Entity<OrderLineToppingMapping>()
            .HasOne(olt => olt.OrderLine)
            .WithMany(ol => ol.OrderLineToppings)
            .HasForeignKey(olt => olt.OrderLineId);

        // OrderLine 1..* - Order 1
        modelBuilder.Entity<OrderLine>()
            .HasOne(ol => ol.Order)
            .WithMany(o => o.OrderLines)
            .HasForeignKey(ol => ol.OrderId);

        // OrderLine 0..* - MenuItem 1
        modelBuilder.Entity<OrderLine>()
            .HasOne(ol => ol.MenuItem)
            .WithMany(mi => mi.OrderLines)
            .HasForeignKey(ol => ol.MenuItemId);

        // --------------------------- BASIC ASSOCIATION (MenuItem - Store) -------------------------------------
        modelBuilder.Entity<MenuItem>()
            .HasMany(mi => mi.Stores)
            .WithMany(s => s.MenuItems)
            .UsingEntity(j => j.ToTable("MenuItemStores"));

        // -------------------- ASSOCIATION WITH ATTRIBUTE (AssignmentHistory) ----------------------------------
        modelBuilder.Entity<AssignmentHistory>()
            .HasKey(ah => ah.Id);

        modelBuilder.Entity<AssignmentHistory>()
            .HasOne(ah => ah.Store)
            .WithMany(s => s.AssignmentHistories)
            .HasForeignKey(ah => ah.StoreId);

        modelBuilder.Entity<AssignmentHistory>()
            .HasOne(ah => ah.Employee)
            .WithMany(e => e.AssignmentHistories)
            .HasForeignKey(ah => ah.EmployeeId);

        // ------------------------------- SUBSET ASSOCIATION (EMPLOYEE - SHIFT) -------------------------------
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.WorksInShifts)
            .WithMany(s => s.Employees)
            .UsingEntity(j => j.ToTable("EmployeeWorksInShifts"));

        modelBuilder.Entity<Shift>()
            .HasOne(s => s.ManagingEmployee)
            .WithMany(e => e.ManagedShifts)
            .HasForeignKey(s => s.ManagingEmployeeId)
            .IsRequired(false);

        // ----------------------------------------- CONSTRAINTS AND VALIDATIONS -------------------------------------------
        modelBuilder.Entity<MenuItem>()
            .ToTable(t => t.HasCheckConstraint("CK_MenuItem_BasePrice", "[BasePrice] > 0"));
        modelBuilder.Entity<MenuItem>()
            .ToTable(t => t.HasCheckConstraint("CK_MenuItem_StockQuantity", "[StockQuantity] >= 0"));
        modelBuilder.Entity<MenuItem>()
            .ToTable(t => t.HasCheckConstraint("CK_MenuItem_Name", "[Name] <> ''"));

        modelBuilder.Entity<FruitTea>()
            .ToTable(t => t.HasCheckConstraint("CK_FruitTea_IceLevel", "[IceLevel] BETWEEN 0 AND 3"));
        modelBuilder.Entity<FruitTea>()
            .ToTable(t => t.HasCheckConstraint("CK_FruitTea_FruitBase", "[FruitBase] BETWEEN 0 AND 3"));
        modelBuilder.Entity<FruitTea>()
            .ToTable(t => t.HasCheckConstraint("CK_FruitTea_TeaBase", "[TeaBase] BETWEEN 0 AND 2"));

        modelBuilder.Entity<MilkTea>()
            .ToTable(t => t.HasCheckConstraint("CK_MilkTea_TeaBase", "[TeaBase] BETWEEN 0 AND 2"));
        modelBuilder.Entity<MilkTea>()
            .ToTable(t => t.HasCheckConstraint("CK_MilkTea_MilkBase", "[MilkBase] BETWEEN 0 AND 1"));

        modelBuilder.Entity<Frappe>()
            .ToTable(t => t.HasCheckConstraint("CK_Frappe_BaseFlavor", "[BaseFlavor] BETWEEN 0 AND 2"));

        modelBuilder.Entity<OrderLine>()
            .ToTable(t => t.HasCheckConstraint("CK_OrderLine_Quantity", "[Quantity] >= 1"));
        modelBuilder.Entity<OrderLineToppingMapping>()
            .ToTable(t => t.HasCheckConstraint("CK_OrderLineToppingMapping_Topping", "[Topping] BETWEEN 0 AND 2"));
        modelBuilder.Entity<OrderLine>()
            .ToTable(t => t.HasCheckConstraint("CK_OrderLine_Size", "[Size] BETWEEN 0 AND 2"));

        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<Order>()
            .ToTable(t => t.HasCheckConstraint("CK_Order_Status", "[Status] BETWEEN 0 AND 5"));
        modelBuilder.Entity<Order>()
            .ToTable(t => t.HasCheckConstraint("CK_Order_OrderDateTime", "[OrderDateTime] > '0001-01-01'"));

        modelBuilder.Entity<Store>()
            .ToTable(t => t.HasCheckConstraint("CK_Store_Name", "LENGTH([Name]) > 0"));
        modelBuilder.Entity<Store>()
            .ToTable(t => t.HasCheckConstraint("CK_Store_Location", "LENGTH([Location]) > 0"));

        modelBuilder.Entity<EmployeeCertificate>().ToTable("EmployeeCertificates");

        // EmployeeRoleMapping: composite PK and map enum to column EmployeeRole
        modelBuilder.Entity<EmployeeRoleMapping>(entity =>
        {
            // map to table and add check constraint via later call (or you can inline)
            entity.ToTable("EmployeeRoleMappings");

            entity.HasKey(e => new { e.EmployeeId, e.Role });

            entity.Property(e => e.Role)
                  .HasConversion<int>()
                  .HasColumnName("EmployeeRole");

            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.EmployeeRoles)
                  .HasForeignKey(e => e.EmployeeId)
                  .IsRequired();
        });

        // Apply the check constraint for EmployeeRoleMapping table
        modelBuilder.Entity<EmployeeRoleMapping>()
            .ToTable(t => t.HasCheckConstraint("CK_EmployeeRoleMapping_EmployeeRole", "[EmployeeRole] BETWEEN 0 AND 2"));

        modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("CK_Employee_HygieneScore",
                "[HygieneScore] IS NULL OR ([HygieneScore] >= 0 AND [HygieneScore] <= 10)"));
        modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("CK_Employee_RegisterProficiency",
                "[RegisterProficiency] IS NULL OR ([RegisterProficiency] >= 0 AND [RegisterProficiency] <= 10)"));
        modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("CK_Employee_Email", "[Email] LIKE '_%@_%.__%'"));
        modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("CK_Employee_Phone", "LENGTH([Phone]) = 9"));
        modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("CK_Employee_Salary", "[Salary] > 0"));
        modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("CK_Employee_TrainingSessions",
                "[TrainingSessionsConducted] IS NULL OR [TrainingSessionsConducted] >= 0"));

        modelBuilder.Entity<AssignmentHistory>()
            .ToTable(t => t.HasCheckConstraint("CK_AssignmentHistory_EndDate", "[EndDate] > [StartDate]"));

        modelBuilder.Entity<Shift>()
            .ToTable(t => t.HasCheckConstraint("CK_Shift_Times", "[EndTime] > [StartTime]"));
        modelBuilder.Entity<Shift>()
            .ToTable(t => t.HasCheckConstraint("CK_Shift_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6"));
        
        
        // SEED
        modelBuilder.Entity<Store>().HasData(
            new Store { Id = 1, Name = "Bubble Heaven", Location = "123 Tea St, Warsaw, Poland" },
            new Store { Id = 2, Name = "The Brew Nook", Location = "456 Pearl Ave, Krakow, Poland" }
        );

        // Seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FullName = "Anna Kowalska",
                Email = "anna.k@bubbleheaven.pl",
                Phone = "123456789",
                Address = "789 Main St, Warsaw, Poland",
                Salary = 50000,
                HygieneScore = 9,
                RegisterProficiency = 10,
                TrainingSessionsConducted = 5
            },
            new Employee
            {
                Id = 2,
                FullName = "Piotr Nowak",
                Email = "piotr.n@bubbleheaven.pl",
                Phone = "987654321",
                Address = "101 Side Rd, Krakow, Poland",
                Salary = 65000,
                HygieneScore = 10,
                RegisterProficiency = 8,
                TrainingSessionsConducted = 10
            },
            new Employee
            {
                Id = 3,
                FullName = "Ewa Mazur",
                Email = "ewa.m@bubbleheaven.pl",
                Phone = "112233445",
                Address = "202 Center Ave, Warsaw, Poland",
                Salary = 45000,
                HygieneScore = 8,
                RegisterProficiency = 9,
                TrainingSessionsConducted = 3
            }
        );
        
        // Seed Employee Roles
        modelBuilder.Entity<EmployeeRoleMapping>().HasData(
            new EmployeeRoleMapping { EmployeeId = 1, Role = EmployeeRole.MANAGER },
            new EmployeeRoleMapping { EmployeeId = 1, Role = EmployeeRole.BARISTA },
            new EmployeeRoleMapping { EmployeeId = 2, Role = EmployeeRole.MANAGER },
            new EmployeeRoleMapping { EmployeeId = 2, Role = EmployeeRole.CASHIER },
            new EmployeeRoleMapping { EmployeeId = 3, Role = EmployeeRole.BARISTA },
            new EmployeeRoleMapping { EmployeeId = 3, Role = EmployeeRole.CASHIER }
        );

        // Seed MenuItemAllergens
        modelBuilder.Entity<MenuItemAllergen>().HasData(
            new MenuItemAllergen { Id = 1, Name = "Soy" },
            new MenuItemAllergen { Id = 2, Name = "Milk" }
        );
        
        // Seed Menu Items (polymorphic)
        modelBuilder.Entity<FruitTea>().HasData(
            new FruitTea
            {
                Id = 1,
                Name = "Strawberry Fruit Tea",
                BasePrice = 15.00,
                StockQuantity = 100,
                FruitBase = FruitBase.STRAWBERRY,
                TeaBase = TeaBase.BLACK,
                IceLevel = 2
            }
        );

        modelBuilder.Entity<MilkTea>().HasData(
            new MilkTea
            {
                Id = 2,
                Name = "Classic Pearl Milk Tea",
                BasePrice = 18.00,
                StockQuantity = 150,
                TeaBase = TeaBase.BLACK,
                MilkBase = MilkBase.COW
            }
        );
        
        modelBuilder.Entity<Frappe>().HasData(
            new Frappe
            {
                Id = 3,
                Name = "Choco Chip Frappe",
                BasePrice = 22.50,
                StockQuantity = 80,
                BaseFlavor = BaseFlavor.CHOCOLATE,
                HasWhippedCream = true
            }
        );
        
        // Seed the many-to-many join table for MenuItem <-> MenuItemAllergen
        modelBuilder.Entity<MenuItem>()
            .HasMany(mi => mi.MenuItemAllergens)
            .WithMany(a => a.MenuItems)
            .UsingEntity(j => j.ToTable("MenuItem_MenuItemAllergens").HasData(
                new { MenuItemsId = 2, MenuItemAllergensId = 2 }, // Classic Pearl Milk Tea (Id=2) has Milk allergen (Id=2)
                new { MenuItemsId = 3, MenuItemAllergensId = 2 }  // Choco Chip Frappe (Id=3) has Milk allergen (Id=2)
            ));

        // Seed the many-to-many join table for Store <-> MenuItem
        modelBuilder.Entity<Store>()
            .HasMany(s => s.MenuItems)
            .WithMany(mi => mi.Stores)
            .UsingEntity(j => j.ToTable("MenuItemStores").HasData(
                new { StoresId = 1, MenuItemsId = 1 }, // Store 1 has MenuItem 1
                new { StoresId = 1, MenuItemsId = 2 }, // Store 1 has MenuItem 2
                new { StoresId = 2, MenuItemsId = 2 }, // Store 2 has MenuItem 2
                new { StoresId = 2, MenuItemsId = 3 }  // Store 2 has MenuItem 3
            ));
        
        // Seed AssignmentHistory
        modelBuilder.Entity<AssignmentHistory>().HasData(
            new AssignmentHistory { Id = -1, StoreId = 1, EmployeeId = 1, StartDate = DateTime.Parse("2024-06-01T00:00:00Z"), EndDate = DateTime.Parse("2024-08-01T00:00:00Z") }
        );

        // Seed Shifts
        modelBuilder.Entity<Shift>().HasData(
            new Shift
            {
                Id = 1,
                DayOfWeek = DayOfWeek.MONDAY,
                StartTime = new DateTime(2024, 7, 29, 9, 0, 0),
                EndTime = new DateTime(2024, 7, 29, 17, 0, 0),
                ManagingEmployeeId = 1
            },
            new Shift
            {
                Id = 2,
                DayOfWeek = DayOfWeek.TUESDAY,
                StartTime = new DateTime(2024, 7, 30, 10, 0, 0),
                EndTime = new DateTime(2024, 7, 30, 18, 0, 0),
                ManagingEmployeeId = 2
            }
        );
        
        // Seed the many-to-many join table for Employee <-> Shift
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.WorksInShifts)
            .WithMany(s => s.Employees)
            .UsingEntity(j => j.ToTable("EmployeeWorksInShifts").HasData(
                new { WorksInShiftsId = 1, EmployeesId = 1 },
                new { WorksInShiftsId = 1, EmployeesId = 3 },
                new { WorksInShiftsId = 2, EmployeesId = 2 }
            ));
        
        // Seed Orders
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                Status = OrderStatus.COMPLETED,
                OrderDateTime = DateTime.Parse("2024-07-27T10:30:00Z")
            }
        );

        // Seed OrderLines
        modelBuilder.Entity<OrderLine>().HasData(
            new OrderLine { Id = 1, OrderId = 1, MenuItemId = 1, Quantity = 2, Size = Size.L },
            new OrderLine { Id = 2, OrderId = 1, MenuItemId = 2, Quantity = 1, Size = Size.M }
        );
        
        // Seed OrderLineToppingMapping
        modelBuilder.Entity<OrderLineToppingMapping>().HasData(
            new OrderLineToppingMapping { OrderLineId = 1, Topping = Topping.TAPIOCA },
            new OrderLineToppingMapping { OrderLineId = 1, Topping = Topping.KONJAC }
        );

    }
}
