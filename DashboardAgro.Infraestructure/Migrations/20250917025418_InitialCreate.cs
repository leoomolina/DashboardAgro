using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardAgro.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControleImportacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusImportacao = table.Column<int>(type: "int", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantidadeRegistros = table.Column<int>(type: "int", nullable: false),
                    DescricaoStatusImportacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControleImportacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoLavoura = table.Column<int>(type: "int", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regiao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadeFederativa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiglaUF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeUF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRegiao = table.Column<int>(type: "int", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadeFederativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadeFederativa_Regiao_IdRegiao",
                        column: x => x.IdRegiao,
                        principalTable: "Regiao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DadosLavouraPermanente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaDestinadaColheita = table.Column<long>(type: "bigint", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    AreaColhida = table.Column<long>(type: "bigint", nullable: false),
                    QuantidadeProduzida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RendimentoMedioProducao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorProducao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdProducao = table.Column<int>(type: "int", nullable: false),
                    IdUf = table.Column<int>(type: "int", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosLavouraPermanente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosLavouraPermanente_Producao_IdProducao",
                        column: x => x.IdProducao,
                        principalTable: "Producao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DadosLavouraPermanente_UnidadeFederativa_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "UnidadeFederativa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DadosLavouraTemporaria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaPlantada = table.Column<long>(type: "bigint", nullable: false),
                    ImportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    AreaColhida = table.Column<long>(type: "bigint", nullable: false),
                    QuantidadeProduzida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RendimentoMedioProducao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorProducao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdProducao = table.Column<int>(type: "int", nullable: false),
                    IdUf = table.Column<int>(type: "int", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosLavouraTemporaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosLavouraTemporaria_Producao_IdProducao",
                        column: x => x.IdProducao,
                        principalTable: "Producao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DadosLavouraTemporaria_UnidadeFederativa_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "UnidadeFederativa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DadosLavouraPermanente_IdEstado",
                table: "DadosLavouraPermanente",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_DadosLavouraPermanente_IdProducao",
                table: "DadosLavouraPermanente",
                column: "IdProducao");

            migrationBuilder.CreateIndex(
                name: "IX_DadosLavouraTemporaria_IdEstado",
                table: "DadosLavouraTemporaria",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_DadosLavouraTemporaria_IdProducao",
                table: "DadosLavouraTemporaria",
                column: "IdProducao");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadeFederativa_IdRegiao",
                table: "UnidadeFederativa",
                column: "IdRegiao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControleImportacao");

            migrationBuilder.DropTable(
                name: "DadosLavouraPermanente");

            migrationBuilder.DropTable(
                name: "DadosLavouraTemporaria");

            migrationBuilder.DropTable(
                name: "Producao");

            migrationBuilder.DropTable(
                name: "UnidadeFederativa");

            migrationBuilder.DropTable(
                name: "Regiao");
        }
    }
}
