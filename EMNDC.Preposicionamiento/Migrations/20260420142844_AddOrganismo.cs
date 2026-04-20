using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMNDC.Preposicionamiento.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganismo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Organismo_OrganismoId",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organismo",
                table: "Organismo");

            migrationBuilder.RenameTable(
                name: "Organismo",
                newName: "Organismos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organismos",
                table: "Organismos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Organismos_OrganismoId",
                table: "Productos",
                column: "OrganismoId",
                principalTable: "Organismos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Organismos_OrganismoId",
                table: "Productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organismos",
                table: "Organismos");

            migrationBuilder.RenameTable(
                name: "Organismos",
                newName: "Organismo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organismo",
                table: "Organismo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Organismo_OrganismoId",
                table: "Productos",
                column: "OrganismoId",
                principalTable: "Organismo",
                principalColumn: "Id");
        }
    }
}
