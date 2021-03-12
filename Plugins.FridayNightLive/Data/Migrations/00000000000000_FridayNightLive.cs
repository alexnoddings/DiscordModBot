using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elvet.FridayNightLive.Data.Migrations
{
    public partial class FridayNightLive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FnlGuildConfig",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    WinnerRoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    HostRoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaderboardMessageId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnlGuildConfig", x => x.GuildId)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "FnlSessions",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnlSessions", x => new { x.GuildId, x.Number })
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "FnlSessionHosts",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnlSessionHosts", x => new { x.GuildId, x.SessionNumber, x.UserId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_FnlSessionHosts_FnlSessions_GuildId_SessionNumber",
                        columns: x => new { x.GuildId, x.SessionNumber },
                        principalTable: "FnlSessions",
                        principalColumns: new[] { "GuildId", "Number" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FnlSessionWinners",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnlSessionWinners", x => new { x.GuildId, x.SessionNumber, x.UserId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_FnlSessionWinners_FnlSessions_GuildId_SessionNumber",
                        columns: x => new { x.GuildId, x.SessionNumber },
                        principalTable: "FnlSessions",
                        principalColumns: new[] { "GuildId", "Number" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FnlGuildConfig");

            migrationBuilder.DropTable(
                name: "FnlSessionHosts");

            migrationBuilder.DropTable(
                name: "FnlSessionWinners");

            migrationBuilder.DropTable(
                name: "FnlSessions");
        }
    }
}
