using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarUrifieldtoChanneltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUri",
                table: "Channels",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUri",
                table: "Channels");
        }
    }
}
