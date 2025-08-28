using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BubbleTeaShop.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentHistoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentHistories",
                table: "AssignmentHistories");

            migrationBuilder.DeleteData(
                table: "AssignmentHistories",
                keyColumns: new[] { "EmployeeId", "StartDate", "StoreId" },
                keyValues: new object[] { 1, new DateTime(2024, 6, 1, 2, 0, 0, 0, DateTimeKind.Local), 1 });

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AssignmentHistories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentHistories",
                table: "AssignmentHistories",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AssignmentHistories",
                columns: new[] { "Id", "EmployeeId", "EndDate", "StartDate", "StoreId" },
                values: new object[] { -1, 1, new DateTime(2024, 8, 1, 2, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 6, 1, 2, 0, 0, 0, DateTimeKind.Local), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHistories_StoreId",
                table: "AssignmentHistories",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentHistories",
                table: "AssignmentHistories");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentHistories_StoreId",
                table: "AssignmentHistories");

            migrationBuilder.DeleteData(
                table: "AssignmentHistories",
                keyColumn: "Id",
                keyColumnType: "INTEGER",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AssignmentHistories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentHistories",
                table: "AssignmentHistories",
                columns: new[] { "StoreId", "EmployeeId", "StartDate" });

            migrationBuilder.InsertData(
                table: "AssignmentHistories",
                columns: new[] { "EmployeeId", "StartDate", "StoreId", "EndDate" },
                values: new object[] { 1, new DateTime(2024, 6, 1, 2, 0, 0, 0, DateTimeKind.Local), 1, new DateTime(2024, 8, 1, 2, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
