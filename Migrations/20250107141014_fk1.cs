using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class fk1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "TblAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TblAnswer_QuestionId",
                table: "TblAnswer",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblAnswer_TblQuestion_QuestionId",
                table: "TblAnswer",
                column: "QuestionId",
                principalTable: "TblQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblAnswer_TblQuestion_QuestionId",
                table: "TblAnswer");

            migrationBuilder.DropIndex(
                name: "IX_TblAnswer_QuestionId",
                table: "TblAnswer");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TblAnswer");
        }
    }
}
