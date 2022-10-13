using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoParaleloAPI.Migrations
{
    public partial class updateAgencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Agencias",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Agencias");
        }
    }
}
