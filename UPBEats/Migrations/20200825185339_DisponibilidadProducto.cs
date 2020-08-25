using Microsoft.EntityFrameworkCore.Migrations;

namespace UPBEats.Migrations
{
    public partial class DisponibilidadProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Producto",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Producto");
        }
    }
}
