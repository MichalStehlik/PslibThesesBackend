﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    RGB = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    AuthorityId = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CanBeAuthor = table.Column<bool>(nullable: false),
                    CanBeEvaluator = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AlternateKey_AuthorityId", x => x.AuthorityId);
                });

            migrationBuilder.CreateTable(
                name: "Ideas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Resources = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Participants = table.Column<int>(nullable: false),
                    Offered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ideas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdeaGoals",
                columns: table => new
                {
                    IdeaId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaGoals", x => new { x.IdeaId, x.Order });
                    table.ForeignKey(
                        name: "FK_IdeaGoals_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdeaOutlines",
                columns: table => new
                {
                    IdeaId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaOutlines", x => new { x.IdeaId, x.Order });
                    table.ForeignKey(
                        name: "FK_IdeaOutlines_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdeaTargets",
                columns: table => new
                {
                    IdeaId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaTargets", x => new { x.IdeaId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_IdeaTargets_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdeaTargets_Targets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Targets",
                columns: new[] { "Id", "RGB", "Text" },
                values: new object[,]
                {
                    { 1, -256, "MP Lyceum" },
                    { 2, -23296, "RP Lyceum" },
                    { 3, -65536, "MP IT" },
                    { 4, -16776961, "MP Strojírenství" },
                    { 5, -16744448, "MP Elektrotechnika" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_UserId",
                table: "Ideas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaTargets_TargetId",
                table: "IdeaTargets",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdeaGoals");

            migrationBuilder.DropTable(
                name: "IdeaOutlines");

            migrationBuilder.DropTable(
                name: "IdeaTargets");

            migrationBuilder.DropTable(
                name: "Ideas");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}