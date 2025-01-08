using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class quesremove1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblEmployee_TblQuestion_TblQuestionId",
                table: "TblEmployee");

            migrationBuilder.DropIndex(
                name: "IX_TblEmployee_TblQuestionId",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "TblQuestionId",
                table: "TblEmployee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TblQuestionId",
                table: "TblEmployee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployee_TblQuestionId",
                table: "TblEmployee",
                column: "TblQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblEmployee_TblQuestion_TblQuestionId",
                table: "TblEmployee",
                column: "TblQuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id");
        }
    }
}
