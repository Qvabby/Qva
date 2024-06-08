using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qvastart___1.Migrations
{
    /// <inheritdoc />
    public partial class imageHandlingSwitch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "ImagesTB");

            migrationBuilder.DropColumn(
                name: "_ContentDisposition",
                table: "ImagesTB");

            migrationBuilder.DropColumn(
                name: "_ContentType",
                table: "ImagesTB");

            migrationBuilder.RenameColumn(
                name: "base64",
                table: "ImagesTB",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "ImagesTB",
                newName: "base64");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "ImagesTB",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

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
    }
}
