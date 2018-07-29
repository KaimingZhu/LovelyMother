using Microsoft.EntityFrameworkCore.Migrations;

namespace Motherlibrary.Migrations
{
    public partial class LocalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlackListProgresses",
                columns: table => new
                {
                    Uwp_ID = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(nullable: true),
                    ResetName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackListProgresses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(nullable: true),
                    Begin = table.Column<string>(nullable: true),
                    DefaultTime = table.Column<int>(nullable: false),
                    FinishTime = table.Column<int>(nullable: false),
                    Introduction = table.Column<string>(nullable: true),
                    FinishFlag = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackListProgresses");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
