﻿// <auto-generated />
using System;
using LeagueManager.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LeagueManager.Persistence.EntityFramework.Migrations
{
    [DbContext(typeof(LeagueManagerDbContext))]
    [Migration("20200129063916_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LeagueManager.Domain.Common.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<byte[]>("Flag");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competition.TeamLeague", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId");

                    b.Property<byte[]>("Logo");

                    b.Property<string>("Name");

                    b.Property<int?>("PointSystemId");

                    b.Property<int?>("SportsId");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("PointSystemId");

                    b.HasIndex("SportsId");

                    b.ToTable("TeamLeagues");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competitor.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId");

                    b.Property<byte[]>("Logo");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competitor.TeamCompetitor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("TeamId");

                    b.Property<int?>("TeamLeagueId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamLeagueId");

                    b.ToTable("TeamCompetitor");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competitor.TeamCompetitorPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number");

                    b.Property<int?>("PlayerId");

                    b.Property<int?>("TeamCompetitorId");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamCompetitorId");

                    b.ToTable("TeamCompetitorPlayer");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamLeagueMatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Guid");

                    b.Property<DateTime?>("StartTime");

                    b.Property<int?>("TeamLeagueRoundId");

                    b.HasKey("Id");

                    b.HasIndex("TeamLeagueRoundId");

                    b.ToTable("TeamLeagueMatch");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatchEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HomeAway");

                    b.Property<int?>("ScoreId");

                    b.Property<int?>("TeamId");

                    b.Property<int?>("TeamLeagueMatchId");

                    b.HasKey("Id");

                    b.HasIndex("ScoreId");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamLeagueMatchId");

                    b.ToTable("TeamMatchEntry");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatchEntryLineupEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Guid");

                    b.Property<string>("Number");

                    b.Property<int?>("PlayerId");

                    b.Property<int?>("TeamMatchEntryId");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamMatchEntryId");

                    b.ToTable("TeamMatchEntryLineupEntry");
                });

            modelBuilder.Entity("LeagueManager.Domain.Player.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("LeagueManager.Domain.Points.PointSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Draw");

                    b.Property<int>("Lost");

                    b.Property<int>("Win");

                    b.HasKey("Id");

                    b.ToTable("PointSystem");
                });

            modelBuilder.Entity("LeagueManager.Domain.Round.TeamLeagueRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("TeamLeagueId");

                    b.HasKey("Id");

                    b.HasIndex("TeamLeagueId");

                    b.ToTable("TeamLeagueRound");
                });

            modelBuilder.Entity("LeagueManager.Domain.Score.IntegerScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Value");

                    b.HasKey("Id");

                    b.ToTable("IntegerScore");
                });

            modelBuilder.Entity("LeagueManager.Domain.Sports.TeamSports", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("OptionsId");

                    b.HasKey("Id");

                    b.HasIndex("OptionsId");

                    b.ToTable("TeamSports");
                });

            modelBuilder.Entity("LeagueManager.Domain.Sports.TeamSportsOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AmountOfPlayers");

                    b.HasKey("Id");

                    b.ToTable("TeamSportsOptions");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competition.TeamLeague", b =>
                {
                    b.HasOne("LeagueManager.Domain.Common.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("LeagueManager.Domain.Points.PointSystem", "PointSystem")
                        .WithMany()
                        .HasForeignKey("PointSystemId");

                    b.HasOne("LeagueManager.Domain.Sports.TeamSports", "Sports")
                        .WithMany()
                        .HasForeignKey("SportsId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competitor.Team", b =>
                {
                    b.HasOne("LeagueManager.Domain.Common.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competitor.TeamCompetitor", b =>
                {
                    b.HasOne("LeagueManager.Domain.Competitor.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("LeagueManager.Domain.Competition.TeamLeague")
                        .WithMany("Competitors")
                        .HasForeignKey("TeamLeagueId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Competitor.TeamCompetitorPlayer", b =>
                {
                    b.HasOne("LeagueManager.Domain.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("LeagueManager.Domain.Competitor.TeamCompetitor")
                        .WithMany("Players")
                        .HasForeignKey("TeamCompetitorId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamLeagueMatch", b =>
                {
                    b.HasOne("LeagueManager.Domain.Round.TeamLeagueRound", "TeamLeagueRound")
                        .WithMany("Matches")
                        .HasForeignKey("TeamLeagueRoundId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatchEntry", b =>
                {
                    b.HasOne("LeagueManager.Domain.Score.IntegerScore", "Score")
                        .WithMany()
                        .HasForeignKey("ScoreId");

                    b.HasOne("LeagueManager.Domain.Competitor.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("LeagueManager.Domain.Match.TeamLeagueMatch")
                        .WithMany("MatchEntries")
                        .HasForeignKey("TeamLeagueMatchId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatchEntryLineupEntry", b =>
                {
                    b.HasOne("LeagueManager.Domain.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("LeagueManager.Domain.Match.TeamMatchEntry", "TeamMatchEntry")
                        .WithMany("Lineup")
                        .HasForeignKey("TeamMatchEntryId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Round.TeamLeagueRound", b =>
                {
                    b.HasOne("LeagueManager.Domain.Competition.TeamLeague", "TeamLeague")
                        .WithMany("Rounds")
                        .HasForeignKey("TeamLeagueId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Sports.TeamSports", b =>
                {
                    b.HasOne("LeagueManager.Domain.Sports.TeamSportsOptions", "Options")
                        .WithMany()
                        .HasForeignKey("OptionsId");
                });
#pragma warning restore 612, 618
        }
    }
}