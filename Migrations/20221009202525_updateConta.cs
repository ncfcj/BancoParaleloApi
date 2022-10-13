using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoParaleloAPI.Migrations
{
    public partial class updateConta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Conta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Numero",
                table: "Conta",
                type: "bigint",
                nullable: true);
        }
    }
}
