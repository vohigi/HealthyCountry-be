using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class icpc2_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "4e30d84c-4800-4d42-8668-85935735fa00");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "592ec6c7-7bf0-4cb7-86e4-61b764d8f4ec", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$KZTDX.7OZMLCOudE55hODuNGp2un7A/M76A7WSE/EBPyhZkmp3apO", "380505680632", "ADMIN", "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_NumberOnlyCode",
                table: "ICPC2Codes",
                column: "NumberOnlyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ICPC2Codes_NumberOnlyCode",
                table: "ICPC2Codes");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "592ec6c7-7bf0-4cb7-86e4-61b764d8f4ec");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "4e30d84c-4800-4d42-8668-85935735fa00", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$R5dLOb5lp0d268yr4/3hTO7Dn3xw8uIGcHpN5WRd5hci0NXkrG5L.", "380505680632", "ADMIN", "11111111" });
        }
    }
}
