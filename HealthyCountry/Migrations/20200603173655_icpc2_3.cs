using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class icpc2_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ICPC2Codes_NumberOnlyCode",
                table: "ICPC2Codes");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "17859519-967d-4a3a-a6bf-3f34b04f75a4");

            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                table: "appointments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosisId",
                table: "appointments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonId",
                table: "appointments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ICPC2GroupEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    TableKey = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ICPC2Id = table.Column<string>(type: "char(36)", nullable: false),
                    GroupId = table.Column<byte>(nullable: false),
                    IsActual = table.Column<bool>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime(0)", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICPC2GroupEntity", x => x.Id);
                    table.UniqueConstraint("AK_ICPC2GroupEntity_TableKey", x => x.TableKey);
                    table.ForeignKey(
                        name: "FK_ICPC2GroupEntity_ICPC2Codes_ICPC2Id",
                        column: x => x.ICPC2Id,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "4e30d84c-4800-4d42-8668-85935735fa00", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$R5dLOb5lp0d268yr4/3hTO7Dn3xw8uIGcHpN5WRd5hci0NXkrG5L.", "380505680632", "ADMIN", "11111111" });

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

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2GroupEntity_ICPC2Id",
                table: "ICPC2GroupEntity",
                column: "ICPC2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2GroupEntity_GroupId_ICPC2Id_IsActual_InsertDate",
                table: "ICPC2GroupEntity",
                columns: new[] { "GroupId", "ICPC2Id", "IsActual", "InsertDate" },
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "ICPC2GroupEntity");

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
                keyValue: "4e30d84c-4800-4d42-8668-85935735fa00");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "appointments");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "17859519-967d-4a3a-a6bf-3f34b04f75a4", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$g0Y2DeIDAy2M/r2hLvomDO7tXzDfn0.krobMH/DRZYoQjXvkj34oW", "380505680632", "ADMIN", "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_NumberOnlyCode",
                table: "ICPC2Codes",
                column: "NumberOnlyCode");
        }
    }
}
