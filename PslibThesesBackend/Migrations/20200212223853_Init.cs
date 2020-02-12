using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class Init : Migration
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
                    Template = table.Column<int>(nullable: false),
                    RequiredGoals = table.Column<int>(nullable: false),
                    RequiredOutlines = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                });

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
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CanBeAuthor = table.Column<bool>(nullable: false),
                    CanBeEvaluator = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                    RequiredForAdvancement = table.Column<bool>(nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetTerms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    SetId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    WarningDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetTerms_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    UserId = table.Column<Guid>(nullable: false),
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
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Resources = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AuthorId = table.Column<Guid>(nullable: false),
                    ClassName = table.Column<string>(nullable: true),
                    ManagerId = table.Column<Guid>(nullable: false),
                    SetId = table.Column<int>(nullable: false),
                    MaterialCosts = table.Column<int>(nullable: false),
                    MaterialCostsProvidedBySchool = table.Column<int>(nullable: false),
                    ServicesCosts = table.Column<int>(nullable: false),
                    ServicesCostsProvidedBySchool = table.Column<int>(nullable: false),
                    DetailExpenditures = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Works_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Works_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Works_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    SetTermId = table.Column<int>(nullable: false),
                    SetRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetQuestions_SetRoles_SetRoleId",
                        column: x => x.SetRoleId,
                        principalTable: "SetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetQuestions_SetTerms_SetTermId",
                        column: x => x.SetTermId,
                        principalTable: "SetTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdeaGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaGoals", x => x.Id);
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaOutlines", x => x.Id);
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdeaId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaTargets", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "WorkGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    WorkId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkGoals_Ideas_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkGoals_Works_WorkId1",
                        column: x => x.WorkId1,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOutlines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    WorkId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOutlines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOutlines_Ideas_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOutlines_Works_WorkId1",
                        column: x => x.WorkId1,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    Critical = table.Column<bool>(nullable: false),
                    SetQuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetAnswers_SetQuestions_SetQuestionId",
                        column: x => x.SetQuestionId,
                        principalTable: "SetQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_IdeaGoals_IdeaId_Order",
                table: "IdeaGoals",
                columns: new[] { "IdeaId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOutlines_IdeaId_Order",
                table: "IdeaOutlines",
                columns: new[] { "IdeaId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_UserId",
                table: "Ideas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaTargets_TargetId",
                table: "IdeaTargets",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaTargets_IdeaId_TargetId",
                table: "IdeaTargets",
                columns: new[] { "IdeaId", "TargetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetAnswers_SetQuestionId",
                table: "SetAnswers",
                column: "SetQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SetQuestions_SetRoleId",
                table: "SetQuestions",
                column: "SetRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SetQuestions_SetTermId",
                table: "SetQuestions",
                column: "SetTermId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRoles_SetId",
                table: "SetRoles",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetTerms_SetId",
                table: "SetTerms",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkGoals_WorkId",
                table: "WorkGoals",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkGoals_WorkId1",
                table: "WorkGoals",
                column: "WorkId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOutlines_WorkId",
                table: "WorkOutlines",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOutlines_WorkId1",
                table: "WorkOutlines",
                column: "WorkId1");

            migrationBuilder.CreateIndex(
                name: "IX_Works_AuthorId",
                table: "Works",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_ManagerId",
                table: "Works",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_SetId",
                table: "Works",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_UserId",
                table: "Works",
                column: "UserId");
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
                name: "SetAnswers");

            migrationBuilder.DropTable(
                name: "WorkGoals");

            migrationBuilder.DropTable(
                name: "WorkOutlines");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "SetQuestions");

            migrationBuilder.DropTable(
                name: "Ideas");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "SetRoles");

            migrationBuilder.DropTable(
                name: "SetTerms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sets");
        }
    }
}
