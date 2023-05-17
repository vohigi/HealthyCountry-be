using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace HealthyCountry.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ICPC2Codes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TableKey = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Inclusions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Exclusions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Criteria = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Considerations = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NumberOnlyCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Edrpou = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiagnosisEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: true),
                    ClinicalStatus = table.Column<int>(type: "integer", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CodeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnosisEntity_ICPC2Codes_CodeId",
                        column: x => x.CodeId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ICPC2GroupEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TableKey = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ICPC2Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<byte>(type: "smallint", nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    TaxId = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Specialization = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DiagnosisId = table.Column<Guid>(type: "uuid", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_DiagnosisEntity_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "DiagnosisEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentsToActionLinks",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentsToActionLinks", x => new { x.AppointmentId, x.CodingId });
                    table.UniqueConstraint("AK_AppointmentsToActionLinks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentsToActionLinks_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
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
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentsToReasonLinks", x => new { x.AppointmentId, x.CodingId });
                    table.UniqueConstraint("AK_AppointmentsToReasonLinks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentsToReasonLinks_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentsToReasonLinks_ICPC2Codes_CodingId",
                        column: x => x.CodingId,
                        principalTable: "ICPC2Codes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Address", "Edrpou", "IsActive", "Name" },
                values: new object[] { new Guid("650d70c4-5136-4c13-9a60-aa3aebae8ea5"), "London 221B Baker Street", "11111111", false, "Default Organization" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "Specialization", "TaxId" },
                values: new object[] { new Guid("45497320-bf4f-4850-bb44-fa55b68d1618"), new DateTime(2000, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", new Guid("650d70c4-5136-4c13-9a60-aa3aebae8ea5"), "$2b$10$TRtiQ9deL1pVHmKQ5lmU0ejp3R1yw/BiyVehjnqu/PfzfW5QMF2xe", "380505680632", 3, 0, "11111111" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DiagnosisId",
                table: "Appointments",
                column: "DiagnosisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_Code_Name_IsActual_InsertDate",
                table: "ICPC2Codes",
                columns: new[] { "Code", "Name", "IsActual", "InsertDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_Name",
                table: "ICPC2Codes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_NumberOnlyCode",
                table: "ICPC2Codes",
                column: "NumberOnlyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2Codes_SearchVector",
                table: "ICPC2Codes",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2GroupEntity_GroupId_ICPC2Id_IsActual_InsertDate",
                table: "ICPC2GroupEntity",
                columns: new[] { "GroupId", "ICPC2Id", "IsActual", "InsertDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ICPC2GroupEntity_ICPC2Id",
                table: "ICPC2GroupEntity",
                column: "ICPC2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentsToActionLinks");

            migrationBuilder.DropTable(
                name: "AppointmentsToReasonLinks");

            migrationBuilder.DropTable(
                name: "ICPC2GroupEntity");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "DiagnosisEntity");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ICPC2Codes");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
