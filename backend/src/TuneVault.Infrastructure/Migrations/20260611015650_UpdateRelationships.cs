using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuneVault.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaShares_MediaItems_MediaItemId",
                table: "MediaShares");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaShares_Playlists_PlaylistId",
                table: "MediaShares");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaShares_MediaItems_MediaItemId",
                table: "MediaShares",
                column: "MediaItemId",
                principalTable: "MediaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaShares_Playlists_PlaylistId",
                table: "MediaShares",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaShares_MediaItems_MediaItemId",
                table: "MediaShares");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaShares_Playlists_PlaylistId",
                table: "MediaShares");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaShares_MediaItems_MediaItemId",
                table: "MediaShares",
                column: "MediaItemId",
                principalTable: "MediaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaShares_Playlists_PlaylistId",
                table: "MediaShares",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
