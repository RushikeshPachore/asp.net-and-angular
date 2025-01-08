using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class quesremove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblEmployee_TblQuestion_QuestionId",
                table: "TblEmployee");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "TblEmployee",
                newName: "TblQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_TblEmployee_QuestionId",
                table: "TblEmployee",
                newName: "IX_TblEmployee_TblQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblEmployee_TblQuestion_TblQuestionId",
                table: "TblEmployee",
                column: "TblQuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblEmployee_TblQuestion_TblQuestionId",
                table: "TblEmployee");

            migrationBuilder.RenameColumn(
                name: "TblQuestionId",
                table: "TblEmployee",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_TblEmployee_TblQuestionId",
                table: "TblEmployee",
                newName: "IX_TblEmployee_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblEmployee_TblQuestion_QuestionId",
                table: "TblEmployee",
                column: "QuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
