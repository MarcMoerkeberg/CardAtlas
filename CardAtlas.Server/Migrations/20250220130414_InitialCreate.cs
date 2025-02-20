using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CardAtlas.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrameLayouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameLayouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PrintCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Legalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintFinishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintFinishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rarities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rarities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    MtgoCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ArenaCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ParentSetCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    Block = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    BlockCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    SetTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCardsInSet = table.Column<int>(type: "int", nullable: false),
                    IsDigitalOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsFoilOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsNonFoilOnly = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_SetTypes_SetTypeId",
                        column: x => x.SetTypeId,
                        principalTable: "SetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardGameType",
                columns: table => new
                {
                    CardsId = table.Column<long>(type: "bigint", nullable: false),
                    GameTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardGameType", x => new { x.CardsId, x.GameTypesId });
                    table.ForeignKey(
                        name: "FK_CardGameType_GameTypes_GameTypesId",
                        column: x => x.GameTypesId,
                        principalTable: "GameTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageTypeId = table.Column<int>(type: "int", nullable: false),
                    ImageFormatId = table.Column<int>(type: "int", nullable: false),
                    ImageStatusId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardImages_ImageFormats_ImageFormatId",
                        column: x => x.ImageFormatId,
                        principalTable: "ImageFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardImages_ImageStatuses_ImageStatusId",
                        column: x => x.ImageStatusId,
                        principalTable: "ImageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardImages_ImageTypes_ImageTypeId",
                        column: x => x.ImageTypeId,
                        principalTable: "ImageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardLegalities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    StandardLegalityId = table.Column<int>(type: "int", nullable: false),
                    FutureLegalityId = table.Column<int>(type: "int", nullable: false),
                    HistoricLegalityId = table.Column<int>(type: "int", nullable: false),
                    TimelessLegalityId = table.Column<int>(type: "int", nullable: false),
                    GladiatorLegalityId = table.Column<int>(type: "int", nullable: false),
                    PioneerLegalityId = table.Column<int>(type: "int", nullable: false),
                    ExplorerLegalityId = table.Column<int>(type: "int", nullable: false),
                    ModernLegalityId = table.Column<int>(type: "int", nullable: false),
                    LegacyLegalityId = table.Column<int>(type: "int", nullable: false),
                    PauperLegalityId = table.Column<int>(type: "int", nullable: false),
                    VintageLegalityId = table.Column<int>(type: "int", nullable: false),
                    PennyLegalityId = table.Column<int>(type: "int", nullable: false),
                    CommanderLegalityId = table.Column<int>(type: "int", nullable: false),
                    OatBreakerLegalityId = table.Column<int>(type: "int", nullable: false),
                    StandardBrawlLegalityId = table.Column<int>(type: "int", nullable: false),
                    BrawlLegalityId = table.Column<int>(type: "int", nullable: false),
                    AlchemyLegalityId = table.Column<int>(type: "int", nullable: false),
                    PauperCommanderLegalityId = table.Column<int>(type: "int", nullable: false),
                    DuelCommanderLegalityId = table.Column<int>(type: "int", nullable: false),
                    OldSchoolLegalityId = table.Column<int>(type: "int", nullable: false),
                    PreModernlLegalityId = table.Column<int>(type: "int", nullable: false),
                    PreDHLegalityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLegalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_AlchemyLegalityId",
                        column: x => x.AlchemyLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_BrawlLegalityId",
                        column: x => x.BrawlLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_CommanderLegalityId",
                        column: x => x.CommanderLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_DuelCommanderLegalityId",
                        column: x => x.DuelCommanderLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_ExplorerLegalityId",
                        column: x => x.ExplorerLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_FutureLegalityId",
                        column: x => x.FutureLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_GladiatorLegalityId",
                        column: x => x.GladiatorLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_HistoricLegalityId",
                        column: x => x.HistoricLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_LegacyLegalityId",
                        column: x => x.LegacyLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_ModernLegalityId",
                        column: x => x.ModernLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_OatBreakerLegalityId",
                        column: x => x.OatBreakerLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_OldSchoolLegalityId",
                        column: x => x.OldSchoolLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_PauperCommanderLegalityId",
                        column: x => x.PauperCommanderLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_PauperLegalityId",
                        column: x => x.PauperLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_PennyLegalityId",
                        column: x => x.PennyLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_PioneerLegalityId",
                        column: x => x.PioneerLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_PreDHLegalityId",
                        column: x => x.PreDHLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_PreModernlLegalityId",
                        column: x => x.PreModernlLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_StandardBrawlLegalityId",
                        column: x => x.StandardBrawlLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_StandardLegalityId",
                        column: x => x.StandardLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_TimelessLegalityId",
                        column: x => x.TimelessLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_VintageLegalityId",
                        column: x => x.VintageLegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OracleText = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    TypeLine = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    FlavorText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManaCost = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConvertedManaCost = table.Column<decimal>(type: "decimal(8,1)", precision: 8, scale: 1, nullable: true),
                    Power = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Toughness = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    SetId = table.Column<int>(type: "int", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    RarityId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ColorIdentity = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CardLegalityId = table.Column<long>(type: "bigint", nullable: false),
                    Loyalty = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CollectorNumber = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    FrameLayoutId = table.Column<int>(type: "int", nullable: false),
                    PromoTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCardId = table.Column<long>(type: "bigint", nullable: true),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    CanBeFoundInBoosters = table.Column<bool>(type: "bit", nullable: false),
                    IsDigitalOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsFullArt = table.Column<bool>(type: "bit", nullable: false),
                    IsOversized = table.Column<bool>(type: "bit", nullable: false),
                    IsPromo = table.Column<bool>(type: "bit", nullable: false),
                    IsReprint = table.Column<bool>(type: "bit", nullable: false),
                    IsTextless = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_CardLegalities_CardLegalityId",
                        column: x => x.CardLegalityId,
                        principalTable: "CardLegalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_ParentCardId",
                        column: x => x.ParentCardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cards_FrameLayouts_FrameLayoutId",
                        column: x => x.FrameLayoutId,
                        principalTable: "FrameLayouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Rarities_RarityId",
                        column: x => x.RarityId,
                        principalTable: "Rarities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    FoilPrice = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    PurchaseUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPrices_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPrices_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardPrintFinish",
                columns: table => new
                {
                    CardsId = table.Column<long>(type: "bigint", nullable: false),
                    PrintFinishesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrintFinish", x => new { x.CardsId, x.PrintFinishesId });
                    table.ForeignKey(
                        name: "FK_CardPrintFinish_Cards_CardsId",
                        column: x => x.CardsId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPrintFinish_PrintFinishes_PrintFinishesId",
                        column: x => x.PrintFinishesId,
                        principalTable: "PrintFinishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Usd" },
                    { 2, "Eur" },
                    { 3, "Tix" }
                });

            migrationBuilder.InsertData(
                table: "FrameLayouts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Year1993" },
                    { 2, "Year1997" },
                    { 3, "Year2003" },
                    { 4, "Year2015" },
                    { 5, "Future" }
                });

            migrationBuilder.InsertData(
                table: "GameTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Paper" },
                    { 2, "Arena" },
                    { 3, "Mtgo" }
                });

            migrationBuilder.InsertData(
                table: "ImageFormats",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "PNG" },
                    { 2, "JPG" }
                });

            migrationBuilder.InsertData(
                table: "ImageStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Missing" },
                    { 2, "Placeholder" },
                    { 3, "LowResolution" },
                    { 4, "HighResolutionScan" }
                });

            migrationBuilder.InsertData(
                table: "ImageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Png" },
                    { 2, "BorderCrop" },
                    { 3, "ArtCrop" },
                    { 4, "Large" },
                    { 5, "Normal" },
                    { 6, "Small" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name", "PrintCode" },
                values: new object[,]
                {
                    { -1, "NA", "NotImplemented", null },
                    { 1, "en", "English", "en" },
                    { 2, "es", "Spanish", "sp" },
                    { 3, "fr", "French", "fr" },
                    { 4, "de", "German", "de" },
                    { 5, "it", "Italian", "it" },
                    { 6, "pt", "Portuguese", "pt" },
                    { 7, "ja", "Japanese", "jp" },
                    { 8, "ko", "Korean", "kr" },
                    { 9, "ru", "Russian", "ru" },
                    { 10, "zhs", "SimplifiedChinese", "cs" },
                    { 11, "zht", "TraditionalChinese", "ct" },
                    { 12, "he", "Hebrew", null },
                    { 13, "la", "Latin", null },
                    { 14, "grc", "AncientGreek", null },
                    { 15, "ar", "Arabic", null },
                    { 16, "sa", "Sanskrit", null },
                    { 17, "ph", "Phyrexian", "ph" }
                });

            migrationBuilder.InsertData(
                table: "PrintFinishes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Foil" },
                    { 2, "NonFoil" },
                    { 3, "Etched" }
                });

            migrationBuilder.InsertData(
                table: "Rarities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Common" },
                    { 2, "Uncommon" },
                    { 3, "Rare" },
                    { 4, "Special" },
                    { 5, "Mythic" },
                    { 6, "Bonus" }
                });

            migrationBuilder.InsertData(
                table: "SetTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Core" },
                    { 2, "Expansion" },
                    { 3, "Masters" },
                    { 4, "Alchemy" },
                    { 5, "Masterpiece" },
                    { 6, "Arsenal" },
                    { 7, "FromTheVault" },
                    { 8, "Spellbook" },
                    { 9, "PremiumDeck" },
                    { 10, "DuelDeck" },
                    { 11, "DraftInnovation" },
                    { 12, "TreasureChest" },
                    { 13, "Commander" },
                    { 14, "Planechase" },
                    { 15, "Archenemy" },
                    { 16, "Vanguard" },
                    { 17, "Funny" },
                    { 18, "Starter" },
                    { 19, "Box" },
                    { 20, "Promo" },
                    { 21, "Token" },
                    { 22, "Memorabilia" },
                    { 23, "MiniGame" }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "CurrencyId", "Name" },
                values: new object[,]
                {
                    { -1, -1, "NotImplemented" },
                    { 1, 1, "TcgPlayer" },
                    { 2, 2, "CardMarket" },
                    { 3, 3, "CardHoarder" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardGameType_GameTypesId",
                table: "CardGameType",
                column: "GameTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_CardId",
                table: "CardImages",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_ImageFormatId",
                table: "CardImages",
                column: "ImageFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_ImageStatusId",
                table: "CardImages",
                column: "ImageStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_ImageTypeId",
                table: "CardImages",
                column: "ImageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_AlchemyLegalityId",
                table: "CardLegalities",
                column: "AlchemyLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_BrawlLegalityId",
                table: "CardLegalities",
                column: "BrawlLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_CardId",
                table: "CardLegalities",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_CommanderLegalityId",
                table: "CardLegalities",
                column: "CommanderLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_DuelCommanderLegalityId",
                table: "CardLegalities",
                column: "DuelCommanderLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_ExplorerLegalityId",
                table: "CardLegalities",
                column: "ExplorerLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_FutureLegalityId",
                table: "CardLegalities",
                column: "FutureLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_GladiatorLegalityId",
                table: "CardLegalities",
                column: "GladiatorLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_HistoricLegalityId",
                table: "CardLegalities",
                column: "HistoricLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_LegacyLegalityId",
                table: "CardLegalities",
                column: "LegacyLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_ModernLegalityId",
                table: "CardLegalities",
                column: "ModernLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_OatBreakerLegalityId",
                table: "CardLegalities",
                column: "OatBreakerLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_OldSchoolLegalityId",
                table: "CardLegalities",
                column: "OldSchoolLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_PauperCommanderLegalityId",
                table: "CardLegalities",
                column: "PauperCommanderLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_PauperLegalityId",
                table: "CardLegalities",
                column: "PauperLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_PennyLegalityId",
                table: "CardLegalities",
                column: "PennyLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_PioneerLegalityId",
                table: "CardLegalities",
                column: "PioneerLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_PreDHLegalityId",
                table: "CardLegalities",
                column: "PreDHLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_PreModernlLegalityId",
                table: "CardLegalities",
                column: "PreModernlLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_StandardBrawlLegalityId",
                table: "CardLegalities",
                column: "StandardBrawlLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_StandardLegalityId",
                table: "CardLegalities",
                column: "StandardLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_TimelessLegalityId",
                table: "CardLegalities",
                column: "TimelessLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_VintageLegalityId",
                table: "CardLegalities",
                column: "VintageLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_CardId",
                table: "CardPrices",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_VendorId",
                table: "CardPrices",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrintFinish_PrintFinishesId",
                table: "CardPrintFinish",
                column: "PrintFinishesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ArtistId",
                table: "Cards",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardLegalityId",
                table: "Cards",
                column: "CardLegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_FrameLayoutId",
                table: "Cards",
                column: "FrameLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_LanguageId",
                table: "Cards",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ParentCardId",
                table: "Cards",
                column: "ParentCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RarityId",
                table: "Cards",
                column: "RarityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SetId",
                table: "Cards",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_SetTypeId",
                table: "Sets",
                column: "SetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CurrencyId",
                table: "Vendors",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardGameType_Cards_CardsId",
                table: "CardGameType",
                column: "CardsId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardImages_Cards_CardId",
                table: "CardImages",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardLegalities_Cards_CardId",
                table: "CardLegalities",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardLegalities_Cards_CardId",
                table: "CardLegalities");

            migrationBuilder.DropTable(
                name: "CardGameType");

            migrationBuilder.DropTable(
                name: "CardImages");

            migrationBuilder.DropTable(
                name: "CardPrices");

            migrationBuilder.DropTable(
                name: "CardPrintFinish");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "GameTypes");

            migrationBuilder.DropTable(
                name: "ImageFormats");

            migrationBuilder.DropTable(
                name: "ImageStatuses");

            migrationBuilder.DropTable(
                name: "ImageTypes");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "PrintFinishes");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "CardLegalities");

            migrationBuilder.DropTable(
                name: "FrameLayouts");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Rarities");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Legalities");

            migrationBuilder.DropTable(
                name: "SetTypes");
        }
    }
}
