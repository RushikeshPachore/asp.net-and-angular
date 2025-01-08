using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class fk12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblAnswer_TblQuestion_QuestionId",
                table: "TblAnswer");

            migrationBuilder.AddForeignKey(
                name: "FK_TblAnswer_TblQuestion_QuestionId",
                table: "TblAnswer",
                column: "QuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblAnswer_TblQuestion_QuestionId",
                table: "TblAnswer");

            migrationBuilder.AddForeignKey(
                name: "FK_TblAnswer_TblQuestion_QuestionId",
                table: "TblAnswer",
                column: "QuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
