using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea_Database_Interface.Migrations
{
    /// <inheritdoc />
    public partial class addcomercios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprendedores_Categorías_CategoríaId",
                schema: "IdeaDatabase",
                table: "Emprendedores");

            migrationBuilder.DropIndex(
                name: "IX_Emprendedores_CategoríaId",
                schema: "IdeaDatabase",
                table: "Emprendedores");

            migrationBuilder.DropColumn(
                name: "CategoríaId",
                schema: "IdeaDatabase",
                table: "Emprendedores");

            
            migrationBuilder.CreateTable(
                name: "Comercios",
                schema: "IdeaDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IAE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CódigoFUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Número = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CódigoPostal = table.Column<int>(type: "int", nullable: true),
                    Provincia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Municipio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeléfonoFijo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Móvi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CIF = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comercios", x => x.Id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Comercios",
                schema: "IdeaDatabase");


        }
    }
}
