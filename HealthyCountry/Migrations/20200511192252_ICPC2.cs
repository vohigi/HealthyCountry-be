using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyCountry.Migrations
{
    public partial class ICPC2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "a5d1bf49-a58b-46d1-a335-3982c9cb95e2");

            migrationBuilder.CreateTable(
                name: "ICPC2Codes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "b3215d32-4942-496c-aa14-e4dc8c5f4ece", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$LIuCmf2TobWW2i4LJcye0O9OoWC.ePmypoYkPUMFkjknWwJ1XwAS6", "380505680632", "ADMIN", "11111111" });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ICPC2Codes");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "b3215d32-4942-496c-aa14-e4dc8c5f4ece");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "BirthDate", "Email", "FirstName", "Gender", "IsActive", "LastName", "MiddleName", "OrganizationId", "Password", "Phone", "Role", "TaxId" },
                values: new object[] { "a5d1bf49-a58b-46d1-a335-3982c9cb95e2", new DateTime(1000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "MALE", false, "Adminovski", "Adminovich", "org_1", "$2b$10$h7VILUXPCflmuxbvaEnq4Oa/BmPGk0tqEPjBCNXXjkKku6wwvirie", "380505680632", "ADMIN", "11111111" });
        }
    }
}
