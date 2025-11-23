using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "[Phone] LIKE '01%' AND [Phone] NOT LIKE '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "[Phone] LIKE '01%' AND [Phone] NOT LIKE '%[^0-9]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "[Phone] like '01' and Phone Not Like '%[^0-9]%' ");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "[Phone] like '01' and Phone Not Like '%[^0-9]%' ");
        }
    }
}
