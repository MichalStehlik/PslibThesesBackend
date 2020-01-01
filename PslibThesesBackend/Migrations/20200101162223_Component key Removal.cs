using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class ComponentkeyRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdeaTargets",
                table: "IdeaTargets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdeaOutlines",
                table: "IdeaOutlines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdeaGoals",
                table: "IdeaGoals");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "IdeaTargets",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "IdeaOutlines",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "IdeaGoals",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdeaTargets",
                table: "IdeaTargets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdeaOutlines",
                table: "IdeaOutlines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdeaGoals",
                table: "IdeaGoals",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaTargets_IdeaId_TargetId",
                table: "IdeaTargets",
                columns: new[] { "IdeaId", "TargetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOutlines_IdeaId_Order",
                table: "IdeaOutlines",
                columns: new[] { "IdeaId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdeaGoals_IdeaId_Order",
                table: "IdeaGoals",
                columns: new[] { "IdeaId", "Order" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdeaTargets",
                table: "IdeaTargets");

            migrationBuilder.DropIndex(
                name: "IX_IdeaTargets_IdeaId_TargetId",
                table: "IdeaTargets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdeaOutlines",
                table: "IdeaOutlines");

            migrationBuilder.DropIndex(
                name: "IX_IdeaOutlines_IdeaId_Order",
                table: "IdeaOutlines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdeaGoals",
                table: "IdeaGoals");

            migrationBuilder.DropIndex(
                name: "IX_IdeaGoals_IdeaId_Order",
                table: "IdeaGoals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "IdeaTargets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "IdeaOutlines");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "IdeaGoals");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdeaTargets",
                table: "IdeaTargets",
                columns: new[] { "IdeaId", "TargetId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdeaOutlines",
                table: "IdeaOutlines",
                columns: new[] { "IdeaId", "Order" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdeaGoals",
                table: "IdeaGoals",
                columns: new[] { "IdeaId", "Order" });
        }
    }
}
