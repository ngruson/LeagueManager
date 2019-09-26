﻿// <auto-generated />
using System;
using LeagueManager.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LeagueManager.Persistence.EntityFramework.Migrations
{
    [DbContext(typeof(LeagueManagerDbContext))]
    partial class LeagueManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("PointSystemId");

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

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Guid");

                    b.Property<DateTime?>("StartTime");

                    b.Property<int?>("TeamLeagueRoundId");

                    b.HasKey("Id");

                    b.HasIndex("TeamLeagueRoundId");

                    b.ToTable("TeamMatch");
                });

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatchEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HomeAway");

                    b.Property<int?>("ScoreId");

                    b.Property<int?>("TeamId");

                    b.Property<int?>("TeamMatchId");

                    b.HasKey("Id");

                    b.HasIndex("ScoreId");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamMatchId");

                    b.ToTable("TeamMatchEntry");
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

            modelBuilder.Entity("LeagueManager.Domain.Competition.TeamLeague", b =>
                {
                    b.HasOne("LeagueManager.Domain.Common.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("LeagueManager.Domain.Points.PointSystem", "PointSystem")
                        .WithMany()
                        .HasForeignKey("PointSystemId");
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

            modelBuilder.Entity("LeagueManager.Domain.Match.TeamMatch", b =>
                {
                    b.HasOne("LeagueManager.Domain.Round.TeamLeagueRound")
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

                    b.HasOne("LeagueManager.Domain.Match.TeamMatch")
                        .WithMany("MatchEntries")
                        .HasForeignKey("TeamMatchId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Round.TeamLeagueRound", b =>
                {
                    b.HasOne("LeagueManager.Domain.Competition.TeamLeague")
                        .WithMany("Rounds")
                        .HasForeignKey("TeamLeagueId");
                });
#pragma warning restore 612, 618
        }
    }
}
