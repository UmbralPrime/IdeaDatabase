using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea_Database_Interface.Migrations
{
    /// <inheritdoc />
    public partial class Addbonostodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonos",
                schema: "IdeaDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimerApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegunodApellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDeBonos = table.Column<int>(type: "int", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direcction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CódigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Teléfono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarjetaNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NúmeroId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NúmeroId2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonos",
                schema: "IdeaDatabase");
        }
    }
}
