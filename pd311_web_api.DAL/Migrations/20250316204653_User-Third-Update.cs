using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pd311_web_api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserThirdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Manufactures_ManufactureId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage");

            migrationBuilder.AlterColumn<string>(
                name: "ManufactureId",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gearbox",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CarImage",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Manufactures_ManufactureId",
                table: "Cars",
                column: "ManufactureId",
                principalTable: "Manufactures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Manufactures_ManufactureId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CarImage");

            migrationBuilder.AlterColumn<string>(
                name: "ManufactureId",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Gearbox",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage",
                column: "FileName");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Manufactures_ManufactureId",
                table: "Cars",
                column: "ManufactureId",
                principalTable: "Manufactures",
                principalColumn: "Id");
        }
    }
}
