using Microsoft.EntityFrameworkCore.Migrations;

namespace UPBEats.Migrations
{
    public partial class AdiccionVendedorFavorito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendedorFavorito",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompradorId = table.Column<int>(nullable: false),
                    VendedorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendedorFavorito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendedorFavorito_Usuario_CompradorId",
                        column: x => x.CompradorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendedorFavorito_Usuario_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendedorFavorito_CompradorId",
                table: "VendedorFavorito",
                column: "CompradorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendedorFavorito_VendedorId",
                table: "VendedorFavorito",
                column: "VendedorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendedorFavorito");
        }
    }
}
