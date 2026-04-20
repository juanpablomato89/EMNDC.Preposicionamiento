using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMNDC.Preposicionamiento.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Organismo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ModifieddAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Iso",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "PosicionamientoId",
                table: "Provincias",
                newName: "Posicionamiento_Id");

            migrationBuilder.RenameColumn(
                name: "UnidadMedidas",
                table: "Productos",
                newName: "UnidadMedida");

            migrationBuilder.RenameColumn(
                name: "Almacenado",
                table: "Productos",
                newName: "FechaIngreso");

            migrationBuilder.RenameColumn(
                name: "Descricion",
                table: "Pais",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "PosicionamientoId",
                table: "Municipios",
                newName: "Posicionamiento_Id");

            migrationBuilder.RenameColumn(
                name: "PosicionamientoId",
                table: "Almacens",
                newName: "Posicionamiento_Id");

            migrationBuilder.RenameColumn(
                name: "PosicionamientoId",
                table: "Addresses",
                newName: "Posicionamiento_Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Token",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Provincias",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Provincias",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "OrganismoId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Municipios",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Municipios",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Almacens",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Almacens",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Addresses",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Addresses",
                type: "double",
                precision: 10,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Organismo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descriptions = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organismo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_OrganismoId",
                table: "Productos",
                column: "OrganismoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Organismo_OrganismoId",
                table: "Productos",
                column: "OrganismoId",
                principalTable: "Organismo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Organismo_OrganismoId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "Organismo");

            migrationBuilder.DropIndex(
                name: "IX_Productos_OrganismoId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Token");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Provincias");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Provincias");

            migrationBuilder.DropColumn(
                name: "OrganismoId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Almacens");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Almacens");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Id",
                table: "Provincias",
                newName: "PosicionamientoId");

            migrationBuilder.RenameColumn(
                name: "UnidadMedida",
                table: "Productos",
                newName: "UnidadMedidas");

            migrationBuilder.RenameColumn(
                name: "FechaIngreso",
                table: "Productos",
                newName: "Almacenado");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Pais",
                newName: "Descricion");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Id",
                table: "Municipios",
                newName: "PosicionamientoId");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Id",
                table: "Almacens",
                newName: "PosicionamientoId");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Id",
                table: "Addresses",
                newName: "PosicionamientoId");

            migrationBuilder.AddColumn<string>(
                name: "Organismo",
                table: "Productos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifieddAt",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Iso",
                table: "Addresses",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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
    }
}
