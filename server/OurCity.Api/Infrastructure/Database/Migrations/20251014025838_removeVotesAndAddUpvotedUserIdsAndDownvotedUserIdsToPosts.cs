using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurCity.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class removeVotesAndAddUpvotedUserIdsAndDownvotedUserIdsToPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Posts");

            migrationBuilder.AddColumn<List<int>>(
                name: "DownvotedUserIds",
                table: "Posts",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<List<int>>(
                name: "UpvotedUserIds",
                table: "Posts",
                type: "integer[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownvotedUserIds",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpvotedUserIds",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
