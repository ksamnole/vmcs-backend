using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updatedirectory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Directories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Directories");
        }
    }
}
