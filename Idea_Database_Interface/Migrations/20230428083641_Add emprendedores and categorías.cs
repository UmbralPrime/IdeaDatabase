    using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea_Database_Interface.Migrations
{
    /// <inheritdoc />
    public partial class Addemprendedoresandcategorías : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorías",
                schema: "IdeaDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorías", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emprendedores",
                schema: "IdeaDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teléfono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotivoDeLaConsulto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Incidencias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanViabilidad = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprendedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmprendedoresCategorías",
                schema: "IdeaDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategoría = table.Column<int>(type: "int", nullable: false),
                    IdEmprendedores = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmprendedoresCategorías", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmprendedoresCategorías_Categorías_IdCategoría",
                        column: x => x.IdCategoría,
                        principalSchema: "IdeaDatabase",
                        principalTable: "Categorías",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmprendedoresCategorías_Emprendedores_IdEmprendedores",
                        column: x => x.IdEmprendedores,
                        principalSchema: "IdeaDatabase",
                        principalTable: "Emprendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmprendedoresCategorías_IdCategoría",
                schema: "IdeaDatabase",
                table: "EmprendedoresCategorías",
                column: "IdCategoría");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmprendedoresCategorías",
                schema: "IdeaDatabase");

            migrationBuilder.DropTable(
                name: "Categorías",
                schema: "IdeaDatabase");

            migrationBuilder.DropTable(
                name: "Emprendedores",
                schema: "IdeaDatabase");
        }
    }
}
