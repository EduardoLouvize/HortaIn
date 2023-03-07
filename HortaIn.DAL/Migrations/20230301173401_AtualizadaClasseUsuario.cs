using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HortaIn.DAL.Migrations
{
    public partial class AtualizadaClasseUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomeCompleto",
                table: "Usuarios",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Usuarios",
                newName: "NomeCompleto");
        }
    }
}
