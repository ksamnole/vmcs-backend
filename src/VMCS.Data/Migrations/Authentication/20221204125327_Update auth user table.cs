#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace VMCS.Data.Migrations.Authentication;

/// <inheritdoc />
public partial class Updateauthusertable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "GivenName",
            "AspNetUsers",
            "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "GivenName",
            "AspNetUsers");
    }
}