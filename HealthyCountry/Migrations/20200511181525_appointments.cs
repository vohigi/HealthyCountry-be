using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class appointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "a37030c9-2b2e-4666-8033-9ec437731524");

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<string>(type: "varchar(250)", nullable: false),
                    PatientId = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(250)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(24)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_appointments_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "a5d1bf49-a58b-46d1-a335-3982c9cb95e2", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$h7VILUXPCflmuxbvaEnq4Oa/BmPGk0tqEPjBCNXXjkKku6wwvirie", "380505680632", "ADMIN", "11111111" });

            migrationBuilder.CreateIndex(
                name: "EmployeeId",
                table: "appointments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "appointments",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "a5d1bf49-a58b-46d1-a335-3982c9cb95e2");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "a37030c9-2b2e-4666-8033-9ec437731524", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$eo6CbLO1ef3QRA.p0EC9qeRoYrfxtcT22U4328jvlfsYAedtKNhp2", "380505680632", "ADMIN", "11111111" });
        }
    }
}
