using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class removeImageFromTblEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "TblEmployee");

            migrationBuilder.CreateTable(
                name: "TblImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    MultiImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblImage_TblEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "TblEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblImage_EmployeeId",
                table: "TblImage",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblImage");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "TblEmployee",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
