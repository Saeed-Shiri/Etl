using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarasaEtl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketMakingActivityLicenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketMakerNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyOrderThreshold = table.Column<long>(type: "bigint", nullable: false),
                    SellOrderThreshold = table.Column<long>(type: "bigint", nullable: false),
                    TradeThreshold = table.Column<long>(type: "bigint", nullable: false),
                    BuyRange = table.Column<float>(type: "real", nullable: false),
                    SellRange = table.Column<float>(type: "real", nullable: false),
                    LicenseStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FarasaRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ContractTypeId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketMakingActivityLicenses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketMakingActivityLicenses");
        }
    }
}
