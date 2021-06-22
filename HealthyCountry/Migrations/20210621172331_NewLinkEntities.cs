using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class NewLinkEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_ICPC2Codes_ActionId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_ICPC2Codes_DiagnosisId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_ICPC2Codes_ReasonId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_ActionId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_DiagnosisId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_ReasonId",
                table: "appointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "d8c036e8-682d-4c46-ac14-bace496def6d");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "appointments");

            migrationBuilder.AlterColumn<string>(
                name: "DiagnosisId",
                table: "appointments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AppointmentsToActionLinks",
                columns: table => new
                {
                    AppointmentId = table.Column<string>(nullable: false),
                    CodingId = table.Column<string>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentsToActionLinks", x => new { x.AppointmentId, x.CodingId });
                    table.UniqueConstraint("AK_AppointmentsToActionLinks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentsToActionLinks_appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentsToActionLinks_ICPC2Codes_CodingId",
                        column: x => x.CodingId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentsToReasonLinks",
                columns: table => new
                {
                    AppointmentId = table.Column<string>(nullable: false),
                    CodingId = table.Column<string>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentsToReasonLinks", x => new { x.AppointmentId, x.CodingId });
                    table.UniqueConstraint("AK_AppointmentsToReasonLinks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentsToReasonLinks_appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointments",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentsToReasonLinks_ICPC2Codes_CodingId",
                        column: x => x.CodingId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosisEntity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Severity = table.Column<int>(nullable: true),
                    ClinicalStatus = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    CodeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosisEntity_ICPC2Codes_CodeId",
                        column: x => x.CodeId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "Specialization", "TaxId" },
                values: new object[] { "d890572c-cb54-4e3f-8fdd-8f22f81812b6", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$L7DMm2m2W.LpY/rU5aeqzuwARj5dZb7qJYzsTum4zim9oaHjLx0ye", "380505680632", "ADMIN", 0, "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DiagnosisId",
                table: "appointments",
                column: "DiagnosisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentsToActionLinks_CodingId",
                table: "AppointmentsToActionLinks",
                column: "CodingId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentsToReasonLinks_CodingId",
                table: "AppointmentsToReasonLinks",
                column: "CodingId");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosisEntity_CodeId",
                table: "DiagnosisEntity",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_DiagnosisEntity_DiagnosisId",
                table: "appointments",
                column: "DiagnosisId",
                principalTable: "DiagnosisEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_DiagnosisEntity_DiagnosisId",
                table: "appointments");

            migrationBuilder.DropTable(
                name: "AppointmentsToActionLinks");

            migrationBuilder.DropTable(
                name: "AppointmentsToReasonLinks");

            migrationBuilder.DropTable(
                name: "DiagnosisEntity");

            migrationBuilder.DropIndex(
                name: "IX_appointments_DiagnosisId",
                table: "appointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "d890572c-cb54-4e3f-8fdd-8f22f81812b6");

            migrationBuilder.AlterColumn<string>(
                name: "DiagnosisId",
                table: "appointments",
                type: "varchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                table: "appointments",
                type: "varchar(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonId",
                table: "appointments",
                type: "varchar(36)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "Specialization", "TaxId" },
                values: new object[] { "d8c036e8-682d-4c46-ac14-bace496def6d", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$R8tQazq9W.m6Ztx3ExMQH.6NxQp5nMWglunSbhsw8Uw.JIpsJq60m", "380505680632", "ADMIN", 0, "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ActionId",
                table: "appointments",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DiagnosisId",
                table: "appointments",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ReasonId",
                table: "appointments",
                column: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_ICPC2Codes_ActionId",
                table: "appointments",
                column: "ActionId",
                principalTable: "ICPC2Codes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_ICPC2Codes_DiagnosisId",
                table: "appointments",
                column: "DiagnosisId",
                principalTable: "ICPC2Codes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_ICPC2Codes_ReasonId",
                table: "appointments",
                column: "ReasonId",
                principalTable: "ICPC2Codes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
