using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Orga.Migrations
{
    public partial class AddImageData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageReference",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ImageId",
                table: "Articles",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ImageDatas_ImageId",
                table: "Articles",
                column: "ImageId",
                principalTable: "ImageDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ImageDatas_ImageId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "ImageDatas");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ImageId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "ImageReference",
                table: "Articles",
                type: "text",
                nullable: true);
        }
    }
}
