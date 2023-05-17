using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthyCountry.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DiagnosisEntity_DiagnosisId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_DiagnosisEntity_ICPC2Codes_CodeId",
                table: "DiagnosisEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiagnosisEntity",
                table: "DiagnosisEntity");

            migrationBuilder.RenameTable(
                name: "DiagnosisEntity",
                newName: "Diagnoses");

            migrationBuilder.RenameIndex(
                name: "IX_DiagnosisEntity_CodeId",
                table: "Diagnoses",
                newName: "IX_Diagnoses_CodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diagnoses",
                table: "Diagnoses",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("45497320-bf4f-4850-bb44-fa55b68d1618"),
                column: "Password",
                value: "$2b$10$hTl7cXE2GTVkL3.ppn9hSOgivwKCueNoLz93pFEzqLm9yheC4OfgS");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Diagnoses_DiagnosisId",
                table: "Appointments",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_ICPC2Codes_CodeId",
                table: "Diagnoses",
                column: "CodeId",
                principalTable: "ICPC2Codes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Diagnoses_DiagnosisId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_ICPC2Codes_CodeId",
                table: "Diagnoses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diagnoses",
                table: "Diagnoses");

            migrationBuilder.RenameTable(
                name: "Diagnoses",
                newName: "DiagnosisEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Diagnoses_CodeId",
                table: "DiagnosisEntity",
                newName: "IX_DiagnosisEntity_CodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiagnosisEntity",
                table: "DiagnosisEntity",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("45497320-bf4f-4850-bb44-fa55b68d1618"),
                column: "Password",
                value: "$2b$10$TRtiQ9deL1pVHmKQ5lmU0ejp3R1yw/BiyVehjnqu/PfzfW5QMF2xe");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DiagnosisEntity_DiagnosisId",
                table: "Appointments",
                column: "DiagnosisId",
                principalTable: "DiagnosisEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagnosisEntity_ICPC2Codes_CodeId",
                table: "DiagnosisEntity",
                column: "CodeId",
                principalTable: "ICPC2Codes",
                principalColumn: "Id");
        }
    }
}
