using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMNDC.Preposicionamiento.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelMuncipioProvincia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios");

            migrationBuilder.AddColumn<string>(
                name: "CodigoProvincia",
                table: "Provincias",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinciaId",
                table: "Municipios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoMunicipio",
                table: "Municipios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios",
                column: "ProvinciaId",
                principalTable: "Provincias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "CodigoProvincia",
                table: "Provincias");

            migrationBuilder.DropColumn(
                name: "CodigoMunicipio",
                table: "Municipios");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinciaId",
                table: "Municipios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios",
                column: "ProvinciaId",
                principalTable: "Provincias",
                principalColumn: "Id");
        }
    }
}
