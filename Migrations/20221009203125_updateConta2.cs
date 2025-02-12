﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoParaleloAPI.Migrations
{
    public partial class updateConta2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Conta_ContaId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Conta");

            migrationBuilder.DropTable(
                name: "Agencia");

            migrationBuilder.DropTable(
                name: "TipoDeConta");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ContaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ContaId",
                table: "Usuarios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContaId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Agencia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDeConta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDeConta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgenciaId = table.Column<long>(type: "bigint", nullable: true),
                    TipoId = table.Column<long>(type: "bigint", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conta_Agencia_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "Agencia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conta_TipoDeConta_TipoId",
                        column: x => x.TipoId,
                        principalTable: "TipoDeConta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ContaId",
                table: "Usuarios",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_Conta_AgenciaId",
                table: "Conta",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Conta_TipoId",
                table: "Conta",
                column: "TipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Conta_ContaId",
                table: "Usuarios",
                column: "ContaId",
                principalTable: "Conta",
                principalColumn: "Id");
        }
    }
}
