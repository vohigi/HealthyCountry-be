using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class isActive_Orgs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "d1b883f7-311a-4c18-983d-d22be046d9f7");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "a37030c9-2b2e-4666-8033-9ec437731524", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$eo6CbLO1ef3QRA.p0EC9qeRoYrfxtcT22U4328jvlfsYAedtKNhp2", "380505680632", "ADMIN", "11111111" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "a37030c9-2b2e-4666-8033-9ec437731524");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Organizations");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "d1b883f7-311a-4c18-983d-d22be046d9f7", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", "Adminovski", "Adminovich", "org_1", "$2b$10$cJO36jbyw4/9vOitC1fitOPxKaIRIAzRbxdES/GE23Xk3hwfbmv5G", "380505680632", "ADMIN", "11111111" });
        }
    }
}
