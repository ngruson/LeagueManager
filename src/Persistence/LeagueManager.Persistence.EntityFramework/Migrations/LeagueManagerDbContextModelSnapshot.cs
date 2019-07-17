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

            modelBuilder.Entity("LeagueManager.Domain.Entities.CompetingTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("TeamId");

                    b.Property<int?>("TeamLeagueId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamLeagueId");

                    b.ToTable("CompetingTeam");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Competition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<byte[]>("Logo");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Competitions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Competition");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Country", b =>
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

            modelBuilder.Entity("LeagueManager.Domain.Entities.LeagueRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("TeamLeagueId");

                    b.HasKey("Id");

                    b.HasIndex("TeamLeagueId");

                    b.ToTable("LeagueRound");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("LeagueRoundId");

                    b.Property<DateTime?>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("LeagueRoundId");

                    b.ToTable("Match");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Match");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Score", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Score");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Score");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Team", b =>
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

            modelBuilder.Entity("LeagueManager.Domain.Entities.TeamMatchEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ScoreId");

                    b.Property<int?>("TeamId");

                    b.Property<int?>("TeamMatchId");

                    b.HasKey("Id");

                    b.HasIndex("ScoreId");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamMatchId");

                    b.ToTable("TeamMatchEntry");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.TeamLeague", b =>
                {
                    b.HasBaseType("LeagueManager.Domain.Entities.Competition");

                    b.HasDiscriminator().HasValue("TeamLeague");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.TeamMatch", b =>
                {
                    b.HasBaseType("LeagueManager.Domain.Entities.Match");

                    b.HasDiscriminator().HasValue("TeamMatch");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.IntegerScore", b =>
                {
                    b.HasBaseType("LeagueManager.Domain.Entities.Score");

                    b.Property<int>("Value");

                    b.HasDiscriminator().HasValue("IntegerScore");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.CompetingTeam", b =>
                {
                    b.HasOne("LeagueManager.Domain.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("LeagueManager.Domain.Entities.TeamLeague")
                        .WithMany("Teams")
                        .HasForeignKey("TeamLeagueId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.LeagueRound", b =>
                {
                    b.HasOne("LeagueManager.Domain.Entities.TeamLeague")
                        .WithMany("Rounds")
                        .HasForeignKey("TeamLeagueId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Match", b =>
                {
                    b.HasOne("LeagueManager.Domain.Entities.LeagueRound")
                        .WithMany("Matches")
                        .HasForeignKey("LeagueRoundId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.Team", b =>
                {
                    b.HasOne("LeagueManager.Domain.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("LeagueManager.Domain.Entities.TeamMatchEntry", b =>
                {
                    b.HasOne("LeagueManager.Domain.Entities.Score", "Score")
                        .WithMany()
                        .HasForeignKey("ScoreId");

                    b.HasOne("LeagueManager.Domain.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.HasOne("LeagueManager.Domain.Entities.TeamMatch")
                        .WithMany("MatchEntries")
                        .HasForeignKey("TeamMatchId");
                });
#pragma warning restore 612, 618
        }
    }
}
