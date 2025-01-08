using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class erttat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblAnswer_TblEmployee_EmployeeId",
                table: "TblAnswer");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "TblAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TblAnswer_TblEmployee_EmployeeId",
                table: "TblAnswer",
                column: "EmployeeId",
                principalTable: "TblEmployee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblAnswer_TblEmployee_EmployeeId",
                table: "TblAnswer");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "TblAnswer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TblAnswer_TblEmployee_EmployeeId",
                table: "TblAnswer",
                column: "EmployeeId",
                principalTable: "TblEmployee",
                principalColumn: "Id");
        }
    }
}
