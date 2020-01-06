﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PslibThesesBackend.Models;

namespace PslibThesesBackend.Migrations
{
    [DbContext(typeof(ThesesContext))]
    [Migration("20200101162223_Component key Removal")]
    partial class ComponentkeyRemoval
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PslibThesesBackend.Models.Idea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Offered")
                        .HasColumnType("bit");

                    b.Property<int>("Participants")
                        .HasColumnType("int");

                    b.Property<string>("Resources")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ideas");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdeaId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId", "Order")
                        .IsUnique();

                    b.ToTable("IdeaOutlines");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdeaId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId", "Order")
                        .IsUnique();

                    b.ToTable("IdeaGoals");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaTarget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdeaId")
                        .HasColumnType("int");

                    b.Property<int>("TargetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TargetId");

                    b.HasIndex("IdeaId", "TargetId")
                        .IsUnique();

                    b.ToTable("IdeaTargets");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.Target", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RGB")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Targets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RGB = -256,
                            Text = "MP Lyceum"
                        },
                        new
                        {
                            Id = 2,
                            RGB = -23296,
                            Text = "RP Lyceum"
                        },
                        new
                        {
                            Id = 3,
                            RGB = -65536,
                            Text = "MP IT"
                        },
                        new
                        {
                            Id = 4,
                            RGB = -16776961,
                            Text = "MP Strojírenství"
                        },
                        new
                        {
                            Id = 5,
                            RGB = -16744448,
                            Text = "MP Elektrotechnika"
                        });
                });

            modelBuilder.Entity("PslibThesesBackend.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("CanBeAuthor")
                        .HasColumnType("bit");

                    b.Property<bool>("CanBeEvaluator")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.Idea", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaContent", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Idea", "Idea")
                        .WithMany("Contents")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaGoal", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Idea", "Idea")
                        .WithMany("Goals")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaTarget", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Idea", "Idea")
                        .WithMany("IdeaTargets")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibThesesBackend.Models.Target", "Target")
                        .WithMany("Ideas")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}