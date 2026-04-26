using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMNDC.Preposicionamiento.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndUserOrganismo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganismoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganismoId",
                table: "AspNetUsers",
                column: "OrganismoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organismos_OrganismoId",
                table: "AspNetUsers",
                column: "OrganismoId",
                principalTable: "Organismos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organismos_OrganismoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrganismoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganismoId",
                table: "AspNetUsers");
        }
    }
}
