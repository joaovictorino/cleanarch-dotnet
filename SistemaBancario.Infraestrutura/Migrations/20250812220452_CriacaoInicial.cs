using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaBancario.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "CHAR(36)", nullable: false, collation: "ascii_general_ci"),
                    Numero = table.Column<string>(type: "VARCHAR(6)", maxLength: 6, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeCliente = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Saldo = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "CHAR(36)", nullable: false, collation: "ascii_general_ci"),
                    IdContaOrigem = table.Column<Guid>(type: "CHAR(36)", nullable: false, collation: "ascii_general_ci"),
                    IdContaDestino = table.Column<Guid>(type: "CHAR(36)", nullable: false, collation: "ascii_general_ci"),
                    Valor = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    DataHoraTransacao = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CodigoTransacao = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacoes_Contas_IdContaDestino",
                        column: x => x.IdContaDestino,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacoes_Contas_IdContaOrigem",
                        column: x => x.IdContaOrigem,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Conta_Numero",
                table: "Contas",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_Codigo",
                table: "Transacoes",
                column: "CodigoTransacao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_IdContaDestino",
                table: "Transacoes",
                column: "IdContaDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_IdContaOrigem",
                table: "Transacoes",
                column: "IdContaOrigem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Contas");
        }
    }
}
