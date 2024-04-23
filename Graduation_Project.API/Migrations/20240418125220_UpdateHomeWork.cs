using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHomeWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Homeworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_TeacherId",
                table: "Homeworks",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Teachers_TeacherId",
                table: "Homeworks",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Teachers_TeacherId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_TeacherId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Homeworks");
        }
    }
}
