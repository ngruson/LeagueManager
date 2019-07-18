using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LeagueManager.Persistence.EntityFramework.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Flag = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegerScore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegerScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamLeagues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Logo = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLeagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamLeagues_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Logo = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamLeagueRound",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    TeamLeagueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLeagueRound", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamLeagueRound_TeamLeagues_TeamLeagueId",
                        column: x => x.TeamLeagueId,
                        principalTable: "TeamLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamCompetitor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(nullable: true),
                    TeamLeagueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamCompetitor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamCompetitor_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamCompetitor_TeamLeagues_TeamLeagueId",
                        column: x => x.TeamLeagueId,
                        principalTable: "TeamLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(nullable: true),
                    TeamLeagueRoundId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMatch_TeamLeagueRound_TeamLeagueRoundId",
                        column: x => x.TeamLeagueRoundId,
                        principalTable: "TeamLeagueRound",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMatchEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(nullable: true),
                    ScoreId = table.Column<int>(nullable: true),
                    TeamMatchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMatchEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntry_IntegerScore_ScoreId",
                        column: x => x.ScoreId,
                        principalTable: "IntegerScore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntry_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntry_TeamMatch_TeamMatchId",
                        column: x => x.TeamMatchId,
                        principalTable: "TeamMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamCompetitor_TeamId",
                table: "TeamCompetitor",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamCompetitor_TeamLeagueId",
                table: "TeamCompetitor",
                column: "TeamLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagueRound_TeamLeagueId",
                table: "TeamLeagueRound",
                column: "TeamLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_CountryId",
                table: "TeamLeagues",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatch_TeamLeagueRoundId",
                table: "TeamMatch",
                column: "TeamLeagueRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntry_ScoreId",
                table: "TeamMatchEntry",
                column: "ScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntry_TeamId",
                table: "TeamMatchEntry",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntry_TeamMatchId",
                table: "TeamMatchEntry",
                column: "TeamMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CountryId",
                table: "Teams",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamCompetitor");

            migrationBuilder.DropTable(
                name: "TeamMatchEntry");

            migrationBuilder.DropTable(
                name: "IntegerScore");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "TeamMatch");

            migrationBuilder.DropTable(
                name: "TeamLeagueRound");

            migrationBuilder.DropTable(
                name: "TeamLeagues");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
