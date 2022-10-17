using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EfcoreApp.Infrastructure.Migrations
{
    public partial class manytomanypayload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battle_BattlesBattleId",
                table: "BattleSamurai");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraisId",
                table: "BattleSamurai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Battle",
                table: "Battle");

            migrationBuilder.RenameTable(
                name: "Battle",
                newName: "Battles");

            migrationBuilder.RenameColumn(
                name: "SamuraisId",
                table: "BattleSamurai",
                newName: "SamuraiId");

            migrationBuilder.RenameColumn(
                name: "BattlesBattleId",
                table: "BattleSamurai",
                newName: "BattleId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleSamurai_SamuraisId",
                table: "BattleSamurai",
                newName: "IX_BattleSamurai_SamuraiId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "BattleSamurai",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Battles",
                table: "Battles",
                column: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battles_BattleId",
                table: "BattleSamurai",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraiId",
                table: "BattleSamurai",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battles_BattleId",
                table: "BattleSamurai");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraiId",
                table: "BattleSamurai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Battles",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "BattleSamurai");

            migrationBuilder.RenameTable(
                name: "Battles",
                newName: "Battle");

            migrationBuilder.RenameColumn(
                name: "SamuraiId",
                table: "BattleSamurai",
                newName: "SamuraisId");

            migrationBuilder.RenameColumn(
                name: "BattleId",
                table: "BattleSamurai",
                newName: "BattlesBattleId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleSamurai_SamuraiId",
                table: "BattleSamurai",
                newName: "IX_BattleSamurai_SamuraisId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Battle",
                table: "Battle",
                column: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battle_BattlesBattleId",
                table: "BattleSamurai",
                column: "BattlesBattleId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Samurais_SamuraisId",
                table: "BattleSamurai",
                column: "SamuraisId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
