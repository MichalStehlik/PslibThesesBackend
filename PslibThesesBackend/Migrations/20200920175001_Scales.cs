using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class Scales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Sets_SetId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Users_UserId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "MaxGrade",
                table: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "ScaleId",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Scales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScaleValues",
                columns: table => new
                {
                    ScaleId = table.Column<int>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    Mark = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleValues", x => new { x.ScaleId, x.Rate });
                    table.ForeignKey(
                        name: "FK_ScaleValues_Scales_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "Scales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Scales",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Stará škála" });

            migrationBuilder.InsertData(
                table: "ScaleValues",
                columns: new[] { "ScaleId", "Rate", "Mark", "Name" },
                values: new object[,]
                {
                    { 1, 1.0, 1.0, "Výborný" },
                    { 1, 0.80000000000000004, 2.0, "Chvalitebný" },
                    { 1, 0.59999999999999998, 3.0, "Dobrý" },
                    { 1, 0.40000000000000002, 4.0, "Dostatečný" },
                    { 1, 0.20000000000000001, 5.0, "Nedostatečný" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ScaleId",
                table: "Sets",
                column: "ScaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Scales_ScaleId",
                table: "Sets",
                column: "ScaleId",
                principalTable: "Scales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Sets_SetId",
                table: "Works",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Users_UserId",
                table: "Works",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Scales_ScaleId",
                table: "Sets");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Sets_SetId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Users_UserId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "ScaleValues");

            migrationBuilder.DropTable(
                name: "Scales");

            migrationBuilder.DropIndex(
                name: "IX_Sets_ScaleId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "ScaleId",
                table: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "MaxGrade",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Sets_SetId",
                table: "Works",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Users_UserId",
                table: "Works",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
