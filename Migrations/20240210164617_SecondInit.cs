using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorFinderHubApi.Migrations
{
    public partial class SecondInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "PatientEmail",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "PatientPhoneNumber",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "DoctorStatus",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "doctorSpecialist",
                table: "doctors");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientAuthId",
                table: "patients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorAuthId",
                table: "doctors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "adminAuths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adminAuths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "doctorAuths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doctorSpecialist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorAuths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "patientAuths",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientAuths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminAuthId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_admins_adminAuths_AdminAuthId",
                        column: x => x.AdminAuthId,
                        principalTable: "adminAuths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patients_PatientAuthId",
                table: "patients",
                column: "PatientAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_DoctorAuthId",
                table: "doctors",
                column: "DoctorAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_admins_AdminAuthId",
                table: "admins",
                column: "AdminAuthId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_doctorAuths_DoctorAuthId",
                table: "doctors",
                column: "DoctorAuthId",
                principalTable: "doctorAuths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_patientAuths_PatientAuthId",
                table: "patients",
                column: "PatientAuthId",
                principalTable: "patientAuths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_doctorAuths_DoctorAuthId",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_patientAuths_PatientAuthId",
                table: "patients");

            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "doctorAuths");

            migrationBuilder.DropTable(
                name: "patientAuths");

            migrationBuilder.DropTable(
                name: "adminAuths");

            migrationBuilder.DropIndex(
                name: "IX_patients_PatientAuthId",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_doctors_DoctorAuthId",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "PatientAuthId",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "DoctorAuthId",
                table: "doctors");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "patients",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "patients",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "PatientEmail",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientPhoneNumber",
                table: "patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorStatus",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "doctors",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "doctors",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "doctors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "doctors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "doctorSpecialist",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
