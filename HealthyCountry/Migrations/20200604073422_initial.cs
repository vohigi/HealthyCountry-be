using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ICPC2Codes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    TableKey = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 25, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    ShortName = table.Column<string>(maxLength: 255, nullable: true),
                    Inclusions = table.Column<string>(maxLength: 2000, nullable: true),
                    Exclusions = table.Column<string>(maxLength: 2000, nullable: true),
                    Criteria = table.Column<string>(maxLength: 2000, nullable: true),
                    Considerations = table.Column<string>(maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(maxLength: 2000, nullable: true),
                    IsActual = table.Column<bool>(nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime(0)", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime(0)", nullable: true),
                    NumberOnlyCode = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICPC2Codes", x => x.Id);
                    table.UniqueConstraint("AK_ICPC2Codes_TableKey", x => x.TableKey);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<string>(type: "varchar(50)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: true),
                    Edrpou = table.Column<string>(type: "varchar(10)", nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "ICPC2GroupEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    TableKey = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ICPC2Id = table.Column<string>(type: "varchar(36)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(30)", nullable: true),
                    MiddleName = table.Column<string>(type: "varchar(30)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(30)", nullable: true),
                    TaxId = table.Column<string>(type: "varchar(10)", nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Phone = table.Column<string>(type: "varchar(12)", nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", nullable: true),
                    Password = table.Column<string>(type: "varchar(255)", nullable: true),
                    Role = table.Column<string>(type: "varchar(30)", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<string>(type: "varchar(250)", nullable: false),
                    PatientId = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(250)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(24)", nullable: false),
                    ReasonId = table.Column<string>(nullable: true),
                    ActionId = table.Column<string>(nullable: true),
                    DiagnosisId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_appointments_ICPC2Codes_ActionId",
                        column: x => x.ActionId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_appointments_ICPC2Codes_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_appointments_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_appointments_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_appointments_ICPC2Codes_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationId", "Address", "Edrpou", "IsActive", "Name" },
                values: new object[] { "org_1", "London 221B Baker Street", "11111111", false, "Default Organization" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "bb9e56b6-2fe4-4e1d-b27e-229cc1a6ce77", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$AzUz5k0q5TUhAPh.tymyAO7iaiXD8y77EM0m9Q1UNpyH1B9wNYFqS", "380505680632", "ADMIN", "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ActionId",
                table: "appointments",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DiagnosisId",
                table: "appointments",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "EmployeeId",
                table: "appointments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UserId",
                table: "appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ReasonId",
                table: "appointments",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_Name",
                table: "ICPC2Codes",
                column: "Name")
                .Annotation("MySql:FullTextIndex", true);

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_NumberOnlyCode",
                table: "ICPC2Codes",
                column: "NumberOnlyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_Code_Name_IsActual_InsertDate",
                table: "ICPC2Codes",
                columns: new[] { "Code", "Name", "IsActual", "InsertDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2GroupEntity_ICPC2Id",
                table: "ICPC2GroupEntity",
                column: "ICPC2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2GroupEntity_GroupId_ICPC2Id_IsActual_InsertDate",
                table: "ICPC2GroupEntity",
                columns: new[] { "GroupId", "ICPC2Id", "IsActual", "InsertDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "ICPC2GroupEntity");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ICPC2Codes");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
