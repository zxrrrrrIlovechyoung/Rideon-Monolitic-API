using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rideon.Migrations
{
    /// <inheritdoc />
    public partial class CorregirColumnas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatMoto_UsuUsuario_UsuUsuarioidUsuario",
                table: "CatMoto");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuMantenimiento_CatMantenimiento_CatMantenimientoidCatMantenimiento",
                table: "UsuMantenimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuMantenimiento_CatMoto_idMotoUsuario",
                table: "UsuMantenimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuUsuario_UsuCatSexo_idSexo",
                table: "UsuUsuario");

            migrationBuilder.DropIndex(
                name: "IX_UsuMantenimiento_CatMantenimientoidCatMantenimiento",
                table: "UsuMantenimiento");

            migrationBuilder.DropIndex(
                name: "IX_CatMoto_UsuUsuarioidUsuario",
                table: "CatMoto");

            migrationBuilder.DropColumn(
                name: "CatMantenimientoidCatMantenimiento",
                table: "UsuMantenimiento");

            migrationBuilder.DropColumn(
                name: "UsuUsuarioidUsuario",
                table: "CatMoto");

            migrationBuilder.DropColumn(
                name: "strMarca",
                table: "CatMoto");

            migrationBuilder.DropColumn(
                name: "strTipoDeMoto",
                table: "CatMoto");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "CatMantenimiento");

            migrationBuilder.RenameColumn(
                name: "idUsuario",
                table: "CatMoto",
                newName: "idTipoMoto");

            migrationBuilder.RenameColumn(
                name: "idMotoUsuario",
                table: "CatMoto",
                newName: "idMoto");

            migrationBuilder.RenameColumn(
                name: "idMantenimiento",
                table: "CatMantenimiento",
                newName: "idCatMantenimiento");

            migrationBuilder.AlterColumn<string>(
                name: "strTokenRecuperacion",
                table: "UsuUsuario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strTelefono",
                table: "UsuUsuario",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strNombre",
                table: "UsuUsuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strFotoPerfil",
                table: "UsuUsuario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strCorreo",
                table: "UsuUsuario",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "strContrasena",
                table: "UsuUsuario",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "strApellidoPaterno",
                table: "UsuUsuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strApellidoMaterno",
                table: "UsuUsuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idSexo",
                table: "UsuUsuario",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strNotas",
                table: "UsuMantenimiento",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strNm",
                table: "CatMoto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strModelo",
                table: "CatMoto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strHp",
                table: "CatMoto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strDescripcion",
                table: "CatMoto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strCilindraje",
                table: "CatMoto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "imgFoto",
                table: "CatMoto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idMarca",
                table: "CatMoto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "strDescripcion",
                table: "CatMantenimiento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CatMarcaMoto",
                columns: table => new
                {
                    idMarca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strMarca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatMarcaMoto", x => x.idMarca);
                });

            migrationBuilder.CreateTable(
                name: "CatTipoMoto",
                columns: table => new
                {
                    idTipoMoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strTipoMoto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatTipoMoto", x => x.idTipoMoto);
                });

            migrationBuilder.CreateTable(
                name: "UsuUsuarioMoto",
                columns: table => new
                {
                    idUsuarioMoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    idMoto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuUsuarioMoto", x => x.idUsuarioMoto);
                    table.ForeignKey(
                        name: "FK_UsuUsuarioMoto_CatMoto_idMoto",
                        column: x => x.idMoto,
                        principalTable: "CatMoto",
                        principalColumn: "idMoto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuUsuarioMoto_UsuUsuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "UsuUsuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatMoto_idMarca",
                table: "CatMoto",
                column: "idMarca");

            migrationBuilder.CreateIndex(
                name: "IX_CatMoto_idTipoMoto",
                table: "CatMoto",
                column: "idTipoMoto");

            migrationBuilder.CreateIndex(
                name: "IX_CatMantenimiento_MotoCatalogoId",
                table: "CatMantenimiento",
                column: "MotoCatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuUsuarioMoto_idMoto",
                table: "UsuUsuarioMoto",
                column: "idMoto");

            migrationBuilder.CreateIndex(
                name: "IX_UsuUsuarioMoto_idUsuario",
                table: "UsuUsuarioMoto",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_CatMantenimiento_CatMoto_MotoCatalogoId",
                table: "CatMantenimiento",
                column: "MotoCatalogoId",
                principalTable: "CatMoto",
                principalColumn: "idMoto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatMoto_CatMarcaMoto_idMarca",
                table: "CatMoto",
                column: "idMarca",
                principalTable: "CatMarcaMoto",
                principalColumn: "idMarca",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatMoto_CatTipoMoto_idTipoMoto",
                table: "CatMoto",
                column: "idTipoMoto",
                principalTable: "CatTipoMoto",
                principalColumn: "idTipoMoto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuMantenimiento_UsuUsuarioMoto_idMotoUsuario",
                table: "UsuMantenimiento",
                column: "idMotoUsuario",
                principalTable: "UsuUsuarioMoto",
                principalColumn: "idUsuarioMoto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuUsuario_UsuCatSexo_idSexo",
                table: "UsuUsuario",
                column: "idSexo",
                principalTable: "UsuCatSexo",
                principalColumn: "idSexo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatMantenimiento_CatMoto_MotoCatalogoId",
                table: "CatMantenimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_CatMoto_CatMarcaMoto_idMarca",
                table: "CatMoto");

            migrationBuilder.DropForeignKey(
                name: "FK_CatMoto_CatTipoMoto_idTipoMoto",
                table: "CatMoto");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuMantenimiento_UsuUsuarioMoto_idMotoUsuario",
                table: "UsuMantenimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuUsuario_UsuCatSexo_idSexo",
                table: "UsuUsuario");

            migrationBuilder.DropTable(
                name: "CatMarcaMoto");

            migrationBuilder.DropTable(
                name: "CatTipoMoto");

            migrationBuilder.DropTable(
                name: "UsuUsuarioMoto");

            migrationBuilder.DropIndex(
                name: "IX_CatMoto_idMarca",
                table: "CatMoto");

            migrationBuilder.DropIndex(
                name: "IX_CatMoto_idTipoMoto",
                table: "CatMoto");

            migrationBuilder.DropIndex(
                name: "IX_CatMantenimiento_MotoCatalogoId",
                table: "CatMantenimiento");

            migrationBuilder.DropColumn(
                name: "idMarca",
                table: "CatMoto");

            migrationBuilder.DropColumn(
                name: "strDescripcion",
                table: "CatMantenimiento");

            migrationBuilder.RenameColumn(
                name: "idTipoMoto",
                table: "CatMoto",
                newName: "idUsuario");

            migrationBuilder.RenameColumn(
                name: "idMoto",
                table: "CatMoto",
                newName: "idMotoUsuario");

            migrationBuilder.RenameColumn(
                name: "idCatMantenimiento",
                table: "CatMantenimiento",
                newName: "idMantenimiento");

            migrationBuilder.AlterColumn<string>(
                name: "strTokenRecuperacion",
                table: "UsuUsuario",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strTelefono",
                table: "UsuUsuario",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strNombre",
                table: "UsuUsuario",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "strFotoPerfil",
                table: "UsuUsuario",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strCorreo",
                table: "UsuUsuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "strContrasena",
                table: "UsuUsuario",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strApellidoPaterno",
                table: "UsuUsuario",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "strApellidoMaterno",
                table: "UsuUsuario",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "idSexo",
                table: "UsuUsuario",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "strNotas",
                table: "UsuMantenimiento",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "CatMantenimientoidCatMantenimiento",
                table: "UsuMantenimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "strNm",
                table: "CatMoto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "strModelo",
                table: "CatMoto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "strHp",
                table: "CatMoto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "strDescripcion",
                table: "CatMoto",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "strCilindraje",
                table: "CatMoto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "imgFoto",
                table: "CatMoto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "UsuUsuarioidUsuario",
                table: "CatMoto",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "strMarca",
                table: "CatMoto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "strTipoDeMoto",
                table: "CatMoto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "CatMantenimiento",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuMantenimiento_CatMantenimientoidCatMantenimiento",
                table: "UsuMantenimiento",
                column: "CatMantenimientoidCatMantenimiento");

            migrationBuilder.CreateIndex(
                name: "IX_CatMoto_UsuUsuarioidUsuario",
                table: "CatMoto",
                column: "UsuUsuarioidUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_CatMoto_UsuUsuario_UsuUsuarioidUsuario",
                table: "CatMoto",
                column: "UsuUsuarioidUsuario",
                principalTable: "UsuUsuario",
                principalColumn: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuMantenimiento_CatMantenimiento_CatMantenimientoidCatMantenimiento",
                table: "UsuMantenimiento",
                column: "CatMantenimientoidCatMantenimiento",
                principalTable: "CatMantenimiento",
                principalColumn: "idMantenimiento");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuMantenimiento_CatMoto_idMotoUsuario",
                table: "UsuMantenimiento",
                column: "idMotoUsuario",
                principalTable: "CatMoto",
                principalColumn: "idMotoUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuUsuario_UsuCatSexo_idSexo",
                table: "UsuUsuario",
                column: "idSexo",
                principalTable: "UsuCatSexo",
                principalColumn: "idSexo");
        }
    }
}
