using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SceneSherpa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "reviews",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "age_rating",
                table: "media",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "media",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "media",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "user_user",
                columns: table => new
                {
                    friended_id = table.Column<int>(type: "integer", nullable: false),
                    friends_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_user", x => new { x.friended_id, x.friends_id });
                    table.ForeignKey(
                        name: "fk_user_user_users_friended_id",
                        column: x => x.friended_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_user_users_friends_id",
                        column: x => x.friends_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_user_friends_id",
                table: "user_user",
                column: "friends_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_user");

            migrationBuilder.DropColumn(
                name: "age_rating",
                table: "media");

            migrationBuilder.DropColumn(
                name: "description",
                table: "media");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "media");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "reviews",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
