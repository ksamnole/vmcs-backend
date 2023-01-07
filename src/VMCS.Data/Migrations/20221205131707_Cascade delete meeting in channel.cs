using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Cascadedeletemeetinginchannel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Channels_ChannelId",
                table: "Meetings");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Channels_ChannelId",
                table: "Meetings",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Channels_ChannelId",
                table: "Meetings");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Channels_ChannelId",
                table: "Meetings",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id");
        }
    }
}
