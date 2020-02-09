using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class WorkItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetAnswer_SetQuestion_SetQuestionId",
                table: "SetAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_SetQuestion_SetRoles_SetRoleId",
                table: "SetQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_SetQuestion_SetTerms_SetTermId",
                table: "SetQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Users_AuthorId",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Users_ManagerId",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Sets_SetId",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_Users_UserId",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoal_Ideas_WorkId",
                table: "WorkGoal");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoal_Work_WorkId1",
                table: "WorkGoal");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutline_Ideas_WorkId",
                table: "WorkOutline");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutline_Work_WorkId1",
                table: "WorkOutline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkOutline",
                table: "WorkOutline");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkGoal",
                table: "WorkGoal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Work",
                table: "Work");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetQuestion",
                table: "SetQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetAnswer",
                table: "SetAnswer");

            migrationBuilder.DropColumn(
                name: "MaterialCostsBySchool",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "ServicesCostsBySchool",
                table: "Work");

            migrationBuilder.RenameTable(
                name: "WorkOutline",
                newName: "WorkOutlines");

            migrationBuilder.RenameTable(
                name: "WorkGoal",
                newName: "WorkGoals");

            migrationBuilder.RenameTable(
                name: "Work",
                newName: "Works");

            migrationBuilder.RenameTable(
                name: "SetQuestion",
                newName: "SetQuestions");

            migrationBuilder.RenameTable(
                name: "SetAnswer",
                newName: "SetAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_WorkOutline_WorkId1",
                table: "WorkOutlines",
                newName: "IX_WorkOutlines_WorkId1");

            migrationBuilder.RenameIndex(
                name: "IX_WorkOutline_WorkId",
                table: "WorkOutlines",
                newName: "IX_WorkOutlines_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkGoal_WorkId1",
                table: "WorkGoals",
                newName: "IX_WorkGoals_WorkId1");

            migrationBuilder.RenameIndex(
                name: "IX_WorkGoal_WorkId",
                table: "WorkGoals",
                newName: "IX_WorkGoals_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_Work_UserId",
                table: "Works",
                newName: "IX_Works_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Work_SetId",
                table: "Works",
                newName: "IX_Works_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_Work_ManagerId",
                table: "Works",
                newName: "IX_Works_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Work_AuthorId",
                table: "Works",
                newName: "IX_Works_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_SetQuestion_SetTermId",
                table: "SetQuestions",
                newName: "IX_SetQuestions_SetTermId");

            migrationBuilder.RenameIndex(
                name: "IX_SetQuestion_SetRoleId",
                table: "SetQuestions",
                newName: "IX_SetQuestions_SetRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_SetAnswer_SetQuestionId",
                table: "SetAnswers",
                newName: "IX_SetAnswers_SetQuestionId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Ideas",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "WorkId1",
                table: "WorkOutlines",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkId1",
                table: "WorkGoals",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Works",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagerId",
                table: "Works",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorId",
                table: "Works",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "MaterialCostsProvidedBySchool",
                table: "Works",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServicesCostsProvidedBySchool",
                table: "Works",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkOutlines",
                table: "WorkOutlines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkGoals",
                table: "WorkGoals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Works",
                table: "Works",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetQuestions",
                table: "SetQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetAnswers",
                table: "SetAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SetAnswers_SetQuestions_SetQuestionId",
                table: "SetAnswers",
                column: "SetQuestionId",
                principalTable: "SetQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetQuestions_SetRoles_SetRoleId",
                table: "SetQuestions",
                column: "SetRoleId",
                principalTable: "SetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetQuestions_SetTerms_SetTermId",
                table: "SetQuestions",
                column: "SetTermId",
                principalTable: "SetTerms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoals_Ideas_WorkId",
                table: "WorkGoals",
                column: "WorkId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoals_Works_WorkId1",
                table: "WorkGoals",
                column: "WorkId1",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutlines_Ideas_WorkId",
                table: "WorkOutlines",
                column: "WorkId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutlines_Works_WorkId1",
                table: "WorkOutlines",
                column: "WorkId1",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Users_AuthorId",
                table: "Works",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Users_ManagerId",
                table: "Works",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetAnswers_SetQuestions_SetQuestionId",
                table: "SetAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SetQuestions_SetRoles_SetRoleId",
                table: "SetQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SetQuestions_SetTerms_SetTermId",
                table: "SetQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoals_Ideas_WorkId",
                table: "WorkGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoals_Works_WorkId1",
                table: "WorkGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutlines_Ideas_WorkId",
                table: "WorkOutlines");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutlines_Works_WorkId1",
                table: "WorkOutlines");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Users_AuthorId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Users_ManagerId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Sets_SetId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Users_UserId",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Works",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkOutlines",
                table: "WorkOutlines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkGoals",
                table: "WorkGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetQuestions",
                table: "SetQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SetAnswers",
                table: "SetAnswers");

            migrationBuilder.DropColumn(
                name: "MaterialCostsProvidedBySchool",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "ServicesCostsProvidedBySchool",
                table: "Works");

            migrationBuilder.RenameTable(
                name: "Works",
                newName: "Work");

            migrationBuilder.RenameTable(
                name: "WorkOutlines",
                newName: "WorkOutline");

            migrationBuilder.RenameTable(
                name: "WorkGoals",
                newName: "WorkGoal");

            migrationBuilder.RenameTable(
                name: "SetQuestions",
                newName: "SetQuestion");

            migrationBuilder.RenameTable(
                name: "SetAnswers",
                newName: "SetAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_Works_UserId",
                table: "Work",
                newName: "IX_Work_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_SetId",
                table: "Work",
                newName: "IX_Work_SetId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_ManagerId",
                table: "Work",
                newName: "IX_Work_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_AuthorId",
                table: "Work",
                newName: "IX_Work_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkOutlines_WorkId1",
                table: "WorkOutline",
                newName: "IX_WorkOutline_WorkId1");

            migrationBuilder.RenameIndex(
                name: "IX_WorkOutlines_WorkId",
                table: "WorkOutline",
                newName: "IX_WorkOutline_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkGoals_WorkId1",
                table: "WorkGoal",
                newName: "IX_WorkGoal_WorkId1");

            migrationBuilder.RenameIndex(
                name: "IX_WorkGoals_WorkId",
                table: "WorkGoal",
                newName: "IX_WorkGoal_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_SetQuestions_SetTermId",
                table: "SetQuestion",
                newName: "IX_SetQuestion_SetTermId");

            migrationBuilder.RenameIndex(
                name: "IX_SetQuestions_SetRoleId",
                table: "SetQuestion",
                newName: "IX_SetQuestion_SetRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_SetAnswers_SetQuestionId",
                table: "SetAnswer",
                newName: "IX_SetAnswer_SetQuestionId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ideas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Work",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Work",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Work",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "MaterialCostsBySchool",
                table: "Work",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServicesCostsBySchool",
                table: "Work",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "WorkId1",
                table: "WorkOutline",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkId1",
                table: "WorkGoal",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Work",
                table: "Work",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkOutline",
                table: "WorkOutline",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkGoal",
                table: "WorkGoal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetQuestion",
                table: "SetQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SetAnswer",
                table: "SetAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SetAnswer_SetQuestion_SetQuestionId",
                table: "SetAnswer",
                column: "SetQuestionId",
                principalTable: "SetQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetQuestion_SetRoles_SetRoleId",
                table: "SetQuestion",
                column: "SetRoleId",
                principalTable: "SetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetQuestion_SetTerms_SetTermId",
                table: "SetQuestion",
                column: "SetTermId",
                principalTable: "SetTerms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Users_AuthorId",
                table: "Work",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Users_ManagerId",
                table: "Work",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Sets_SetId",
                table: "Work",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Users_UserId",
                table: "Work",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoal_Ideas_WorkId",
                table: "WorkGoal",
                column: "WorkId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoal_Work_WorkId1",
                table: "WorkGoal",
                column: "WorkId1",
                principalTable: "Work",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutline_Ideas_WorkId",
                table: "WorkOutline",
                column: "WorkId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutline_Work_WorkId1",
                table: "WorkOutline",
                column: "WorkId1",
                principalTable: "Work",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
