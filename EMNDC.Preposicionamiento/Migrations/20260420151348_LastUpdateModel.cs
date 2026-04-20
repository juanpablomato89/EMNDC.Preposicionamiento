using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMNDC.Preposicionamiento.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Municipios_CityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Pais_CountryId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provincias_StateId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Almacens_Addresses_AddressId",
                table: "Almacens");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Organismos_OrganismoId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Provincias_Pais_PaisId",
                table: "Provincias");

            migrationBuilder.DropTable(
                name: "AlmacenProducto");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pais",
                table: "Pais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Almacens",
                table: "Almacens");

            migrationBuilder.DropColumn(
                name: "Posicionamiento_Id",
                table: "Provincias");

            migrationBuilder.DropColumn(
                name: "FechaIngreso",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "Organismos");

            migrationBuilder.DropColumn(
                name: "Posicionamiento_Id",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Posicionamiento_Id",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Posicionamiento_Id",
                table: "Almacens");

            migrationBuilder.RenameTable(
                name: "Pais",
                newName: "Paises");

            migrationBuilder.RenameTable(
                name: "Almacens",
                newName: "Almacenes");

            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Provincias",
                newName: "Posicionamiento_Lon");

            migrationBuilder.RenameColumn(
                name: "Latitud",
                table: "Provincias",
                newName: "Posicionamiento_Lat");

            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Municipios",
                newName: "Posicionamiento_Lon");

            migrationBuilder.RenameColumn(
                name: "Latitud",
                table: "Municipios",
                newName: "Posicionamiento_Lat");

            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Addresses",
                newName: "Posicionamiento_Lon");

            migrationBuilder.RenameColumn(
                name: "Latitud",
                table: "Addresses",
                newName: "Posicionamiento_Lat");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Addresses",
                newName: "MunicipioId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_StateId",
                table: "Addresses",
                newName: "IX_Addresses_MunicipioId");

            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Almacenes",
                newName: "Posicionamiento_Lon");

            migrationBuilder.RenameColumn(
                name: "Latitud",
                table: "Almacenes",
                newName: "Posicionamiento_Lat");

            migrationBuilder.RenameIndex(
                name: "IX_Almacens_AddressId",
                table: "Almacenes",
                newName: "IX_Almacenes_AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Token",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "PaisId",
                table: "Provincias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Provincias",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoProvincia",
                table: "Provincias",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Productos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Creado",
                table: "Organismos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Organismos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Modificado",
                table: "Organismos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Municipios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoMunicipio",
                table: "Municipios",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Paises",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Almacenes",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paises",
                table: "Paises",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Almacenes",
                table: "Almacenes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StocksAlmacen",
                columns: table => new
                {
                    AlmacenId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creado = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modificado = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocksAlmacen", x => new { x.AlmacenId, x.ProductoId });
                    table.ForeignKey(
                        name: "FK_StocksAlmacen_Almacenes_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "Almacenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocksAlmacen_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Token_RefreshToken",
                table: "Token",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_CodigoProvincia",
                table: "Provincias",
                column: "CodigoProvincia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Descripcion",
                table: "Productos",
                column: "Descripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_CodigoMunicipio",
                table: "Municipios",
                column: "CodigoMunicipio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paises_Descripcion",
                table: "Paises",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Almacenes_Descripcion",
                table: "Almacenes",
                column: "Descripcion");

            migrationBuilder.CreateIndex(
                name: "IX_StocksAlmacen_ProductoId",
                table: "StocksAlmacen",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Municipios_MunicipioId",
                table: "Addresses",
                column: "MunicipioId",
                principalTable: "Municipios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Almacenes_Addresses_AddressId",
                table: "Almacenes",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios",
                column: "ProvinciaId",
                principalTable: "Provincias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Organismos_OrganismoId",
                table: "Productos",
                column: "OrganismoId",
                principalTable: "Organismos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Provincias_Paises_PaisId",
                table: "Provincias",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Municipios_MunicipioId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Almacenes_Addresses_AddressId",
                table: "Almacenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Organismos_OrganismoId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Provincias_Paises_PaisId",
                table: "Provincias");

            migrationBuilder.DropTable(
                name: "StocksAlmacen");

            migrationBuilder.DropIndex(
                name: "IX_Token_RefreshToken",
                table: "Token");

            migrationBuilder.DropIndex(
                name: "IX_Provincias_CodigoProvincia",
                table: "Provincias");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Descripcion",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Municipios_CodigoMunicipio",
                table: "Municipios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Paises",
                table: "Paises");

            migrationBuilder.DropIndex(
                name: "IX_Paises_Descripcion",
                table: "Paises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Almacenes",
                table: "Almacenes");

            migrationBuilder.DropIndex(
                name: "IX_Almacenes_Descripcion",
                table: "Almacenes");

            migrationBuilder.DropColumn(
                name: "Creado",
                table: "Organismos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Organismos");

            migrationBuilder.DropColumn(
                name: "Modificado",
                table: "Organismos");

            migrationBuilder.RenameTable(
                name: "Paises",
                newName: "Pais");

            migrationBuilder.RenameTable(
                name: "Almacenes",
                newName: "Almacens");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lon",
                table: "Provincias",
                newName: "Longitud");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lat",
                table: "Provincias",
                newName: "Latitud");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lon",
                table: "Municipios",
                newName: "Longitud");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lat",
                table: "Municipios",
                newName: "Latitud");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lon",
                table: "Addresses",
                newName: "Longitud");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lat",
                table: "Addresses",
                newName: "Latitud");

            migrationBuilder.RenameColumn(
                name: "MunicipioId",
                table: "Addresses",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_MunicipioId",
                table: "Addresses",
                newName: "IX_Addresses_StateId");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lon",
                table: "Almacens",
                newName: "Longitud");

            migrationBuilder.RenameColumn(
                name: "Posicionamiento_Lat",
                table: "Almacens",
                newName: "Latitud");

            migrationBuilder.RenameIndex(
                name: "IX_Almacenes_AddressId",
                table: "Almacens",
                newName: "IX_Almacens_AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Token",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "PaisId",
                table: "Provincias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Provincias",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoProvincia",
                table: "Provincias",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Posicionamiento_Id",
                table: "Provincias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UnidadMedida",
                table: "Productos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaIngreso",
                table: "Productos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "Organismos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Municipios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoMunicipio",
                table: "Municipios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Posicionamiento_Id",
                table: "Municipios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Posicionamiento_Id",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Pais",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Almacens",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Posicionamiento_Id",
                table: "Almacens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pais",
                table: "Pais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Almacens",
                table: "Almacens",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AlmacenProducto",
                columns: table => new
                {
                    AlmacensId = table.Column<int>(type: "int", nullable: false),
                    ProductosId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlmacenProducto", x => new { x.AlmacensId, x.ProductosId });
                    table.ForeignKey(
                        name: "FK_AlmacenProducto_Almacens_AlmacensId",
                        column: x => x.AlmacensId,
                        principalTable: "Almacens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlmacenProducto_Productos_ProductosId",
                        column: x => x.ProductosId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlmacenProducto_ProductosId",
                table: "AlmacenProducto",
                column: "ProductosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Municipios_CityId",
                table: "Addresses",
                column: "CityId",
                principalTable: "Municipios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Pais_CountryId",
                table: "Addresses",
                column: "CountryId",
                principalTable: "Pais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provincias_StateId",
                table: "Addresses",
                column: "StateId",
                principalTable: "Provincias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Almacens_Addresses_AddressId",
                table: "Almacens",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Provincias_ProvinciaId",
                table: "Municipios",
                column: "ProvinciaId",
                principalTable: "Provincias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Organismos_OrganismoId",
                table: "Productos",
                column: "OrganismoId",
                principalTable: "Organismos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provincias_Pais_PaisId",
                table: "Provincias",
                column: "PaisId",
                principalTable: "Pais",
                principalColumn: "Id");
        }
    }
}
