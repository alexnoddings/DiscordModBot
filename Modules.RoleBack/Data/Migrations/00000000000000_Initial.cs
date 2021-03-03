using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elvet.RoleBack.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGuildRoles",
                columns: table => new
                {
                    Guild = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    User = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGuildRoles", x => new { x.User, x.Guild })
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGuildRoles");
        }
    }
}
