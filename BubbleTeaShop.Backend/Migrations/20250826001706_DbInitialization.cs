using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BubbleTesShop.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DbInitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Salary = table.Column<float>(type: "REAL", nullable: false),
                    HygieneScore = table.Column<int>(type: "INTEGER", nullable: true),
                    RegisterProficiency = table.Column<int>(type: "INTEGER", nullable: true),
                    TrainingSessionsConducted = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.CheckConstraint("CK_Employee_Email", "[Email] LIKE '_%@_%.__%'");
                    table.CheckConstraint("CK_Employee_HygieneScore", "[HygieneScore] IS NULL OR ([HygieneScore] >= 0 AND [HygieneScore] <= 10)");
                    table.CheckConstraint("CK_Employee_Phone", "LENGTH([Phone]) = 9");
                    table.CheckConstraint("CK_Employee_RegisterProficiency", "[RegisterProficiency] IS NULL OR ([RegisterProficiency] >= 0 AND [RegisterProficiency] <= 10)");
                    table.CheckConstraint("CK_Employee_Salary", "[Salary] > 0");
                    table.CheckConstraint("CK_Employee_TrainingSessions", "[TrainingSessionsConducted] IS NULL OR [TrainingSessionsConducted] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "MenuItemAllergens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemAllergens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BasePrice = table.Column<double>(type: "REAL", nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.CheckConstraint("CK_MenuItem_BasePrice", "[BasePrice] > 0");
                    table.CheckConstraint("CK_MenuItem_Name", "[Name] <> ''");
                    table.CheckConstraint("CK_MenuItem_StockQuantity", "[StockQuantity] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CK_Order_OrderDateTime", "[OrderDateTime] > '0001-01-01'");
                    table.CheckConstraint("CK_Order_Status", "[Status] BETWEEN 0 AND 5");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.CheckConstraint("CK_Store_Location", "LENGTH([Location]) > 0");
                    table.CheckConstraint("CK_Store_Name", "LENGTH([Name]) > 0");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CertificateName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificates_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoleMappings",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeRole = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleMappings", x => new { x.EmployeeId, x.EmployeeRole });
                    table.CheckConstraint("CK_EmployeeRoleMapping_EmployeeRole", "[EmployeeRole] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_EmployeeRoleMappings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    ManagingEmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.CheckConstraint("CK_Shift_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6");
                    table.CheckConstraint("CK_Shift_Times", "[EndTime] > [StartTime]");
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_ManagingEmployeeId",
                        column: x => x.ManagingEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Frappes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaseFlavor = table.Column<int>(type: "INTEGER", nullable: false),
                    HasWhippedCream = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frappes", x => x.Id);
                    table.CheckConstraint("CK_Frappe_BaseFlavor", "[BaseFlavor] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_Frappes_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FruitTeas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FruitBase = table.Column<int>(type: "INTEGER", nullable: false),
                    TeaBase = table.Column<int>(type: "INTEGER", nullable: false),
                    IceLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitTeas", x => x.Id);
                    table.CheckConstraint("CK_FruitTea_FruitBase", "[FruitBase] BETWEEN 0 AND 3");
                    table.CheckConstraint("CK_FruitTea_IceLevel", "[IceLevel] BETWEEN 0 AND 3");
                    table.CheckConstraint("CK_FruitTea_TeaBase", "[TeaBase] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_FruitTeas_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem_MenuItemAllergens",
                columns: table => new
                {
                    MenuItemAllergensId = table.Column<int>(type: "INTEGER", nullable: false),
                    MenuItemsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem_MenuItemAllergens", x => new { x.MenuItemAllergensId, x.MenuItemsId });
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuItemAllergens_MenuItemAllergens_MenuItemAllergensId",
                        column: x => x.MenuItemAllergensId,
                        principalTable: "MenuItemAllergens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuItemAllergens_MenuItems_MenuItemsId",
                        column: x => x.MenuItemsId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilkTeas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeaBase = table.Column<int>(type: "INTEGER", nullable: false),
                    MilkBase = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilkTeas", x => x.Id);
                    table.CheckConstraint("CK_MilkTea_MilkBase", "[MilkBase] BETWEEN 0 AND 1");
                    table.CheckConstraint("CK_MilkTea_TeaBase", "[TeaBase] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_MilkTeas_MenuItems_Id",
                        column: x => x.Id,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    MenuItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                    table.CheckConstraint("CK_OrderLine_Quantity", "[Quantity] >= 1");
                    table.CheckConstraint("CK_OrderLine_Size", "[Size] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_OrderLines_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentHistories",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentHistories", x => new { x.StoreId, x.EmployeeId, x.StartDate });
                    table.CheckConstraint("CK_AssignmentHistory_EndDate", "[EndDate] > [StartDate]");
                    table.ForeignKey(
                        name: "FK_AssignmentHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentHistories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemStores",
                columns: table => new
                {
                    MenuItemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    StoresId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemStores", x => new { x.MenuItemsId, x.StoresId });
                    table.ForeignKey(
                        name: "FK_MenuItemStores_MenuItems_MenuItemsId",
                        column: x => x.MenuItemsId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemStores_Stores_StoresId",
                        column: x => x.StoresId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorksInShifts",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorksInShiftsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorksInShifts", x => new { x.EmployeesId, x.WorksInShiftsId });
                    table.ForeignKey(
                        name: "FK_EmployeeWorksInShifts_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorksInShifts_Shifts_WorksInShiftsId",
                        column: x => x.WorksInShiftsId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineToppings",
                columns: table => new
                {
                    OrderLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Topping = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineToppings", x => new { x.OrderLineId, x.Topping });
                    table.CheckConstraint("CK_OrderLineToppingMapping_Topping", "[Topping] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_OrderLineToppings_OrderLines_OrderLineId",
                        column: x => x.OrderLineId,
                        principalTable: "OrderLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistories_EmployeeId",
                table: "AssignmentHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_EmployeeId",
                table: "EmployeeCertificates",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorksInShifts_WorksInShiftsId",
                table: "EmployeeWorksInShifts",
                column: "WorksInShiftsId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuItemAllergens_MenuItemsId",
                table: "MenuItem_MenuItemAllergens",
                column: "MenuItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemStores_StoresId",
                table: "MenuItemStores",
                column: "StoresId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_MenuItemId",
                table: "OrderLines",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ManagingEmployeeId",
                table: "Shifts",
                column: "ManagingEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentHistories");

            migrationBuilder.DropTable(
                name: "EmployeeCertificates");

            migrationBuilder.DropTable(
                name: "EmployeeRoleMappings");

            migrationBuilder.DropTable(
                name: "EmployeeWorksInShifts");

            migrationBuilder.DropTable(
                name: "Frappes");

            migrationBuilder.DropTable(
                name: "FruitTeas");

            migrationBuilder.DropTable(
                name: "MenuItem_MenuItemAllergens");

            migrationBuilder.DropTable(
                name: "MenuItemStores");

            migrationBuilder.DropTable(
                name: "MilkTeas");

            migrationBuilder.DropTable(
                name: "OrderLineToppings");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "MenuItemAllergens");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
