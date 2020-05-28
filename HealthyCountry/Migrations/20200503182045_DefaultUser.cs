using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class DefaultUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "LastName", "MiddleName", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", "Adminovski", "Adminovich", "$2b$10$3J46c85C2oBZ.mXcd/Jy0e43ZMnKzP4p8LpEayVX8q34g41Aumzd2", "380505680632", "ADMIN", "11111111" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "00000000-0000-0000-0000-000000000000");
        }
    }
}
