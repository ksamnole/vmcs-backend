using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChannelInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelInvitations_Users_RecepientId",
                table: "ChannelInvitations");

            migrationBuilder.RenameColumn(
                name: "RecepientId",
                table: "ChannelInvitations",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelInvitations_RecepientId",
                table: "ChannelInvitations",
                newName: "IX_ChannelInvitations_RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelInvitations_Users_RecipientId",
                table: "ChannelInvitations",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelInvitations_Users_RecipientId",
                table: "ChannelInvitations");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "ChannelInvitations",
                newName: "RecepientId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelInvitations_RecipientId",
                table: "ChannelInvitations",
                newName: "IX_ChannelInvitations_RecepientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelInvitations_Users_RecepientId",
                table: "ChannelInvitations",
                column: "RecepientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
