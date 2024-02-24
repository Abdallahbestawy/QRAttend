using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAttend.Migrations
{
    public partial class Edit_RelationIn_Section_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Sections",
                newName: "SectionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_CourseId",
                table: "Sections",
                newName: "IX_Sections_SectionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_SectionGroup_SectionGroupId",
                table: "Sections",
                column: "SectionGroupId",
                principalTable: "SectionGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_SectionGroup_SectionGroupId",
                table: "Sections");

            migrationBuilder.RenameColumn(
                name: "SectionGroupId",
                table: "Sections",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_SectionGroupId",
                table: "Sections",
                newName: "IX_Sections_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
