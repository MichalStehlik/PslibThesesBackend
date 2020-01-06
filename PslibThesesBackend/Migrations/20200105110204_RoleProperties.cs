using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class RoleProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetQuestion",
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
                    table.PrimaryKey("PK_SetQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetQuestion_SetRoles_SetRoleId",
                        column: x => x.SetRoleId,
                        principalTable: "SetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetQuestion_SetTerms_SetTermId",
                        column: x => x.SetTermId,
                        principalTable: "SetTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetAnswer",
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
                    table.PrimaryKey("PK_SetAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetAnswer_SetQuestion_SetQuestionId",
                        column: x => x.SetQuestionId,
                        principalTable: "SetQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetAnswer_SetQuestionId",
                table: "SetAnswer",
                column: "SetQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SetQuestion_SetRoleId",
                table: "SetQuestion",
                column: "SetRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SetQuestion_SetTermId",
                table: "SetQuestion",
                column: "SetTermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetAnswer");

            migrationBuilder.DropTable(
                name: "SetQuestion");
        }
    }
}
