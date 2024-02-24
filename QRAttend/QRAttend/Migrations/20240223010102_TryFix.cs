using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAttend.Migrations
{
    public partial class TryFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssistantTeacherSections_SectionGroup_SectionGroupId",
                table: "AssistantTeacherSections");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionGroup_Courses_CourseId",
                table: "SectionGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_SectionGroup_SectionGroupId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSections_SectionGroup_SectionGroupId",
                table: "StudentSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionGroup",
                table: "SectionGroup");

            migrationBuilder.RenameTable(
                name: "SectionGroup",
                newName: "SectionGroups");

            migrationBuilder.RenameIndex(
                name: "IX_SectionGroup_CourseId",
                table: "SectionGroups",
                newName: "IX_SectionGroups_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionGroups",
                table: "SectionGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssistantTeacherSections_SectionGroups_SectionGroupId",
                table: "AssistantTeacherSections",
                column: "SectionGroupId",
                principalTable: "SectionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionGroups_Courses_CourseId",
                table: "SectionGroups",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_SectionGroups_SectionGroupId",
                table: "Sections",
                column: "SectionGroupId",
                principalTable: "SectionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSections_SectionGroups_SectionGroupId",
                table: "StudentSections",
                column: "SectionGroupId",
                principalTable: "SectionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssistantTeacherSections_SectionGroups_SectionGroupId",
                table: "AssistantTeacherSections");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionGroups_Courses_CourseId",
                table: "SectionGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_SectionGroups_SectionGroupId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSections_SectionGroups_SectionGroupId",
                table: "StudentSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionGroups",
                table: "SectionGroups");

            migrationBuilder.RenameTable(
                name: "SectionGroups",
                newName: "SectionGroup");

            migrationBuilder.RenameIndex(
                name: "IX_SectionGroups_CourseId",
                table: "SectionGroup",
                newName: "IX_SectionGroup_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionGroup",
                table: "SectionGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssistantTeacherSections_SectionGroup_SectionGroupId",
                table: "AssistantTeacherSections",
                column: "SectionGroupId",
                principalTable: "SectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionGroup_Courses_CourseId",
                table: "SectionGroup",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_SectionGroup_SectionGroupId",
                table: "Sections",
                column: "SectionGroupId",
                principalTable: "SectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSections_SectionGroup_SectionGroupId",
                table: "StudentSections",
                column: "SectionGroupId",
                principalTable: "SectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
