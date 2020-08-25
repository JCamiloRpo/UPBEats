using Microsoft.EntityFrameworkCore.Migrations;

namespace UPBEats.Migrations
{
    public partial class AdiccionProductoFavorito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductoFavorito",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(nullable: false),
                    ProductoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoFavorito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoFavorito_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoFavorito_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoFavorito_ProductoId",
                table: "ProductoFavorito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoFavorito_UsuarioId",
                table: "ProductoFavorito",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoFavorito");
        }
    }
}
