using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorFinderHubApi.Migrations
{
    public partial class ThirdInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookingAppointments_doctors_DoctorId",
                table: "bookingAppointments");

            migrationBuilder.DropForeignKey(
                name: "FK_bookingAppointments_patients_PatientId",
                table: "bookingAppointments");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "bookingAppointments",
                newName: "PatientAuthId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "bookingAppointments",
                newName: "DoctorAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_bookingAppointments_PatientId",
                table: "bookingAppointments",
                newName: "IX_bookingAppointments_PatientAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_bookingAppointments_DoctorId",
                table: "bookingAppointments",
                newName: "IX_bookingAppointments_DoctorAuthId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookingAppointments_doctorAuths_DoctorAuthId",
                table: "bookingAppointments",
                column: "DoctorAuthId",
                principalTable: "doctorAuths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookingAppointments_patientAuths_PatientAuthId",
                table: "bookingAppointments",
                column: "PatientAuthId",
                principalTable: "patientAuths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookingAppointments_doctorAuths_DoctorAuthId",
                table: "bookingAppointments");

            migrationBuilder.DropForeignKey(
                name: "FK_bookingAppointments_patientAuths_PatientAuthId",
                table: "bookingAppointments");

            migrationBuilder.RenameColumn(
                name: "PatientAuthId",
                table: "bookingAppointments",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "DoctorAuthId",
                table: "bookingAppointments",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_bookingAppointments_PatientAuthId",
                table: "bookingAppointments",
                newName: "IX_bookingAppointments_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_bookingAppointments_DoctorAuthId",
                table: "bookingAppointments",
                newName: "IX_bookingAppointments_DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookingAppointments_doctors_DoctorId",
                table: "bookingAppointments",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookingAppointments_patients_PatientId",
                table: "bookingAppointments",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
