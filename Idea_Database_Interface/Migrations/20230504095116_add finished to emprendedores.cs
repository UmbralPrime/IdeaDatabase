using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea_Database_Interface.Migrations
{
    /// <inheritdoc />
    public partial class addfinishedtoemprendedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Terminado",
                schema: "IdeaDatabase",
                table: "Emprendedores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Terminado",
                schema: "IdeaDatabase",
                table: "Emprendedores");
        }
    }
}
