using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMNDC.Preposicionamiento.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Descricion",
                table: "Provincias",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "Descricion",
                table: "Municipios",
                newName: "Descripcion");

            migrationBuilder.AddColumn<int>(
                name: "PosicionamientoId",
                table: "Provincias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PosicionamientoId",
                table: "Municipios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PosicionamientoId",
                table: "Almacens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PosicionamientoId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Posicionamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Lat = table.Column<double>(type: "double", nullable: false),
                    Lon = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posicionamientos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_PosicionamientoId",
                table: "Provincias",
                column: "PosicionamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_PosicionamientoId",
                table: "Municipios",
                column: "PosicionamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Almacens_PosicionamientoId",
                table: "Almacens",
                column: "PosicionamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PosicionamientoId",
                table: "Addresses",
                column: "PosicionamientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Posicionamientos_PosicionamientoId",
                table: "Addresses",
                column: "PosicionamientoId",
                principalTable: "Posicionamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Almacens_Posicionamientos_PosicionamientoId",
                table: "Almacens",
                column: "PosicionamientoId",
                principalTable: "Posicionamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Posicionamientos_PosicionamientoId",
                table: "Municipios",
                column: "PosicionamientoId",
                principalTable: "Posicionamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provincias_Posicionamientos_PosicionamientoId",
                table: "Provincias",
                column: "PosicionamientoId",
                principalTable: "Posicionamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Posicionamientos_PosicionamientoId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Almacens_Posicionamientos_PosicionamientoId",
                table: "Almacens");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Posicionamientos_PosicionamientoId",
                table: "Municipios");

            migrationBuilder.DropForeignKey(
                name: "FK_Provincias_Posicionamientos_PosicionamientoId",
                table: "Provincias");

            migrationBuilder.DropTable(
                name: "Posicionamientos");

            migrationBuilder.DropIndex(
                name: "IX_Provincias_PosicionamientoId",
                table: "Provincias");

            migrationBuilder.DropIndex(
                name: "IX_Municipios_PosicionamientoId",
                table: "Municipios");

            migrationBuilder.DropIndex(
                name: "IX_Almacens_PosicionamientoId",
                table: "Almacens");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_PosicionamientoId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PosicionamientoId",
                table: "Provincias");

            migrationBuilder.DropColumn(
                name: "PosicionamientoId",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "PosicionamientoId",
                table: "Almacens");

            migrationBuilder.DropColumn(
                name: "PosicionamientoId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Provincias",
                newName: "Descricion");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Municipios",
                newName: "Descricion");

            migrationBuilder.AddColumn<long>(
                name: "Lat",
                table: "Addresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Lon",
                table: "Addresses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
