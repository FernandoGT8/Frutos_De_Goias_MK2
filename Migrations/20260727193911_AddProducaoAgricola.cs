using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrutosDeGoias.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProducaoAgricola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Producoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fruta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantidadeToneladas = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producoes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producoes");
        }
    }
}
