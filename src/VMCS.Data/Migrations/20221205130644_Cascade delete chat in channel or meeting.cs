using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Cascadedeletechatinchannelormeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Channels_ChannelId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Meetings_MeetingId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ChannelId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_MeetingId",
                table: "Chats");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ChatId",
                table: "Meetings",
                column: "ChatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ChatId",
                table: "Channels",
                column: "ChatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Chats_ChatId",
                table: "Channels",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Chats_ChatId",
                table: "Meetings",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Chats_ChatId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Chats_ChatId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_ChatId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Channels_ChatId",
                table: "Channels");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChannelId",
                table: "Chats",
                column: "ChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_MeetingId",
                table: "Chats",
                column: "MeetingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Channels_ChannelId",
                table: "Chats",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Meetings_MeetingId",
                table: "Chats",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
