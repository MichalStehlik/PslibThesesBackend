using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class SetsStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WarningDate",
                table: "SetTerms",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RequiredGoals",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredOutlines",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequiredForAdvancement",
                table: "SetRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Work",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Resources = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AuthorId = table.Column<string>(nullable: false),
                    ClassName = table.Column<string>(nullable: true),
                    ManagerId = table.Column<string>(nullable: false),
                    SetId = table.Column<int>(nullable: false),
                    MaterialCosts = table.Column<int>(nullable: false),
                    MaterialCostsBySchool = table.Column<int>(nullable: false),
                    ServicesCosts = table.Column<int>(nullable: false),
                    ServicesCostsBySchool = table.Column<int>(nullable: false),
                    DetailExpenditures = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Work_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Work_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Work_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Work_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkGoal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    WorkId1 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkGoal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkGoal_Ideas_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkGoal_Work_WorkId1",
                        column: x => x.WorkId1,
                        principalTable: "Work",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOutline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    WorkId1 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOutline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOutline_Ideas_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOutline_Work_WorkId1",
                        column: x => x.WorkId1,
                        principalTable: "Work",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Work_AuthorId",
                table: "Work",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Work_ManagerId",
                table: "Work",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Work_SetId",
                table: "Work",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_Work_UserId",
                table: "Work",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkGoal_WorkId",
                table: "WorkGoal",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkGoal_WorkId1",
                table: "WorkGoal",
                column: "WorkId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOutline_WorkId",
                table: "WorkOutline",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOutline_WorkId1",
                table: "WorkOutline",
                column: "WorkId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkGoal");

            migrationBuilder.DropTable(
                name: "WorkOutline");

            migrationBuilder.DropTable(
                name: "Work");

            migrationBuilder.DropColumn(
                name: "WarningDate",
                table: "SetTerms");

            migrationBuilder.DropColumn(
                name: "RequiredGoals",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "RequiredOutlines",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "RequiredForAdvancement",
                table: "SetRoles");
        }
    }
}
