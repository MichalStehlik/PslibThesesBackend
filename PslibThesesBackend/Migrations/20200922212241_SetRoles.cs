using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class SetRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetRoles_Sets_SetId",
                table: "SetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_SetTerms_Sets_SetId",
                table: "SetTerms");

            migrationBuilder.DropColumn(
                name: "RequiredForAdvancement",
                table: "SetRoles");

            migrationBuilder.DropColumn(
                name: "RequiredForPrint",
                table: "SetRoles");

            migrationBuilder.DropColumn(
                name: "ShowsOnApplication",
                table: "SetRoles");

            migrationBuilder.AddColumn<bool>(
                name: "Manager",
                table: "SetRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrintedInApplication",
                table: "SetRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrintedInReview",
                table: "SetRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SetRoles_Sets_SetId",
                table: "SetRoles",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SetTerms_Sets_SetId",
                table: "SetTerms",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetRoles_Sets_SetId",
                table: "SetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_SetTerms_Sets_SetId",
                table: "SetTerms");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "SetRoles");

            migrationBuilder.DropColumn(
                name: "PrintedInApplication",
                table: "SetRoles");

            migrationBuilder.DropColumn(
                name: "PrintedInReview",
                table: "SetRoles");

            migrationBuilder.AddColumn<bool>(
                name: "RequiredForAdvancement",
                table: "SetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiredForPrint",
                table: "SetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowsOnApplication",
                table: "SetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SetRoles_Sets_SetId",
                table: "SetRoles",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SetTerms_Sets_SetId",
                table: "SetTerms",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
