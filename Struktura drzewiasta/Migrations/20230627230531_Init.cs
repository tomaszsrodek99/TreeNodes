using Microsoft.EntityFrameworkCore.Migrations;

namespace Struktura_drzewiasta.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreeNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeNodes_TreeNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TreeNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeNodes_ParentId",
                table: "TreeNodes",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreeNodes");
        }
    }
}
