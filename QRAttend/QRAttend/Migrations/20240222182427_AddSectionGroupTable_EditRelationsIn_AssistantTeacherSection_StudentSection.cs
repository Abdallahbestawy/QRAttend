using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAttend.Migrations
{
    public partial class AddSectionGroupTable_EditRelationsIn_AssistantTeacherSection_StudentSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssistantTeacherSections_Sections_SectionId",
                table: "AssistantTeacherSections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSections_Sections_SectionId",
                table: "StudentSections");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "StudentSections",
                newName: "SectionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSections_SectionId",
                table: "StudentSections",
                newName: "IX_StudentSections_SectionGroupId");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "AssistantTeacherSections",
                newName: "SectionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AssistantTeacherSections_SectionId",
                table: "AssistantTeacherSections",
                newName: "IX_AssistantTeacherSections_SectionGroupId");

            migrationBuilder.CreateTable(
                name: "SectionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionGroup_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionGroup_CourseId",
                table: "SectionGroup",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssistantTeacherSections_SectionGroup_SectionGroupId",
                table: "AssistantTeacherSections",
                column: "SectionGroupId",
                principalTable: "SectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSections_SectionGroup_SectionGroupId",
                table: "StudentSections",
                column: "SectionGroupId",
                principalTable: "SectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssistantTeacherSections_SectionGroup_SectionGroupId",
                table: "AssistantTeacherSections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSections_SectionGroup_SectionGroupId",
                table: "StudentSections");

            migrationBuilder.DropTable(
                name: "SectionGroup");

            migrationBuilder.RenameColumn(
                name: "SectionGroupId",
                table: "StudentSections",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSections_SectionGroupId",
                table: "StudentSections",
                newName: "IX_StudentSections_SectionId");

            migrationBuilder.RenameColumn(
                name: "SectionGroupId",
                table: "AssistantTeacherSections",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AssistantTeacherSections_SectionGroupId",
                table: "AssistantTeacherSections",
                newName: "IX_AssistantTeacherSections_SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssistantTeacherSections_Sections_SectionId",
                table: "AssistantTeacherSections",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSections_Sections_SectionId",
                table: "StudentSections",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
