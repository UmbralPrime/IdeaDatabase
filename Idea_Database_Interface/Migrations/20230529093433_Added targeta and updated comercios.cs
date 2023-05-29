using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Idea_Database_Interface.Migrations
{
    /// <inheritdoc />
    public partial class Addedtargetaandupdatedcomercios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CIF",
                schema: "IdeaDatabase",
                table: "Comercios");

            migrationBuilder.DropColumn(
                name: "Calle",
                schema: "IdeaDatabase",
                table: "Comercios");

            migrationBuilder.DropColumn(
                name: "Contacto",
                schema: "IdeaDatabase",
                table: "Comercios");

            migrationBuilder.DropColumn(
                name: "CódigoPostal",
                schema: "IdeaDatabase",
                table: "Comercios");

            migrationBuilder.DropColumn(
                name: "Localidad",
                schema: "IdeaDatabase",
                table: "Comercios");

            migrationBuilder.DropColumn(
                name: "Municipio",
                schema: "IdeaDatabase",
                table: "Comercios");

            migrationBuilder.RenameColumn(
                name: "Provincia",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "TeléfonoMóvil");

            migrationBuilder.RenameColumn(
                name: "Número",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Correo");

            migrationBuilder.RenameColumn(
                name: "NombreComercial",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "TipoDeVia");

            migrationBuilder.RenameColumn(
                name: "Móvi",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Dirección");

            migrationBuilder.AlterColumn<string>(
                name: "CódigoFUC",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Localizador",
                schema: "IdeaDatabase",
                table: "Bonos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Localidad",
                schema: "IdeaDatabase",
                table: "Bonos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoDeVia",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "NombreComercial");

            migrationBuilder.RenameColumn(
                name: "TeléfonoMóvil",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Provincia");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Móvi");

            migrationBuilder.RenameColumn(
                name: "Dirección",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Correo",
                schema: "IdeaDatabase",
                table: "Comercios",
                newName: "Número");

            migrationBuilder.AlterColumn<string>(
                name: "CódigoFUC",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CIF",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Calle",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contacto",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CódigoPostal",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidad",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                schema: "IdeaDatabase",
                table: "Comercios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Localizador",
                schema: "IdeaDatabase",
                table: "Bonos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Localidad",
                schema: "IdeaDatabase",
                table: "Bonos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
