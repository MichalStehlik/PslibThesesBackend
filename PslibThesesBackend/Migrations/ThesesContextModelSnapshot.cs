﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PslibThesesBackend.Models;

namespace PslibThesesBackend.Migrations
{
    [DbContext(typeof(ThesesContext))]
    partial class ThesesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ideas");
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

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaOutline", b =>
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

            modelBuilder.Entity("PslibThesesBackend.Models.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("MaxGrade")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequiredGoals")
                        .HasColumnType("int");

                    b.Property<int>("RequiredOutlines")
                        .HasColumnType("int");

                    b.Property<int>("Template")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Critical")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("SetQuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SetQuestionId");

                    b.ToTable("SetAnswers");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("SetRoleId")
                        .HasColumnType("int");

                    b.Property<int>("SetTermId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SetRoleId");

                    b.HasIndex("SetTermId");

                    b.ToTable("SetQuestions");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ClassTeacher")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequiredForAdvancement")
                        .HasColumnType("bit");

                    b.Property<bool>("RequiredForPrint")
                        .HasColumnType("bit");

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<bool>("ShowsOnApplication")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("SetRoles");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetTerm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("WarningDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("SetTerms");
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

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

            modelBuilder.Entity("PslibThesesBackend.Models.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DetailExpenditures")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaterialCosts")
                        .HasColumnType("int");

                    b.Property<int>("MaterialCostsProvidedBySchool")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resources")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("ServicesCosts")
                        .HasColumnType("int");

                    b.Property<int>("ServicesCostsProvidedBySchool")
                        .HasColumnType("int");

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("SetId");

                    b.HasIndex("UserId");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkId");

                    b.ToTable("WorkGoals");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkOutline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkId");

                    b.ToTable("WorkOutlines");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Finalized")
                        .HasColumnType("bit");

                    b.Property<int?>("Mark")
                        .HasColumnType("int");

                    b.Property<string>("Review")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("SetRoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("WorkId");

                    b.ToTable("WorkRoles");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkRoleUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AssignedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("WorkRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkRoleId");

                    b.ToTable("WorkRoleUsers");
                });

            modelBuilder.Entity("PslibThesesBackend.Models.Idea", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.User", "User")
                        .WithMany("OwnedIdeas")
                        .HasForeignKey("UserId")
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

            modelBuilder.Entity("PslibThesesBackend.Models.IdeaOutline", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Idea", "Idea")
                        .WithMany("Outlines")
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

            modelBuilder.Entity("PslibThesesBackend.Models.SetAnswer", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.SetQuestion", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("SetQuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetQuestion", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.SetRole", "Role")
                        .WithMany("Questions")
                        .HasForeignKey("SetRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibThesesBackend.Models.SetTerm", "Term")
                        .WithMany("Questions")
                        .HasForeignKey("SetTermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetRole", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Set", "Set")
                        .WithMany("Roles")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.SetTerm", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Set", "Set")
                        .WithMany("Terms")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.Work", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.User", "Author")
                        .WithMany("AuthoredWorks")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PslibThesesBackend.Models.User", "Manager")
                        .WithMany("ManagedWorks")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PslibThesesBackend.Models.Set", "Set")
                        .WithMany("Works")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PslibThesesBackend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkGoal", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Work", "Work")
                        .WithMany("Goals")
                        .HasForeignKey("WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkOutline", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.Work", "Work")
                        .WithMany("Outlines")
                        .HasForeignKey("WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkRole", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.SetRole", "SetRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("PslibThesesBackend.Models.Work", "Work")
                        .WithMany()
                        .HasForeignKey("WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PslibThesesBackend.Models.WorkRoleUser", b =>
                {
                    b.HasOne("PslibThesesBackend.Models.User", "User")
                        .WithMany("WorkRoleUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PslibThesesBackend.Models.WorkRole", "WorkRole")
                        .WithMany("WorkRoleUsers")
                        .HasForeignKey("WorkRoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
