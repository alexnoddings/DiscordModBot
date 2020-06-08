using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SishIndustries.Discord.ModBot.Plugins.ReRole.Migrations
{
    public partial class DbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreviousRoleDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Added = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<decimal>(nullable: false),
                    GuildId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousRoleDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreviousRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DetailsId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousRoles_PreviousRoleDetails_DetailsId",
                        column: x => x.DetailsId,
                        principalTable: "PreviousRoleDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousRoles_DetailsId",
                table: "PreviousRoles",
                column: "DetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreviousRoles");

            migrationBuilder.DropTable(
                name: "PreviousRoleDetails");
        }
    }
}
