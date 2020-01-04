using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class Sets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    MaxGrade = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Template = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    ClassTeacher = table.Column<bool>(nullable: false),
                    RequiredForPrint = table.Column<bool>(nullable: false),
                    ShowsOnApplication = table.Column<bool>(nullable: false),
                    SetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetRoles_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetTerms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    SetId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetTerms_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetRoles_SetId",
                table: "SetRoles",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetTerms_SetId",
                table: "SetTerms",
                column: "SetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetRoles");

            migrationBuilder.DropTable(
                name: "SetTerms");

            migrationBuilder.DropTable(
                name: "Sets");
        }
    }
}
