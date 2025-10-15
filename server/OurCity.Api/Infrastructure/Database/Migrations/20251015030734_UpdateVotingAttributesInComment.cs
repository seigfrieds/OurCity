using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurCity.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVotingAttributesInComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UpvotedBy",
                table: "Comments",
                newName: "UpvotedUserIds");

            migrationBuilder.RenameColumn(
                name: "DownvotedBy",
                table: "Comments",
                newName: "DownvotedUserIds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpvotedUserIds",
                table: "Comments",
                newName: "UpvotedBy");

            migrationBuilder.RenameColumn(
                name: "DownvotedUserIds",
                table: "Comments",
                newName: "DownvotedBy");

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
