using BubbleTesShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Helpers;

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
            .HasKey(ah => new { ah.StoreId, ah.EmployeeId, ah.StartDate });

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
    }
}
