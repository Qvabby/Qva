using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qvastart___1.Migrations
{
    /// <inheritdoc />
    public partial class base64added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "base64",
                table: "ImagesTB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "base64",
                table: "ImagesTB");
        }
    }
}
