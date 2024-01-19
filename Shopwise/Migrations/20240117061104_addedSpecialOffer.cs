using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopwise.Migrations
{
    public partial class addedSpecialOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferSections",
                table: "OfferSections");

            migrationBuilder.RenameTable(
                name: "OfferSections",
                newName: "OfferSection");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferSection",
                table: "OfferSection",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferSection",
                table: "OfferSection");

            migrationBuilder.RenameTable(
                name: "OfferSection",
                newName: "OfferSections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferSections",
                table: "OfferSections",
                column: "Id");
        }
    }
}
