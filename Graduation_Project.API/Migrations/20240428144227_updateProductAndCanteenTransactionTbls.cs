using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.API.Migrations
{
    /// <inheritdoc />
    public partial class updateProductAndCanteenTransactionTbls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AlterColumn<decimal>(
                name: "TransactionAmount",
                table: "CanteenTransactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

           

            migrationBuilder.CreateTable(
                name: "CanteenTransactionProducts",
                columns: table => new
                {
                    CanteenTransactionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenTransactionProducts", x => new { x.CanteenTransactionId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CanteenTransactionProducts_CanteenTransactions_CanteenTransactionId",
                        column: x => x.CanteenTransactionId,
                        principalTable: "CanteenTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanteenTransactionProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

         
            migrationBuilder.CreateIndex(
                name: "IX_CanteenTransactionProducts_ProductId",
                table: "CanteenTransactionProducts",
                column: "ProductId");

      

          



   
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherGrades_Grades_GradeId",
                table: "TeacherGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherGrades_Teachers_TeacherId",
                table: "TeacherGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Subjects_SubjectId",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeacherId",
                table: "TeacherSubjects");

            migrationBuilder.DropTable(
                name: "CanteenTransactionProducts");

            migrationBuilder.DropTable(
                name: "TeacherClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSubjects",
                table: "TeacherSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherGrades",
                table: "TeacherGrades");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "TeacherSubjects",
                newName: "TeacherSubject");

            migrationBuilder.RenameTable(
                name: "TeacherGrades",
                newName: "TeacherGrade");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_SubjectId",
                table: "TeacherSubject",
                newName: "IX_TeacherSubject_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherGrades_GradeId",
                table: "TeacherGrade",
                newName: "IX_TeacherGrade_GradeId");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionAmount",
                table: "CanteenTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSubject",
                table: "TeacherSubject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherGrade",
                table: "TeacherGrade",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubject_TeacherId",
                table: "TeacherSubject",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGrade_TeacherId",
                table: "TeacherGrade",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherGrade_Grades_GradeId",
                table: "TeacherGrade",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherGrade_Teachers_TeacherId",
                table: "TeacherGrade",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubject_Subjects_SubjectId",
                table: "TeacherSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubject_Teachers_TeacherId",
                table: "TeacherSubject",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
