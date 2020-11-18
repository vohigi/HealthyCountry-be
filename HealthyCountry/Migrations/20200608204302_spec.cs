using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class spec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "bb9e56b6-2fe4-4e1d-b27e-229cc1a6ce77");

            migrationBuilder.AddColumn<int>(
                name: "Specialization",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "Specialization", "TaxId" },
                values: new object[] { "d8c036e8-682d-4c46-ac14-bace496def6d", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$R8tQazq9W.m6Ztx3ExMQH.6NxQp5nMWglunSbhsw8Uw.JIpsJq60m", "380505680632", "ADMIN", 0, "11111111" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "d8c036e8-682d-4c46-ac14-bace496def6d");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "bb9e56b6-2fe4-4e1d-b27e-229cc1a6ce77", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$AzUz5k0q5TUhAPh.tymyAO7iaiXD8y77EM0m9Q1UNpyH1B9wNYFqS", "380505680632", "ADMIN", "11111111" });
        }
    }
}
