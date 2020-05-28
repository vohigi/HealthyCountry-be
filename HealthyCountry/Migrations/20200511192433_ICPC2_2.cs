using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class ICPC2_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Users_EmployeeId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Users_PatientId",
                table: "appointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "b3215d32-4942-496c-aa14-e4dc8c5f4ece");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "17859519-967d-4a3a-a6bf-3f34b04f75a4", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$g0Y2DeIDAy2M/r2hLvomDO7tXzDfn0.krobMH/DRZYoQjXvkj34oW", "380505680632", "ADMIN", "11111111" });

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Users_EmployeeId",
                table: "appointments",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Users_PatientId",
                table: "appointments",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Users_EmployeeId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Users_PatientId",
                table: "appointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "17859519-967d-4a3a-a6bf-3f34b04f75a4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "b3215d32-4942-496c-aa14-e4dc8c5f4ece", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$LIuCmf2TobWW2i4LJcye0O9OoWC.ePmypoYkPUMFkjknWwJ1XwAS6", "380505680632", "ADMIN", "11111111" });

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Users_EmployeeId",
                table: "appointments",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Users_PatientId",
                table: "appointments",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
