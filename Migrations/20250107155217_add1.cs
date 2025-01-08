using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class add1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "TblEmployee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployee_QuestionId",
                table: "TblEmployee",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblEmployee_TblQuestion_QuestionId",
                table: "TblEmployee",
                column: "QuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblEmployee_TblQuestion_QuestionId",
                table: "TblEmployee");

            migrationBuilder.DropIndex(
                name: "IX_TblEmployee_QuestionId",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TblEmployee");
        }
    }
}
