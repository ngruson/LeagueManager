using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LeagueManager.Persistence.EntityFramework.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamMatchEntrySubstitution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamMatchEntryId = table.Column<int>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false),
                    Minute = table.Column<string>(nullable: true),
                    PlayerOutId = table.Column<int>(nullable: true),
                    PlayerInId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMatchEntrySubstitution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntrySubstitution_Players_PlayerInId",
                        column: x => x.PlayerInId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntrySubstitution_Players_PlayerOutId",
                        column: x => x.PlayerOutId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntrySubstitution_TeamMatchEntry_TeamMatchEntryId",
                        column: x => x.TeamMatchEntryId,
                        principalTable: "TeamMatchEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntrySubstitution_PlayerInId",
                table: "TeamMatchEntrySubstitution",
                column: "PlayerInId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntrySubstitution_PlayerOutId",
                table: "TeamMatchEntrySubstitution",
                column: "PlayerOutId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntrySubstitution_TeamMatchEntryId",
                table: "TeamMatchEntrySubstitution",
                column: "TeamMatchEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMatchEntrySubstitution");
        }
    }
}
