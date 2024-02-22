using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRAttend.Migrations
{
    public partial class Add_AssistantTeacherSections_StudentSections_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssistantTeacherSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantTeacherSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssistantTeacherSections_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssistantTeacherSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSections_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssistantTeacherSections_SectionId",
                table: "AssistantTeacherSections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantTeacherSections_TeacherId",
                table: "AssistantTeacherSections",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSections_SectionId",
                table: "StudentSections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSections_StudentId",
                table: "StudentSections",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssistantTeacherSections");

            migrationBuilder.DropTable(
                name: "StudentSections");
        }
    }
}
