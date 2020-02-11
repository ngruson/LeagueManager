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
                    Value = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegerScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointSystem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Win = table.Column<int>(nullable: false),
                    Draw = table.Column<int>(nullable: false),
                    Lost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamSportsOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountOfPlayers = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamSportsOptions", x => x.Id);
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
                name: "TeamSports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    OptionsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamSports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamSports_TeamSportsOptions_OptionsId",
                        column: x => x.OptionsId,
                        principalTable: "TeamSportsOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamLeagues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SportsId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Logo = table.Column<byte[]>(nullable: true),
                    PointSystemId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_TeamLeagues_PointSystem_PointSystemId",
                        column: x => x.PointSystemId,
                        principalTable: "PointSystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamLeagues_TeamSports_SportsId",
                        column: x => x.SportsId,
                        principalTable: "TeamSports",
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
                name: "TeamCompetitorPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(nullable: true),
                    PlayerId = table.Column<int>(nullable: true),
                    TeamCompetitorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamCompetitorPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamCompetitorPlayer_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamCompetitorPlayer_TeamCompetitor_TeamCompetitorId",
                        column: x => x.TeamCompetitorId,
                        principalTable: "TeamCompetitor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamLeagueMatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(nullable: false),
                    TeamLeagueRoundId = table.Column<int>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLeagueMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamLeagueMatch_TeamLeagueRound_TeamLeagueRoundId",
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
                    HomeAway = table.Column<int>(nullable: false),
                    ScoreId = table.Column<int>(nullable: true),
                    TeamLeagueMatchId = table.Column<int>(nullable: true)
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
                        name: "FK_TeamMatchEntry_TeamLeagueMatch_TeamLeagueMatchId",
                        column: x => x.TeamLeagueMatchId,
                        principalTable: "TeamLeagueMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMatchEntryLineupEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamMatchEntryId = table.Column<int>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    PlayerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMatchEntryLineupEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntryLineupEntry_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMatchEntryLineupEntry_TeamMatchEntry_TeamMatchEntryId",
                        column: x => x.TeamMatchEntryId,
                        principalTable: "TeamMatchEntry",
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
                name: "IX_TeamCompetitorPlayer_PlayerId",
                table: "TeamCompetitorPlayer",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamCompetitorPlayer_TeamCompetitorId",
                table: "TeamCompetitorPlayer",
                column: "TeamCompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagueMatch_TeamLeagueRoundId",
                table: "TeamLeagueMatch",
                column: "TeamLeagueRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagueRound_TeamLeagueId",
                table: "TeamLeagueRound",
                column: "TeamLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_CountryId",
                table: "TeamLeagues",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_PointSystemId",
                table: "TeamLeagues",
                column: "PointSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeagues_SportsId",
                table: "TeamLeagues",
                column: "SportsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntry_ScoreId",
                table: "TeamMatchEntry",
                column: "ScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntry_TeamId",
                table: "TeamMatchEntry",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntry_TeamLeagueMatchId",
                table: "TeamMatchEntry",
                column: "TeamLeagueMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntryLineupEntry_PlayerId",
                table: "TeamMatchEntryLineupEntry",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchEntryLineupEntry_TeamMatchEntryId",
                table: "TeamMatchEntryLineupEntry",
                column: "TeamMatchEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CountryId",
                table: "Teams",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSports_OptionsId",
                table: "TeamSports",
                column: "OptionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamCompetitorPlayer");

            migrationBuilder.DropTable(
                name: "TeamMatchEntryLineupEntry");

            migrationBuilder.DropTable(
                name: "TeamCompetitor");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "TeamMatchEntry");

            migrationBuilder.DropTable(
                name: "IntegerScore");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "TeamLeagueMatch");

            migrationBuilder.DropTable(
                name: "TeamLeagueRound");

            migrationBuilder.DropTable(
                name: "TeamLeagues");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "PointSystem");

            migrationBuilder.DropTable(
                name: "TeamSports");

            migrationBuilder.DropTable(
                name: "TeamSportsOptions");
        }
    }
}
