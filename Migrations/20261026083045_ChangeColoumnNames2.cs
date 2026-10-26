using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColoumnNames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Employees",
                newName: "EmployeeStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeStatus",
                table: "Employees",
                newName: "Status");
        }
    }
}
