using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pd311_web_api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserSecondUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarImage",
                columns: table => new
                {
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    CarId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImage", x => x.FileName);
                    table.ForeignKey(
                        name: "FK_CarImage_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarImage_CarId",
                table: "CarImage",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarImage");
        }
    }
}
