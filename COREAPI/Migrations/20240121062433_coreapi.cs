using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COREAPI.Migrations
{
    public partial class coreapi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buyer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingresos = table.Column<float>(type: "real", nullable: false),
                    Intereses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frustraciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Geografia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campana",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Objetivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Canal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Presupuesto = table.Column<float>(type: "real", nullable: false),
                    Frecuencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    fechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campana", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    flag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KPIXCampana",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KPIID = table.Column<int>(type: "int", nullable: false),
                    CampanaID = table.Column<int>(type: "int", nullable: false),
                    Resultado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIXCampana", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buyer");

            migrationBuilder.DropTable(
                name: "Campana");

            migrationBuilder.DropTable(
                name: "KPI");

            migrationBuilder.DropTable(
                name: "KPIXCampana");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
