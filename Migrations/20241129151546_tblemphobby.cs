using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class tblemphobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblEmployeeHobbies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<int>(type: "int", nullable: false),
                    HobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblEmployeeHobbies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblEmployeeHobbies_TblEmployee_EmpId",
                        column: x => x.EmpId,
                        principalTable: "TblEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblEmployeeHobbies_TblHobbies_HobId",
                        column: x => x.HobId,
                        principalTable: "TblHobbies",
                        principalColumn: "HobbyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployeeHobbies_EmpId",
                table: "TblEmployeeHobbies",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployeeHobbies_HobId",
                table: "TblEmployeeHobbies",
                column: "HobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblEmployeeHobbies");
        }
    }
}
