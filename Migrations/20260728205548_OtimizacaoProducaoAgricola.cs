using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrutosDeGoias.Api.Migrations
{
    /// <inheritdoc />
    public partial class OtimizacaoProducaoAgricola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Fruta",
                table: "Producoes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "Producoes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProducaoAgricola_Cidade_Fruta",
                table: "Producoes",
                columns: new[] { "Cidade", "Fruta" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProducaoAgricola_Cidade_Fruta",
                table: "Producoes");

            migrationBuilder.AlterColumn<string>(
                name: "Fruta",
                table: "Producoes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "Producoes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
