using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea_Database_Interface.Migrations
{
    /// <inheritdoc />
    public partial class Addedrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoríaEmprendedores",
                schema: "IdeaDatabase",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    EmprendedoresesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoríaEmprendedores", x => new { x.CategoriasId, x.EmprendedoresesId });
                    table.ForeignKey(
                        name: "FK_CategoríaEmprendedores_Categorías_CategoriasId",
                        column: x => x.CategoriasId,
                        principalSchema: "IdeaDatabase",
                        principalTable: "Categorías",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoríaEmprendedores_Emprendedores_EmprendedoresesId",
                        column: x => x.EmprendedoresesId,
                        principalSchema: "IdeaDatabase",
                        principalTable: "Emprendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoríaEmprendedores_EmprendedoresesId",
                schema: "IdeaDatabase",
                table: "CategoríaEmprendedores",
                column: "EmprendedoresesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoríaEmprendedores",
                schema: "IdeaDatabase");
        }
    }
}
