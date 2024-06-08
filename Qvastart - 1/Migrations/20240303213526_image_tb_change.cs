using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qvastart___1.Migrations
{
    /// <inheritdoc />
    public partial class image_tb_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_ContentDisposition",
                table: "ImagesTB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "_ContentType",
                table: "ImagesTB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_ContentDisposition",
                table: "ImagesTB");

            migrationBuilder.DropColumn(
                name: "_ContentType",
                table: "ImagesTB");
        }
    }
}
