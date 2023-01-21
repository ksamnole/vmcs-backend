using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Adddirectorymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepositoryId",
                table: "Meetings",
                newName: "DirectoryId");

            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DirectoryInJson = table.Column<string>(type: "text", nullable: false),
                    DirectoryZip = table.Column<byte[]>(type: "bytea", nullable: false),
                    MeetingId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Directories_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Directories_MeetingId",
                table: "Directories",
                column: "MeetingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Directories");

            migrationBuilder.RenameColumn(
                name: "DirectoryId",
                table: "Meetings",
                newName: "RepositoryId");
        }
    }
}
