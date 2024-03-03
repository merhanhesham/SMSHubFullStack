using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSHub.Repository.Data.Migrations
{
    public partial class userstablerelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SenderIdUsers",
                columns: table => new
                {
                    SenderIdsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderIdUsers", x => new { x.SenderIdsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SenderIdUsers_SenderIds_SenderIdsId",
                        column: x => x.SenderIdsId,
                        principalTable: "SenderIds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SenderIdUsers_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SenderIdUsers_UsersId",
                table: "SenderIdUsers",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenderIdUsers");
        }
    }
}
